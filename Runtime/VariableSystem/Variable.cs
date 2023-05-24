using System;
using UnityEngine;

namespace VariableSystem
{
    public enum VariableDefinitionType
    {
        Runtime = 0,
        Predefined = 1,
        RemoteVariable = 2
    }

    public enum VariableSyncMode
    {
        SyncOff = 0,
        MasterControlled = 1,
        Unmanaged = 2,
        Assignable = 3
    }

    public abstract class Variable
    {
        public abstract int ID { get; }

        public abstract void Reset(bool forceReset);
        public abstract Type GetValueType();
        public abstract bool IsTypeOf(Type type);
        public abstract VariableDefinitionType DefinitionType { get; }
        public abstract VariableSyncMode SyncMode { get; }
        public abstract string Serialize2String();
        public abstract byte[] Serialize2Binary();
        public abstract bool DeserializeFromJsonAndOverride(string json, bool async);

        public abstract bool DeserializeFromBinaryAndOverride(byte[] binary, bool async);

        public abstract event Action<Variable> Updated;

        public static implicit operator bool(Variable variable)
        {
            return variable != null;
        }

        public static bool Check<T>(Variable variable, int id)
        {
            return Check<T>(variable) && id == variable.ID;
        }

        public static bool Check<T>(Variable variable)
        {
            return variable && variable.IsTypeOf(typeof(T));
        }

        public static bool Check(Variable variable, int id, Type t)
        {
            return variable && variable.IsTypeOf(t) && id == variable.ID;
        }

        public abstract object GetRawValue();

        public abstract bool SetRawValue(object obj);

        public abstract Variable GetRemoteVariable(int localizedID);
    }

    public class Variable<T> : Variable
    {
        private T _variable;
        private readonly T _defaultValue;
        protected int _id;
        public override int ID => _id;
        private readonly VariableDefinitionType _definitionType;
        private readonly VariableSyncMode _syncMode;

        public override VariableDefinitionType DefinitionType => _definitionType;
        public override VariableSyncMode SyncMode => _syncMode;

        private event Action<Variable> _updated;
        public override event Action<Variable> Updated
        {
            add
            {
                _updated += value;
            }
            remove
            {
                _updated -= value;
            }
        }

        protected Variable() { } // For remote variable
        private Variable(int id, T defaultValue, VariableDefinitionType definitionType, VariableSyncMode syncMode)
        {
            _id = id;
            _variable = _defaultValue = defaultValue;
            _definitionType = definitionType;
            _syncMode = syncMode;
        }

        // PredefinedVariable
        public Variable(int id, T defaultValue, VariableSyncMode syncMode) : this(id, defaultValue, VariableDefinitionType.Predefined, syncMode)
        {

        }

        // RuntimeVariable
        public Variable(int id, T defaultValue) : this(id, defaultValue, VariableDefinitionType.Runtime, VariableSyncMode.SyncOff)
        {
        }

        public virtual void SetValue(T value, bool async = true)
        {
            //Debug.Log("Set Value - Before " + ToString());
            if (!_variable.Equals(value))
            {
                TriggerUpdate(value, async);
            }
            _variable = value;
            //Debug.Log("Set Value - After " + ToString());
        }

        /// <summary>
        /// Only use if variable changed internally or via SetValue
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="async">with synchronization or not</param>
        protected void TriggerUpdate(T value, bool async)
        {
            try
            {
                if (async)
                {
                    EventSynchronizer.Enqueue(() => OnUpdate?.Invoke(_variable, value));
                    EventSynchronizer.Enqueue(() => _updated?.Invoke(this));
                }
                else
                {
                    OnUpdate?.Invoke(_variable, value);
                    _updated?.Invoke(this);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("TriggerUpdate Error");
                Debug.LogException(e);
            }
        }

        public virtual T GetValue()
        {
            return _variable;
        }
        /// <summary>
        /// OnUpdate(Previous Value, New Value)
        /// </summary>
        public virtual event Action<T, T> OnUpdate;
        public override void Reset(bool forceReset)
        {
            //Debug.Log("Variable Reset " + GetType() + " ID: " + ID + " id: " + _id + " value: " + _variable + " default value: " + _defaultValue);
            _variable = _defaultValue;
        }
        public override Type GetValueType()
        {
            return typeof(T);
        }
        public override bool IsTypeOf(Type type)
        {
            return type == typeof(T);
        }

        public override Variable GetRemoteVariable(int localizedID)
        {
            return RemoteVariable<T>.CreateVariable(this, localizedID);
        }

        public override object GetRawValue()
        {
            return _variable;
        }

        public override bool SetRawValue(object obj)
        {
            try
            {
                if (obj.GetType().IsInstanceOfType(typeof(T)))
                {
                    SetValue((T) obj);
                    return true;
                }
                else
                {
                    Debug.LogError("SetRawValue Error, " + obj + " type of " + obj?.GetType()?.FullName + " cannot be assigned to the type " + typeof(T).FullName);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("SetRawValue Error");
                Debug.LogException(e);
            }

            return false;
        }

        public override byte[] Serialize2Binary()
        {
            return System.Text.Encoding.ASCII.GetBytes(Serialize2String());
        }
        protected virtual bool Deserialize(byte[] binary, out T result)
        {
            return Deserialize(System.Text.Encoding.ASCII.GetString(binary), out result);
        }

        private static bool Equals(byte[] binary1, byte[] binary2)
        {
            if (binary1 == binary2)
                return true;
            if (binary1 != null && binary2 != null && binary1.Length == binary2.Length)
            {
                for (var i = 0; i < binary1.Length; i++)
                {
                    if (binary1[i] != binary2[i])
                        return false;
                }

                return true;
            }
            return false;
        }

        public override bool DeserializeFromBinaryAndOverride(byte[] binary, bool async)
        {
            try
            {
                if (!Equals(Serialize2Binary(), binary))
                {
                    T value;
                    if (Deserialize(binary, out value))
                        SetValue(value, async);
                    //return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            //return false;
            return true;
        }

        public override string Serialize2String()
        {
            return JsonUtility.ToJson(GetValue());
        }

        protected virtual bool Deserialize(string serialized, out T result)
        {
            result = default;
            try
            {
                T value = JsonUtility.FromJson<T>(serialized);
                if (value != null)
                {
                    result = value;
                    return true;
                }
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public override bool DeserializeFromJsonAndOverride(string serialized, bool async)
        {
            try
            {
                if (!Serialize2String().Equals(serialized))
                {

                    T value;
                    if (Deserialize(serialized, out value))
                        SetValue(value, async);
                    //return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            //return false;
            return true;
        }

        public override string ToString()
        {
            return String.Format("ID: {0} Type: {1} Value: {2} Mode: {3} DefinitionType: {4} SyncType: {5}", ID, GetValueType(), GetRawValue(),
                GetType().Name, DefinitionType, SyncMode);
        }
    }
}