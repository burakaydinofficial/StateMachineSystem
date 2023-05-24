using StateMachineSystem.Base;
using UnityEngine;

namespace StateMachineSystem
{
    [CreateAssetMenu(fileName = "StartPoint", menuName = "State Machine System/State System/Start Point")]
    public class StartPoint : StateMachineSystemObject
    {
        [SerializeField] private State _startState;
        protected override void Start()
        {
            base.Start();
            _startState?.Enter();
        }
    }
}