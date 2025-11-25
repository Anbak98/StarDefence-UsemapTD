using UnityEngine;
using UnityEngine.Tilemaps;

namespace STARTD.Core.Stage
{
    public class StageCreator : IStageCreator
    {
        private TileBase[] tileset; // 전체 타일 목록

        public StageCreator(TileBase[] tileset)
        {
            this.tileset = tileset;
        }

        public bool ChangeTile(int wx, int wy, int tileIdx)
        {
            // 타일맵 Bounds 가져오기
            BoundsInt bounds = StageManager.Singleton.TileMap.cellBounds;

            // 배열 인덱스로 변환 (0,0부터 시작)
            int mx = wx - bounds.xMin;
            int my = wy - bounds.yMin;
            StageManager.Singleton.CurStage.tileIdxs[mx, my] = tileIdx;



            StageManager.Singleton.TileMap.SetTile(
                new Vector3Int(wx, wy, 0),
                tileset[tileIdx]
            );

            StageManager.Singleton.TileMap.RefreshTile(new Vector3Int(wx, wy, 0));

            return true;
        }
    }
}