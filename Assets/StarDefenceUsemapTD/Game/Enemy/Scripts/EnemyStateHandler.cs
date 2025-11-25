using System;
using System.Collections.Generic;
using UnityEngine;

namespace STARTD.Game.Enemy
{
    public class EnemyStateHandler : MonoBehaviour
    {
        public EnemyBehaviour behaviour;
        public BaseEnemyState DefaultState {get; private set;}
        private List<BaseEnemyState> enemyStates = new();
        private readonly Dictionary<Type, BaseEnemyState> enemyStatesMap = new Dictionary<Type, BaseEnemyState>();

        public void Init()
        {
            DefaultState = new EnemyStateMove();
            enemyStates.Add(DefaultState);
            enemyStates.Add(new EnemyStateAttack());

            foreach (var enemyState in enemyStates)
            {
                enemyState.transform = transform;
                enemyState.behaviour = behaviour;
                if (!enemyStatesMap.ContainsKey(enemyState.GetType()))
                    enemyStatesMap.Add(enemyState.GetType(), enemyState);
            }
        }

        public BaseEnemyState GetEnemyState(Type type)
        {
            enemyStatesMap.TryGetValue(type, out var state);
            return state;
        }
    }
}
