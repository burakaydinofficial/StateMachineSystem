using System.IO;
using System.Reflection;
using UnityEngine;

namespace StateMachineSystem.Base
{
    public abstract class StateMachineSystemObject : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private WorkingGroup _workingGroup;

        protected virtual void Create() { }

        protected virtual void Init() { }

        protected virtual void Start() { }

        [ContextMenu("Close")]
        public virtual void Close() { }

        private void OnEnable()
        {
            Initializer.OnCreate += Create;
            Initializer.OnInit += Init;
            Initializer.OnStart += Start;
            Initializer.OnApplicationClose += Close;
        }

        public virtual void OnBeforeSerialize()
        {
            Register(_workingGroup);
        }

        public virtual void OnAfterDeserialize()
        {
            Register(_workingGroup);
        }

        internal void Register(WorkingGroup group)
        {
            if (group == this)
                group = null;
            _workingGroup = group;
            if (_workingGroup)
                _workingGroup.Register(this);
        }
    }
}