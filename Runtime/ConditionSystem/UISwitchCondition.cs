using UnityEngine;

namespace ConditionSystem
{
    public class UISwitchCondition : UIConditionEngine
    {
        [SerializeField] private bool _turnOn = true;
        protected override bool InternalCheck()
        {
            return _turnOn;
        }
    }
}