using System.Collections.Generic;
using UnityEngine;

namespace StateMachineSystem.Base
{
    [CreateAssetMenu(fileName = "WorkingGroup", menuName = "State Machine System/Working Group")]
    public class WorkingGroup : StateMachineSystemObject, ISerializationCallbackReceiver
    {
        [SerializeField] private List<StateMachineSystemObject> _dependency = new List<StateMachineSystemObject>();

        internal void Register(StateMachineSystemObject obj)
        {
            if (!_dependency.Contains(obj) && obj != this)
                _dependency.Add(obj);
        }

        public void OnBeforeSerialize()
        {
            if (_dependency.Contains(this))
                _dependency.Remove(this);

            foreach (var o in _dependency)
            {
                if (o)
                    o.Register(this);
            }
        }

        public void OnAfterDeserialize()
        {
            if (_dependency.Contains(this))
                _dependency.Remove(this);

            foreach (var o in _dependency)
            {
                if (o)
                    o.Register(this);
            }
        }
    }
}