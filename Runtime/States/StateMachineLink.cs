using System;
using UnityEngine;

namespace StateMachineSystem
{
    [CreateAssetMenu(fileName = "StateMachineLink", menuName = "State Machine System/State System/State Machines Links/State Machine Link")]
    public class StateMachineLink : StateMachineLinkBase
    {
        [SerializeField] protected State TargetState;

        protected override void TriggerLink(State state)
        {
            TargetState?.Enter();
        }
    }
}