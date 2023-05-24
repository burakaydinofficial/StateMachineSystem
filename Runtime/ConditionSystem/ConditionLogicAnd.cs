using UnityEngine;

namespace ConditionSystem
{
    [CreateAssetMenu(fileName = "LogicAnd", menuName = "State Machine System/Condition Engines/Logic And")]
    public class ConditionLogicAnd : LogicGate
    {
        protected override bool InternalCheck()
        {
            foreach (var engine in SourceEngines)
            {
                if (engine != null && !engine.Check()) return false;
            }
            return true;
        }
    }
}