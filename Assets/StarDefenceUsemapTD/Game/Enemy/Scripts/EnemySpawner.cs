using STARTD.Core.Stage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Wave
{
    public List<WaveInfo> infos;
}

[Serializable]
public class WaveInfo
{
    public int spawnIndex;
    public int spawnCount;
}

namespace STARTD.Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [field: SerializeField] public List<Wave> Waves { get; private set; }
        private Queue<Wave> queue = new Queue<Wave>();

        public void Awake()
        {
            foreach (var wave in Waves)
            {
                queue.Enqueue(wave);
            }
        }

        public async void Spawn()
        {
            // 다음 스폰 전 대기 (0.5초)
            await Task.Delay(500);
            if (queue.Count == 0) return;

            Wave curWave = queue.Dequeue();

            // while -> spawnCount가 남은 동안 반복
            while (curWave.infos.Count > 0)
            {
                for (int i = curWave.infos.Count - 1; i >= 0; i--)
                {
                    var waveInfo = curWave.infos[i];

                    // Portal 좌표 가져오기
                    Navigation spawnPortal = StageManager.Singleton.CurStage.portalNavigations[waveInfo.spawnIndex];
                    Vector3 spawnPos = StageManager.Singleton.TileMap.CellToWorld(
                        new Vector3Int(spawnPortal.x + StageManager.Singleton.TileMap.cellBounds.xMin, spawnPortal.y + StageManager.Singleton.TileMap.cellBounds.yMin, 0)
                    );
                    // 타일의 정중앙으로 이동
                    spawnPos += new Vector3(StageManager.Singleton.TileMap.cellSize.x, StageManager.Singleton.TileMap.cellSize.y, 0) * 0.5f;

                    // Enemy 생성
                    Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

                    // spawnCount 감소
                    waveInfo.spawnCount--;
                    if (waveInfo.spawnCount <= 0)
                    {
                        curWave.infos.RemoveAt(i);
                    }
                    else
                    {
                        curWave.infos[i] = waveInfo; // 수정된 spawnCount 반영
                    }
                }

                // 다음 스폰 전 대기 (0.5초)
                await Task.Delay(500);
            }
        }

    }
}