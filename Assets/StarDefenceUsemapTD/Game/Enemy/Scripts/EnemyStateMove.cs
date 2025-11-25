using STARTD.Core.Stage;
using System;
using UnityEngine;

namespace STARTD.Game.Enemy
{
    public class EnemyStateMove : BaseEnemyState
    {
        private float moveSpeed = 2f;
        private Vector3 targetWorldPos;
        private bool hasTarget = false;

        public override void Enter(Action<Type> callback)
        {
            base.Enter(callback);

            Debug.Log(transform.position);
        }

        public override void Execute()
        {
            base.Execute();
            Move();
        }

        private void Move()
        {
            // 아직 다음 목적지가 없으면 설정
            if (!hasTarget)
            {
                SetNextTarget();
                return;
            }

            // 이동
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetWorldPos,
                moveSpeed * Time.deltaTime
            );

            // 목적지 도착?
            if (Vector3.Distance(transform.position, targetWorldPos) < 0.1f)
            {
                hasTarget = false;
            }
        }

        private void SetNextTarget()
        {
            Vector3Int cellPos = StageManager.Singleton.TileMap.WorldToCell(transform.position);

            int cx = cellPos.x - StageManager.Singleton.TileMap.cellBounds.xMin;
            int cy = cellPos.y - StageManager.Singleton.TileMap.cellBounds.yMin;

            foreach (var towerRange in StageManager.Singleton.CurStage?.towerRangeIdxs?[cx, cy])
            {
                towerRange.ClearTarget(behaviour);
            }

            Navigation nav = StageManager.Singleton.CurStage.navigations[cx, cy];

            // 목적지 도착 → 공격 상태 전환
            if (nav.nx == StageManager.Singleton.CurStage.player.x &&
                nav.ny == StageManager.Singleton.CurStage.player.y)
            {
                onDone?.Invoke(typeof(EnemyStateAttack));
                return;
            }

            // 이동할 수 없는 경우
            if (nav.nx < 0 || nav.ny < 0)
                return;

            int nx = nav.nx;
            int ny = nav.ny;

            foreach (var towerRange in StageManager.Singleton.CurStage?.towerRangeIdxs?[nx, ny])
            {
                towerRange.SetTarget(behaviour);
            }

            Vector3 nextCell = StageManager.Singleton.TileMap.CellToWorld(
                new Vector3Int(
                    nx + StageManager.Singleton.TileMap.cellBounds.xMin,
                    ny + StageManager.Singleton.TileMap.cellBounds.yMin,
                    0));

            nextCell += StageManager.Singleton.TileMap.cellSize * 0.5f;

            targetWorldPos = nextCell;
            hasTarget = true;
        }
    }
}