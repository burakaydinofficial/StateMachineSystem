using System;

namespace VariableSystem
{
    public class RemoteVariable<T> : Variable<T>
    {
        public readonly Variable<T> ReferenceVariable;
        public override VariableDefinitionType DefinitionType => VariableDefinitionType.RemoteVariable;
        public override VariableSyncMode SyncMode => VariableSyncMode.SyncOff;

        private RemoteVariable(Variable<T> remoteVariable, int localID)
        {
            _id = localID;
            ReferenceVariable = remoteVariable;
        }
        private RemoteVariable(Variable remoteVariable, int localID)
        {
            ReferenceVariable = remoteVariable as Variable<T>;
        }
        public static RemoteVariable<T> CreateVariable(Variable<T> remoteVariable, int localID)
        {
            return new RemoteVariable<T>(remoteVariable, localID);
        }

        public override Type GetValueType()
        {
            return ReferenceVariable.GetValueType();
        }

        public override bool IsTypeOf(Type type)
        {
            return ReferenceVariable.IsTypeOf(type);
        }

        public override void Reset(bool forceReset)
        {
            if (forceReset)
                ReferenceVariable.Reset(forceReset);
        }

        public override T GetValue() => ReferenceVariable.GetValue();
        public override void SetValue(T value, bool async) => ReferenceVariable.SetValue(value, async);
        public override event Action<T,T> OnUpdate
        {
            add => ReferenceVariable.OnUpdate += value;
            remove => ReferenceVariable.OnUpdate -= value;
        }

        public override object GetRawValue()
        {
            return ReferenceVariable.GetRawValue();
        }

        public override bool SetRawValue(object obj)
        {
            return ReferenceVariable.SetRawValue(obj);
        }
    }
}