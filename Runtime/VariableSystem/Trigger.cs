using System;
using UnityEngine;

namespace VariableSystem
{
    [Serializable]
    public class Trigger
    {
        [SerializeField] private bool _value;
        public void SetTrigger()
        {
            _value = true;
        }
        public void ResetTrigger()
        {
            _value = false;
        }
        public bool GetTrigger()
        {
            bool previousValue = _value;
            ResetTrigger();
            return previousValue;
        }
    }
}