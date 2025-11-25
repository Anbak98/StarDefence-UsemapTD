using System;
using UnityEngine;

namespace STARTD.Game.Enemy
{
    public class BaseEnemyState
    {
        public Transform transform;
        public EnemyBehaviour behaviour;
        public Action<Type> onDone;

        public virtual void Enter(Action<Type> callback)
        {
            onDone = callback;
        }

        public virtual void Execute()
        {
        }

        public virtual void Exit()
        {

        }
    }
}
