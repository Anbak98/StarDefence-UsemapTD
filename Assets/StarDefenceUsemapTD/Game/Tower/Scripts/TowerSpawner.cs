using STARTD.Core.Stage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STARTD.Game.Tower
{
    public class TowerSpawner : MonoBehaviour
    {
        [SerializeField] private List<TowerBehaviour> towers;
        private Dictionary<Type, TowerBehaviour> towersMap = new();

        // Start is called before the first frame update
        void Awake()
        {
            foreach (var tower in towers)
            {
                towersMap.Add(tower.GetType(), tower);
            }
        }


        public TowerBehaviour GetTower<T>() where T : TowerBehaviour
        {
            if(towersMap.TryGetValue(typeof(T), out var tower))
            {
                return tower;
            }

            return null;
        }

        public TowerBehaviour SpawnTower<T>(int x, int y) where T : TowerBehaviour
        {
            Vector3 spawnPos = StageManager.Singleton.TileMap.CellToWorld(
                        new Vector3Int(
                            x + StageManager.Singleton.TileMap.cellBounds.xMin, 
                            y + StageManager.Singleton.TileMap.cellBounds.yMin, 
                            0)
                    );
            // 타일의 정중앙으로 이동
            spawnPos += new Vector3(StageManager.Singleton.TileMap.cellSize.x, StageManager.Singleton.TileMap.cellSize.y, 0) * 0.5f;
            spawnPos += new Vector3(0, 0.3f, 0);
            TowerBehaviour tower = Instantiate(towersMap[typeof(T)].gameObject, spawnPos, Quaternion.identity).GetComponent<TowerBehaviour>();
            return tower;
        }
    }
}