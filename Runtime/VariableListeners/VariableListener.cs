using StateMachineSystem.Base;
using UnityEngine;
using VariableSystem;

namespace StateMachineSystem.VariableListeners
{
    public abstract class VariableListener<TData> : StateMachineSystemObject
    {
        [SerializeField] private State _sourceState;
        [SerializeField] private int _variableId;

        protected sealed override void Start()
        {
            base.Start();
            if (_sourceState)
            {
                Variable<TData> variable = _sourceState.GetVariable<TData>(_variableId);
                if (variable)
                {
                    variable.OnUpdate += UpdateVariable;
                }
                else
                {
                    Debug.LogWarning("VariableListener unable to initialize because state doesn't have a variable with id.");
                }
            }
            else
            {
                Debug.LogWarning("VariableListener unable to initialize because doesn't have a state.");
            }
        }

        protected abstract void UpdateVariable(TData previous, TData newValue);
    }
}
