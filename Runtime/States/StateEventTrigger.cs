using System;
using StateMachineSystem.Base;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachineSystem
{
    [CreateAssetMenu(fileName = "StateEventTrigger", menuName = "State Machine System/State System/State Event Trigger")]
    public class StateEventTrigger : StateMachineSystemObject
    {
        [SerializeField] private State sourceState;

        //public UnityEvent InitEvent;
        public UnityEvent EnterEvent;
        public UnityEvent EndEvent;

        protected override void Create()
        {
            if (sourceState)
            {
                //sourceState.OnInit += state => InitEvent.Invoke();
                sourceState.OnEnter += state => EnterEvent.Invoke();
                sourceState.OnExit += state => EndEvent.Invoke();
            }
        }
    }
}