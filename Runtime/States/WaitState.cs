using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using ConditionSystem;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachineSystem
{
    [CreateAssetMenu(fileName = "ConditionalWaitState", menuName = "State Machine System/State System/Conditional Wait State")]
    public class ConditionalWaitState : WaitState
    {
        protected override double Timeout => GetTimeout();
        [Header("If there is no match, default value will be used")]
        [SerializeField] private List<ConditionalFloat> _conditionalTimeouts = new List<ConditionalFloat>();
        
        private ConditionalFloat GetConditionalTimeout(out double value)  {
            foreach (var timeout in _conditionalTimeouts)  {
                if (timeout.Check(out value))
                    return timeout;
            }

            value = base.Timeout;
            return null;
        }

        private double GetTimeout() {
            foreach (var timeout in _conditionalTimeouts) {
                if (timeout.Check(out double value))
                    return value;
            }
            return base.Timeout;
        }

        [Serializable]
        public class ConditionalFloat
        {
            [SerializeField] public ConditionEngine Condition;
            [SerializeField] public double Value;
            
            public bool Check(out double value)
            {
                if (!Condition || Condition.Check())
                {
                    value = Value;
                    return true;
                }
                value = 0;
                return false;
            }
            

        }
    }
    [CreateAssetMenu(fileName = "WaitState", menuName = "State Machine System/State System/Wait State")]
    public class WaitState : State
    {
        [NonSerialized] private Timer timer;
        [Header("Time in milliseconds")]
        [SerializeField] protected double time;

        protected virtual double Timeout => time;

        protected override void Init() {
            OnEnter += state => StartTimer(); 

            startTime = 0;

            timer = new Timer();
            timer.Interval = Timeout;
            timer.Elapsed += TimerOnElapsed;
            timer.AutoReset = false;

            base.Init();
        }

        void StartTimer() {
            timer.Interval = Timeout;
            timer.Start();
            startTime = Time.time;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs) => EventSynchronizer.Enqueue(Exit);

        #region Inspector

        [NonSerialized] protected float startTime = 0;

        #endregion
    }
}