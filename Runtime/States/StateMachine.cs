using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using VariableSystem;

namespace StateMachineSystem
{
    [CreateAssetMenu(fileName = "StateMachine", menuName = "State Machine System/State System/State Machine")]
    public class StateMachine : State
    {
        [SerializeField] private List<State> _states = new List<State>();
        public IReadOnlyList<State> States => _states;

        protected override void Create()
        {
            _states.RemoveAll(x => !x);
            base.Create();
        }

        protected override void Init()
        {
            try
            {
                if (HasChild(this))
                {
                    Debug.LogError("State is child of himself. A stack overflow prevented from happening");
                    return;
                }


                foreach (var state in States)
                {
                    try
                    {
                        state.LinkVariables(_VariableHolder.GetAll());
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(string.Format("{0} cannot link variables {1}", name, state.name), this);
                        Debug.LogException(e, this);
                    }

                }

                base.Init();

            }
            catch (Exception ex)
            {
                Debug.LogError(name + " Failed on Init ", this); // 2
            }
        }

        public override void LinkVariables(List<Variable> remoteVariables)
        {
            base.LinkVariables(remoteVariables);
            _states.ForEach(x => x.LinkVariables(_VariableHolder.GetAll()));
        }


        [NonSerialized] private bool loopLock2 = false;
        public override bool HasChild(State state)
        {
            foreach (var child in States)
                if (child)
                {
                    if (state == child)
                        return true;
                    else if (child.HasChild(state))
                        return true;
                    //(child is StateMachine && ((StateMachine)child).HasChild(state)) return true;
                }
            return false;
        }
    }
}