using ConditionSystem;
using UnityEngine;
using VariableSystem;

namespace StateMachineSystem.Conditions
{
    [CreateAssetMenu(fileName = "VariableRelationalCondition", menuName = "State Machine System/Condition Engines/Variable Relational Condition")]
    public class VariableRelationalCondition : ConditionEngine
    {
        public enum Relation { Equals, GreaterThanParameter, LessThanParameter, NotEquals }
        [Header("If state or variable is null, gate will return false")]
        [SerializeField] private State _state;
        [SerializeField] private int _variableId;
        [SerializeField] private Relation _relationMode;
        [SerializeField] private int _parameter;

        protected override bool InternalCheck()
        {
            if (!_state) return false;
            Variable<int> variable = _state.GetVariable<int>(_variableId);
            if (!variable) return false;

            var v = variable.GetValue();
            switch (_relationMode)
            {
                case Relation.Equals:
                    return v == _parameter;
                case Relation.GreaterThanParameter:
                    return v > _parameter;
                case Relation.LessThanParameter:
                    return v < _parameter;
                case Relation.NotEquals:
                    return v != _parameter;
            }
            return false;
        }

    }
}