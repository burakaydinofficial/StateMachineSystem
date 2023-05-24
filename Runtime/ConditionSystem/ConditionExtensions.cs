using System.Collections.Generic;

namespace ConditionSystem
{
    public static class ConditionExtensions
    {
        public static bool TrueForAllOrEmpty(this List<ICondition> conditions)
        {
            return conditions.Count == 0 || conditions.TrueForAll(x => x == null || x.Check());
        }
        public static bool TrueForAllOrEmpty(this List<ConditionEngine> conditions)
        {
            return conditions.Count == 0 || conditions.TrueForAll(x => x == null || x.Check());
        }
        public static bool TrueForAllOrEmpty(this List<UIConditionEngine> conditions)
        {
            return conditions.Count == 0 || conditions.TrueForAll(x => x == null || x.Check());
        }
    }
}