using STARTD.Game.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace STARTD.Game.Enemy
{
    public class EnemyFSM : MonoBehaviour
    {
        [SerializeField] private EnemyStateHandler handler;
        public EnemyBehaviour behaviour;

        private BaseEnemyState curState;

        public void Init(EnemyBehaviour behaviour)
        {
            handler.Init();    
            this.behaviour = behaviour;
            curState = handler.DefaultState;
        }

        // Start is called before the first frame update
        public void Enter()
        {
            curState?.Enter(type => ChangeState(type));
        }

        // Update is called once per frame
        // EnemyFSM
        public void Execute()
        {
            curState?.Execute();
        }

        // ChangeState 수정
        public void ChangeState(Type type)
        {
            curState?.Exit();
            curState = handler.GetEnemyState(type); // Type 기반 GetEnemyState 필요
            curState?.Enter(type => ChangeState(type));
        }
    }
}
