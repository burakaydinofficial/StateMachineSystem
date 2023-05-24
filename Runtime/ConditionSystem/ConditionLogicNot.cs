using UnityEngine;

namespace ConditionSystem
{
    [CreateAssetMenu(fileName = "LogicNot", menuName = "State Machine System/Condition Engines/Logic Not")]
    public class ConditionLogicNot : ConditionEngine
    {
        [Header("Empty gate will return true")]
        [SerializeField] private ConditionEngine SourceEngine;

        protected override bool InternalCheck()
        {
            return SourceEngine == null || !SourceEngine.Check();
        }
    }
}