using System;
using StateMachineSystem.Base;
using UnityEngine;

namespace StateMachineSystem
{
    public abstract class StateMachineLinkBase : StateMachineSystemObject
    {
        [SerializeField] protected State SourceState;
        [SerializeField] protected bool Disabled = false;


        protected override void Init()
        {
            base.Init();
            if (SourceState) SourceState.OnExit += InternalTriggerLink;
        }

        private void InternalTriggerLink(State state)
        {
            if (!Disabled) TriggerLink(state);
        }
        protected abstract void TriggerLink(State state);
    }
}
