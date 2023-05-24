using UnityEngine;

namespace ConditionSystem
{
    public abstract class UIConditionEngine : MonoBehaviour, ICondition
    {
        [SerializeField] private bool _IgnoreInEditor = false;

        public bool Check()
        {
            return InternalCheck() || _IgnoreInEditor && Application.isEditor;
        }

        protected abstract bool InternalCheck();
    }
}