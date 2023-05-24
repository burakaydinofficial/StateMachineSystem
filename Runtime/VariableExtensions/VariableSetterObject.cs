using UnityEngine;
using StateMachineSystem;
using StateMachineSystem.Base;

namespace VariableExtensions
{
    public abstract class VariableSetterObject<T> : StateMachineSystemObject
    {
        [SerializeField] private State _sourceState;
        [SerializeField] private int _variableId;

        public void SetVariable(T newValue)
        {
            _sourceState.SetVariable(_variableId, newValue);
        }
    }
}