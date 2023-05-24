using UnityEngine;

namespace ConditionSystem
{
    public class UIConditionLinker : UIConditionEngine
    {
        [SerializeField] private ConditionEngine _linkedEngine;
        protected override bool InternalCheck()
        {
            return !_linkedEngine || _linkedEngine.Check();
        }
    }
}