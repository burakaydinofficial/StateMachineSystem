using ConditionSystem;
using UnityEngine;

namespace StateMachineSystem
{

    [CreateAssetMenu(fileName = "ConditionalStateMachineLink", menuName = "State Machine System/State System/State Machines Links/Conditional State Machine Link")]
    public class ConditionalStateMachineLink : StateMachineLink
    {
        [SerializeField] private State _conditionTrueState;
        [SerializeField] private ConditionEngine _conditionEngine;

        protected override void TriggerLink(State state)
        {
            if (_conditionEngine && _conditionEngine.Check()) _conditionTrueState?.Enter();
            else TargetState?.Enter();
        }
    }
}