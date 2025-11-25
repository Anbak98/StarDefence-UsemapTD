using STARTD.Common;
using STARTD.Core.Stage;
using STARTD.Game.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STARTD.Game.Tower
{
    public class TowerWrapper
    {
        public int mx;
        public int my;
        public TowerBehaviour tower;
    }

    public class TowerManager : SingletonBehaviour<TowerManager>
    {
        [SerializeField] private TowerSpawner spawner;
        private List<TowerBehaviour> spawnedTowers = new();
        private int nx = -1;
        private int ny = -1;

        private Dictionary<Type, List<TowerWrapper>> towersMap = new Dictionary<Type, List<TowerWrapper>>()
        {
            {typeof(NormalTower), new List<TowerWrapper>()},
            {typeof(HighTower), new List<TowerWrapper>()},
            {typeof(BigBigTower), new List<TowerWrapper>()}
        };

        private void Update()
        {
            foreach(var towr in spawnedTowers)
            {
                towr.Attack();
            }
        }

        public void TryBuildTower(int x, int y)
        {
            // 타일맵 Bounds 가져오기
            BoundsInt bounds = StageManager.Singleton.TileMap.cellBounds;

            // 배열 인덱스로 변환 (0,0부터 시작)
            int mx = x - bounds.xMin;
            int my = y - bounds.yMin;
            nx = mx;
            ny = my;

            if (StageManager.Singleton.CurStage.towerIdxs[nx, ny] == 0)
            {
                // 타일 월드 좌표 가져오기 (정중앙)
                Vector3 worldPos =
                    StageManager.Singleton.TileMap.GetCellCenterWorld(new Vector3Int(x, y, 0));

                GameScene.Singleton.SetBuyButton(250, () => SpawnTower<NormalTower>(nx, ny), worldPos);
            }
            else if (StageManager.Singleton.CurStage.towerIdxs[nx, ny] == 1)
            {
                // 타일 월드 좌표 가져오기 (정중앙)
                Vector3 worldPos =
                    StageManager.Singleton.TileMap.GetCellCenterWorld(new Vector3Int(x, y, 0));

                bool valid = false;
                if (towersMap.TryGetValue(typeof(NormalTower), out var value))
                {
                    if(value.Count > 1)
                    {
                        valid = true;
                    }
                }
                GameScene.Singleton.SetBuyButton(400, () => CombineTower<NormalTower , HighTower>(nx, ny), worldPos, valid);
            }
            else if (StageManager.Singleton.CurStage.towerIdxs[nx, ny] == 2)
            {
                // 타일 월드 좌표 가져오기 (정중앙)
                Vector3 worldPos =
                    StageManager.Singleton.TileMap.GetCellCenterWorld(new Vector3Int(x, y, 0));

                bool valid = false;
                if (towersMap.TryGetValue(typeof(HighTower), out var value))
                {
                    if (value.Count > 1)
                    {
                        valid = true;
                    }
                }
                GameScene.Singleton.SetBuyButton(1000, () => CombineTower<HighTower, BigBigTower>(nx, ny), worldPos, valid);
            }
        }

        public void SpawnTower<T>(int x, int y) where T : TowerBehaviour
        {
            TowerBehaviour tower = spawner.SpawnTower<T>(x, y);
            StageManager.Singleton.CurStage.towerIdxs[x, y] += 1;
            Debug.Log($"{x}, {y}, {StageManager.Singleton.CurStage.towerIdxs[x, y]}");
            spawnedTowers.Add(tower);
            AddTowerRange(new TowerWrapper() { mx = x, my = y, tower = tower });
            towersMap[typeof(T)].Add(new TowerWrapper() { mx = x, my = y , tower = tower});
            GameScene.Singleton.DisableBuyButton();
            EnemyManager.Singleton.ForceSetTargets();
        }

        public void CombineTower<T, S>(int x, int y) where T : TowerBehaviour where S : TowerBehaviour
        {
            if(towersMap.TryGetValue(typeof(T), out var value))
            {
                if(value.Count > 1)
                {
                    TowerWrapper one = value[0];
                    TowerWrapper two = value[1];
                    foreach (var wrapper in value)
                    {
                        if(wrapper.mx == x && wrapper.my == y)
                        {
                            one = wrapper;
                        }
                        else
                        {
                            two = wrapper;
                        }
                    }

                    //StageManager.Singleton.CurStage.towerIdxs[one.mx, one.my] = 0;
                    StageManager.Singleton.CurStage.towerIdxs[two.mx, two.my] = 0;
                    value.Remove(one);
                    value.Remove(two);
                    spawnedTowers.Remove(one.tower);
                    spawnedTowers.Remove(two.tower);
                    RemoveTowerRange(one);
                    RemoveTowerRange(two);
                    Destroy(one.tower.gameObject);
                    Destroy(two.tower.gameObject);
                    SpawnTower<S>(x, y);
                }
            }
        }

        public void AddTowerRange(TowerWrapper tower)
        {
            foreach(var dxy in tower.tower.Range)
            {
                int nx = tower.mx + dxy.x;
                int ny = tower.my + dxy.y;

                if (nx < 0 || ny < 0 || 
                    nx >= StageManager.Singleton.CurStage.towerRangeIdxs.GetLength(0) ||
                    ny >= StageManager.Singleton.CurStage.towerRangeIdxs.GetLength(1)) continue;

                StageManager.Singleton.CurStage.towerRangeIdxs[nx, ny].Add(tower.tower);
            }
        }

        public void RemoveTowerRange(TowerWrapper tower)
        {
            foreach (var dxy in tower.tower.Range)
            {
                int nx = tower.mx + dxy.x;
                int ny = tower.my + dxy.y;

                if (nx < 0 || ny < 0 ||
                    nx >= StageManager.Singleton.CurStage.towerRangeIdxs.GetLength(0) ||
                    ny >= StageManager.Singleton.CurStage.towerRangeIdxs.GetLength(1)) continue;

                StageManager.Singleton.CurStage.towerRangeIdxs[nx, ny].Remove(tower.tower);
            }
        }
    }
}
