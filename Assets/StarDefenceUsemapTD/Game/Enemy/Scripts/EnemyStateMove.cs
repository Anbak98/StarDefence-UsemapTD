using STARTD.Core.Stage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace STARTD.Game.Enemy
{
    public class EnemyStateMove : MonoBehaviour
    {
        public float moveSpeed = 2f;

        [SerializeField] private Vector3 targetWorldPos;
        [SerializeField] private bool hasTarget = false;

        void Update()
        {
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
            if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
            {
                hasTarget = false;
            }
        }

        private void SetNextTarget()
        {
            Vector3Int cellPos = StageManager.Singleton.TileMap.WorldToCell(transform.position);

            int x = cellPos.x - StageManager.Singleton.TileMap.cellBounds.xMin;
            int y = cellPos.y - StageManager.Singleton.TileMap.cellBounds.yMin;

            // Navigation 얻기
            Navigation nav = StageManager.Singleton.CurStage.navigations[x, y];

            // 목적지가 없으면 멈춤
            if (nav.nx == -1)
                return;

            
            Vector3 nextCell = StageManager.Singleton.TileMap.CellToWorld(
                        new Vector3Int(
                            nav.nx + StageManager.Singleton.TileMap.cellBounds.xMin,
                            nav.ny + StageManager.Singleton.TileMap.cellBounds.yMin, 
                            0)
                    );
            // 타일의 정중앙으로 이동
            nextCell += new Vector3(StageManager.Singleton.TileMap.cellSize.x, StageManager.Singleton.TileMap.cellSize.y, 0) * 0.5f;


            targetWorldPos = nextCell;
            hasTarget = true;
        }
    }
}