using UnityEngine;

namespace StateMachineSystem
{
    [CreateAssetMenu(fileName = "StateMachineLink", menuName = "State Machine System/State System/State Machines Links/State Machine Link With Undo")]
    public class StateMachineLinkWithUndo : StateMachineLink
    {
        public void Undo()
        {
            if (!TargetState.StateActive) return;
            TargetState?.Exit();
            SourceState?.Enter();
        }
    }
}