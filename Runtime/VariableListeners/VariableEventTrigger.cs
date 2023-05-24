using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachineSystem.VariableListeners
{
    [CreateAssetMenu(fileName = "VariableEventTrigger", menuName = "State Machine System/State System/Variable Listeners/Variable Event Trigger")]
    public class VariableEventTrigger<TData, TEvent> : VariableListener<TData> where TEvent : UnityEvent<TData, TData>
    {
        [SerializeField] protected TEvent AlwaysDoEvent;
        [SerializeField] protected List<ParametricEvent> ParametricEvents;

        protected override void UpdateVariable(TData previous, TData newValue)
        {
            AlwaysDoEvent.Invoke(previous, newValue);
            ParametricEvent.Try2Trigger(ParametricEvents, previous, newValue);
        }

        [Serializable]
        public class ParametricEvent
        {
            [SerializeField] private List<TData> _values;
            [SerializeField] private TEvent _events;

            public static void Try2Trigger(List<ParametricEvent> _events, TData previousValue, TData newValue)
            {
                foreach (var parametricEvent in _events)
                {
                    if (parametricEvent._values.Contains(newValue))
                        parametricEvent._events.Invoke(previousValue, newValue);
                }
            }
        }

    }
}