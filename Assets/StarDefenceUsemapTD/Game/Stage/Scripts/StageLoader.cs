using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace STARTD.Core.Stage
{

    public class StageLoader : IStageLoader
    {
        private TileBase[] tileset; // 전체 타일 목록

        public StageLoader(TileBase[] tileset)
        {
            this.tileset = tileset;
        }

        public Stage LoadStage(Tilemap tiles)
        {
            // 1. Tilemap Bounds 계산
            BoundsInt tileBounds = tiles.cellBounds;
            int width = tileBounds.size.x;
            int height = tileBounds.size.y;

            int[,] tileIdxs = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3Int pos = new Vector3Int(tileBounds.xMin + x, tileBounds.yMin + y, 0);

                    // 타일 인덱스
                    TileBase tile = tiles.GetTile(pos);
                    tileIdxs[x, y] = tile != null ? System.Array.IndexOf(tileset, tile) : -1;
                }
            }

            Navigation[,] navigations = CalcNavi(tileIdxs);
            List<Navigation> portalNavigations = new();

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    if (tileIdxs[x, y] == (int)ETileBase.Portal)
                    {
                        portalNavigations.Add(navigations[x,y]);
                    }
                }
            }

            return new Stage(tileIdxs, navigations, portalNavigations);
        }

        // 런타임 이전에 계산하면 좋겠는 걸. c++ constexpr같은 문법? 혹은 json?
        private Navigation[,] CalcNavi(int[,] tileIdxs)
        {
            int width = tileIdxs.GetLength(0);
            int height = tileIdxs.GetLength(1);

            int[] dx = { 0, 0, 1, -1 };
            int[] dy = { 1, -1, 0, 0 };

            Navigation[,] navigations = new Navigation[width, height];
            bool[,] visited = new bool[width, height];
            Vector2Int[,] parent = new Vector2Int[width, height];

            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            // ✅ 1) 모든 Base를 BFS 시작점으로
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    if (tileIdxs[x, y] == (int)ETileBase.Base)
                    {
                        queue.Enqueue(new Vector2Int(x, y));
                        visited[x, y] = true;
                        parent[x, y] = new Vector2Int(-1, -1); // Base는 끝지점
                    }
                }
            }

            // ✅ 2) BFS 확장 (모든 Load + Base로 퍼짐)
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();

                for (int i = 0; i < 4; ++i)
                {
                    int nx = cur.x + dx[i];
                    int ny = cur.y + dy[i];

                    if (nx < 0 || ny < 0 || nx >= width || ny >= height) continue;
                    if (visited[nx, ny]) continue;

                    // Load, Base, Portal 모두 통과 가능
                    if (tileIdxs[nx, ny] != (int)ETileBase.Load &&
                        tileIdxs[nx, ny] != (int)ETileBase.Base &&
                        tileIdxs[nx, ny] != (int)ETileBase.Portal)
                        continue;


                    visited[nx, ny] = true;
                    parent[nx, ny] = cur;
                    queue.Enqueue(new Vector2Int(nx, ny));
                }
            }

            // ✅ 3) 모든 visited 칸에 대해 Base로 향하는 다음 방향 저장
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    navigations[x, y] = new Navigation()
                    {
                        x = x,
                        y = y,
                        nx = -1,
                        ny = -1
                    };
                    if (!visited[x, y]) continue; // Base에 도달 불가

                    Vector2Int next = parent[x, y];

                    // Base는 parent 없음
                    if (next.x == -1) continue;

                    navigations[x, y] = new Navigation()
                    {
                        x = x,
                        y = y,
                        nx = next.x,
                        ny = next.y
                    };
                }
            }

            return navigations;
        }

    }
}