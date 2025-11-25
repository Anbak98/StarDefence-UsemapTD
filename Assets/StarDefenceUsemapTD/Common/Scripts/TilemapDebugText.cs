using STARTD.Core.Stage;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDebugText
{
    public void Init(Tilemap tilemap, Stage stage)
    {
        int width = stage.tileIdxs.GetLength(0);
        int height = stage.tileIdxs.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int value = stage.tileIdxs[x, y];
                if (value < 0) continue;

                Vector3Int cellPos = new Vector3Int(
                    x + tilemap.cellBounds.xMin,
                    y + tilemap.cellBounds.yMin,
                    0
                );
                Vector3 worldPos = tilemap.CellToWorld(cellPos);
                // 타일의 정중앙으로 이동
                worldPos += new Vector3(tilemap.cellSize.x, tilemap.cellSize.y, 0) * 0.5f;

                GameObject go = new GameObject($"TileText_{x}_{y}");
                go.transform.position = worldPos;

                var text = go.AddComponent<TextMeshPro>();
                text.text = $"({stage.navigations[x, y].x}, {stage.navigations[x, y].y}) \n > ({stage.navigations[x, y].nx}, {stage.navigations[x,y].ny})";

                text.fontSize = 3;
                text.alignment = TextAlignmentOptions.Center;

                text.color = Color.red;
            }
        }
    }
}
