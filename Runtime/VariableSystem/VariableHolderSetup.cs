using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Globalization;

namespace VariableSystem
{

    [Serializable]
    public class VariableHolderSetup
    {
        [SerializeField] public RemoteVariableMapper RemoteMapper;
        [SerializeField] private List<IntegerVariableSetup> IntegerVariableSetups;
        [SerializeField] private List<FloatVariableSetup> FloatVariableSetups;
        [SerializeField] private List<Vector3VariableSetup> Vector3VariableSetups;
        [SerializeField] private List<QuaternionVariableSetup> QuaternionVariableSetups;
        [SerializeField] private List<StringVariableSetup> StringVariableSetups;
        [SerializeField] private List<BooleanVariableSetup> BooleanVariableSetups;
        [SerializeField] private List<TriggerSetup> TriggerSetups;
        

        #region VariableClasses
        [Serializable]
        public class IntegerVariableSetup : VariableSetup

        {
            public int DefaultValue;
            public override Variable GetVariable()
            {
                return new IntVariable(ID, DefaultValue, SyncMode);
            }
        }

        [Serializable]
        public class FloatVariableSetup : VariableSetup
        {
            public float DefaultValue;
            public override Variable GetVariable()
            {
                return new FloatVariable(ID, DefaultValue, SyncMode);
            }
        }

        [Serializable]
        public class Vector3VariableSetup : VariableSetup
        {
            public Vector3 DefaultValue;
            public override Variable GetVariable()
            {
                return new Variable<Vector3>(ID, DefaultValue, SyncMode);
            }
        }

        [Serializable]
        public class QuaternionVariableSetup : VariableSetup
        {
            public Quaternion DefaultValue = Quaternion.identity;
            public override Variable GetVariable()
            {
                return new Variable<Quaternion>(ID, DefaultValue, SyncMode);
            }
        }

        [Serializable]
        public class StringVariableSetup : VariableSetup
        {
            public string DefaultValue;
            public override Variable GetVariable()
            {
                return new StringVariable(ID, DefaultValue, SyncMode);
            }
        }

        [Serializable]
        public class BooleanVariableSetup : VariableSetup
        {
            public bool DefaultValue;
            public override Variable GetVariable()
            {
                return new BoolVariable(ID, DefaultValue, SyncMode);
            }
        }

        [Serializable]
        public class TriggerSetup : VariableSetup
        {
            public override Variable GetVariable()
            {
                return new Variable<Trigger>(ID, new Trigger(), SyncMode);
            }
        }

        public abstract class VariableSetup
        {
            [SerializeField] private string name;
            [SerializeField] public int ID = 0;
            [SerializeField] private VariableSyncMode _syncMode = VariableSyncMode.SyncOff;
            protected VariableSyncMode SyncMode => _syncMode;
            

            public static implicit operator bool(VariableSetup variableSetup)
            {
                return variableSetup != null;
            }

            public abstract Variable GetVariable();

#if UNITY_EDITOR
            public static class VariableSetupEditorUtilities
            {
                public static void SetName(VariableSetup setup, string name)
                {
                    setup.name = name;
                }
                public static string GetName(VariableSetup setup)
                {
                    return setup.name;
                }

                public static void SetSyncMode(VariableSetup setup, VariableSyncMode syncMode)
                {
                    setup._syncMode = syncMode;
                }

                public static VariableSyncMode GetSyncMode(VariableSetup setup)
                {
                    return setup._syncMode;
                }
            }
#endif
        }

        public List<Variable> GetVariables()
        {
            List<Variable> variableList = new List<Variable>();
            try
            {
                foreach (var setup in IntegerVariableSetups)
                {
                    variableList.Add(setup.GetVariable());
                }
                foreach (var setup in FloatVariableSetups)
                {
                    variableList.Add(setup.GetVariable());
                }
                foreach (var setup in Vector3VariableSetups)
                {
                    variableList.Add(setup.GetVariable());
                }
                foreach (var setup in QuaternionVariableSetups)
                {
                    variableList.Add(setup.GetVariable());
                }
                foreach (var setup in StringVariableSetups)
                {
                    variableList.Add(setup.GetVariable());
                }
                foreach (var setup in BooleanVariableSetups)
                {
                    variableList.Add(setup.GetVariable());
                }
                foreach (var setup in TriggerSetups)
                {
                    variableList.Add(setup.GetVariable());
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Cannot read VariableSetups in VariableHolderSetup");
                throw;
            }
            return variableList;
        }

        #endregion


        #region EditorUtilities
#if UNITY_EDITOR
        public static class VariableHolderSetupEditorUtilities
        {
            public static List<Vector3VariableSetup> GetVector3VariableSetups(VariableHolderSetup setup)
            {
                return setup.Vector3VariableSetups;
            }
            public static List<QuaternionVariableSetup> GetQuaternionVariableSetups(VariableHolderSetup setup)
            {
                return setup.QuaternionVariableSetups;
            }

            public static void AppendVariableSetup(VariableHolderSetup source, VariableHolderSetup target)
            {
                foreach (var setup in source.BooleanVariableSetups)
                {
                    if (!target.BooleanVariableSetups.Exists(x => x.ID == setup.ID))
                        target.BooleanVariableSetups.Add(setup);
                }
                foreach (var setup in source.FloatVariableSetups)
                {
                    if (!target.FloatVariableSetups.Exists(x => x.ID == setup.ID))
                        target.FloatVariableSetups.Add(setup);
                }
                foreach (var setup in source.IntegerVariableSetups)
                {
                    if (!target.IntegerVariableSetups.Exists(x => x.ID == setup.ID))
                        target.IntegerVariableSetups.Add(setup);
                }
                foreach (var setup in source.QuaternionVariableSetups)
                {
                    if (!target.QuaternionVariableSetups.Exists(x => x.ID == setup.ID))
                        target.QuaternionVariableSetups.Add(setup);
                }
                foreach (var setup in source.StringVariableSetups)
                {
                    if (!target.StringVariableSetups.Exists(x => x.ID == setup.ID))
                        target.StringVariableSetups.Add(setup);
                }
                foreach (var setup in source.TriggerSetups)
                {
                    if (!target.TriggerSetups.Exists(x => x.ID == setup.ID))
                        target.TriggerSetups.Add(setup);
                }
                foreach (var setup in source.Vector3VariableSetups)
                {
                    if (!target.Vector3VariableSetups.Exists(x => x.ID == setup.ID))
                        target.Vector3VariableSetups.Add(setup);
                }
            }
        }
#endif
        #endregion
    }

    public class FloatVariable : Variable<float>
    {
        protected override bool Deserialize(string serialized, out float result)
        {
            return float.TryParse(serialized, out result);
        }

        public override string Serialize2String()
        {
            return GetValue().ToString(CultureInfo.InvariantCulture);
        }

        public override byte[] Serialize2Binary()
        {
            return BitConverter.GetBytes(GetValue());
        }

        protected override bool Deserialize(byte[] binary, out float result)
        {
            int offset = 0;
            try
            {
                result = BitConverter.ToSingle(binary, offset);
            }
            catch (Exception e)
            {
                result = 0;
                return false;
            }
            return true;
        }

        public FloatVariable(int id, float defaultValue, VariableSyncMode syncMode)
            : base(id, defaultValue, syncMode)
        {

        }
    }

    public class IntVariable : Variable<int>
    {
        protected override bool Deserialize(string serialized, out int result)
        {
            return int.TryParse(serialized, out result);
        }

        public override string Serialize2String()
        {
            return GetValue().ToString();
        }

        public override byte[] Serialize2Binary()
        {
            return BitConverter.GetBytes(GetValue());
        }

        protected override bool Deserialize(byte[] binary, out int result)
        {
            int offset = 0;
            try
            {
                result = BitConverter.ToInt32(binary, offset);
            }
            catch (Exception e)
            {
                result = 0;
                return false;
            }
            return true;
        }

        public IntVariable(int id, int defaultValue, VariableSyncMode syncMode)
            : base(id, defaultValue, syncMode)
        {

        }
    }

    public class BoolVariable : Variable<bool>
    {
        protected override bool Deserialize(string serialized, out bool result)
        {
            return bool.TryParse(serialized, out result);
        }

        public override string Serialize2String()
        {
            return GetValue().ToString();
        }

        public override byte[] Serialize2Binary()
        {
            if (GetValue())
                return new byte[] { (byte)1 };
            else
                return new byte[] { (byte)0 };
        }

        protected override bool Deserialize(byte[] binary, out bool result)
        {
            try
            {
                switch (binary[0])
                {
                    case 0:
                        result = false;
                        return true;
                    case 1:
                        result = true;
                        return true;
                    default:
                        result = false;
                        return false;
                }
            }
            catch (Exception e)
            {
                result = false;
                return false;
            }
        }


        public BoolVariable(int id, bool defaultValue, VariableSyncMode syncMode)
            : base(id, defaultValue, syncMode)
        {

        }
    }

    public class StringVariable : Variable<string>
    {
        protected override bool Deserialize(string serialized, out string result)
        {
            result = serialized;
            return true;
        }

        public override string Serialize2String()
        {
            return GetValue();
        }

        public StringVariable(int id, string defaultValue, VariableSyncMode syncMode)
            : base(id, defaultValue, syncMode)
        {

        }
    }
}
