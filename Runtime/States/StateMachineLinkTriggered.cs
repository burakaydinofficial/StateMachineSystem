using System;
using UnityEngine;
using VariableSystem;

namespace StateMachineSystem
{
    [CreateAssetMenu(fileName = "StateMachineLinkTriggered",
        menuName = "State Machine System/State System/State Machines Links/State Machine Link Triggered")]
    public class StateMachineLinkTriggered : StateMachineLink
    {
        [SerializeField] private int triggerVariableID = 0;
        [NonSerialized] private Variable<Trigger> trigger;

        protected override void TriggerLink(State state)
        {
            if (!trigger)
            {
                trigger = state.GetVariable<Trigger>(triggerVariableID);
                if (!trigger)
                {
                    Debug.LogError("StateMachineLinkTriggered cannot find the trigger", this);
                    return;
                }
            }
            if (trigger.GetValue().GetTrigger())
                TargetState?.Enter();
        }
    }
}