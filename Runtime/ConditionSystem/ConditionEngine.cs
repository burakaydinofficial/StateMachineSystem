using System.Collections.Generic;
using StateMachineSystem.Base;
using UnityEngine;

namespace ConditionSystem {
    
    public abstract class ConditionEngine : StateMachineSystemObject, ICondition
    {
        [SerializeField] private bool _IgnoreInEditor = false;

        public bool Check() {
            return InternalCheck() || ( _IgnoreInEditor && Application.isEditor );
        }

        [ContextMenu("Log Condition")]
        public void LogCondition() => Debug.Log("Condition is " + Check(), this);

        protected abstract bool InternalCheck();
        

    }
}
