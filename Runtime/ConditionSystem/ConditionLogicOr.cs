using UnityEngine;

namespace ConditionSystem
{
    [CreateAssetMenu(fileName = "LogicOr", menuName = "State Machine System/Condition Engines/Logic Or")]
    public class ConditionLogicOr : LogicGate
    {
        protected override bool InternalCheck()
        {
            bool engineFound = false;
            foreach (var engine in SourceEngines)
            {
                if (engine != null)
                {
                    if (engine.Check()) return true;
                    engineFound = true;
                }
            }
            return !engineFound;
        }
    }
}