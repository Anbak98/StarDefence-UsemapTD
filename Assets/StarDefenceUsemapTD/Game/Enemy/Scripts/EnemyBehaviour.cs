using STARTD.Core.Stage;
using STARTD.Game.Economy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STARTD.Game.Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private EnemyFSM fsm;
        [SerializeField] private int life;
        [SerializeField] private int dropGold;
        public Life Life { get; private set; }
        public int DropGold => dropGold;

        public void Awake()
        {
            Life = new Life(life, life);
            fsm.Init(this);
        }

        public void Start()
        {
            fsm.Enter();
        }

        public void Update()
        {
            fsm.Execute();
        }

        public bool LifeDown(int amount)
        {
            Life.Remove(amount, out bool IsZero);
            HealthBarManager.Singleton.SetHealth(transform, Life.Cur, Life.Max);

            Debug.Log(Life.Cur);

            if (IsZero)
            {
                EnemyManager.Singleton.EnemyKilled(this);
            }

            return IsZero;
        }

        public void ForceSetTarget()
        {

            Vector3Int cellPos = StageManager.Singleton.TileMap.WorldToCell(transform.position);

            int cx = cellPos.x - StageManager.Singleton.TileMap.cellBounds.xMin;
            int cy = cellPos.y - StageManager.Singleton.TileMap.cellBounds.yMin;

            foreach (var towerRange in StageManager.Singleton.CurStage?.towerRangeIdxs?[cx, cy])
            {
                towerRange.SetTarget(this);
            }
        }
    }
}