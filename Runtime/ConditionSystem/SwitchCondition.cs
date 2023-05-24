using UnityEngine;

namespace ConditionSystem
{
    [CreateAssetMenu(fileName = "SwitchCondition", menuName = "State Machine System/Condition Engines/Switch Condition")]
    public class SwitchCondition : ConditionEngine
    {
        [SerializeField] private bool _turnOn = true;
        protected override bool InternalCheck()
        {
            return _turnOn;
        }
    }
}
