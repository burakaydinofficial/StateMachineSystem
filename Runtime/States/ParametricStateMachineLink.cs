using System;
using StateMachineSystem;
using UnityEngine;
using VariableSystem;

namespace StateMachineSystem
{
    [CreateAssetMenu(fileName = "ParametricStateMachineLink", menuName = "State Machine System/State System/State Machines Links/Parametric State Machine Link")]
    public class ParametricStateMachineLink : StateMachineLink
    {
        [SerializeField] private int variableID = 0;
        [SerializeField] private LinkedState[] linkedStates = new LinkedState[0];
        [NonSerialized] private Variable<int> variable;


        protected override void TriggerLink(State state)
        {
            if (!variable)
            {
                variable = state.GetVariable<int>(variableID);
                if (!variable)
                {
                    Debug.LogError("ParametricStateMachineLink cannot find the variable", this);
                    return;
                }
            }
            int parameter = variable.GetValue();
            int index = Array.FindIndex(linkedStates, x => x.Parameter == parameter);
            if (index >= 0)
                linkedStates[index].TargetState?.Enter();
            else TargetState?.Enter();
        }

        [Serializable]
        public class LinkedState
        {
            public int Parameter;
            public State TargetState;
        }
    }
}