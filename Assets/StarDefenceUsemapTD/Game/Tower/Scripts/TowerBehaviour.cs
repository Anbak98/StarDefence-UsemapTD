using STARTD.Common;
using STARTD.Game.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace STARTD.Game.Tower
{
    public abstract class TowerBehaviour : MonoBehaviour
    {
        public EnemyBehaviour targets;
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public List<Point> Range { get; private set; }
        [field: SerializeField] public float AttackDelay { get; private set; }

        private float attackElapsetTime = 0.0f;

        public void SetTarget(EnemyBehaviour from)
        {
            targets = from;
        }

        public void ClearTarget(EnemyBehaviour from)
        {
            targets = null;
        }

        public void Attack()
        {
            if (targets != null)
            {
                if (attackElapsetTime > AttackDelay)
                {
                    if (targets.LifeDown(Damage))
                    {
                    }
                    attackElapsetTime = 0;
                }

                attackElapsetTime += Time.deltaTime;
            }
            else
            {
                attackElapsetTime = 0;
            }
        }
    }
}