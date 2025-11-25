using STARTD.Core.Stage;
using STARTD.Game.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace STARTD.Game.Enemy
{
    public class EnemyStateAttack : BaseEnemyState
    {
        private float attackDelay = 0.5f;
        private float elpsedTime = 0.0f;
        private int dmg = 10;

        public override void Enter(Action<Type> callback)
        {
            base.Enter(callback);
        }

        public override void Execute()
        {
            base.Execute();

            if (elpsedTime > attackDelay)
            {
                Attack();
                elpsedTime = 0;
            }

            elpsedTime += Time.deltaTime;
        }

        private void Attack()
        {
            GameScene.Singleton.PlayerLifeDown(dmg);
            Vector3Int cellPos = StageManager.Singleton.TileMap.WorldToCell(transform.position);

            int cx = cellPos.x - StageManager.Singleton.TileMap.cellBounds.xMin;
            int cy = cellPos.y - StageManager.Singleton.TileMap.cellBounds.yMin;

            foreach (var towerRange in StageManager.Singleton.CurStage?.towerRangeIdxs?[cx, cy])
            {
                towerRange.SetTarget(behaviour);
            }
        }
    }
}