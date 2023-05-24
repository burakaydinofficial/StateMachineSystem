using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using VariableSystem;


namespace StateMachineSystem
{
    [CreateAssetMenu(fileName = "State", menuName = "State Machine System/State System/State")]
    public class State : VariableSourceObject
    {
        [SerializeField] public string Name;
        //public event Action<State> OnCreate = StateStack.LogCreateAction; 
        //public event Action<State> OnInit = StateStack.LogInitAction;
        public event Action<State> OnEnter = StateStack.LogEnterAction;
        public event Action<State> OnExit = StateStack.LogExitAction;

        public bool StateActive { get; private set; } = false;
        public bool IsEntering { get; private set; } = false;
        public bool IsExiting { get; private set; } = false;

        [SerializeField] public VariableHolderSetup VariableSetup;

        private VariableHolder _variableHolder;
        protected VariableHolder _VariableHolder => _variableHolder = _variableHolder ?? CreateVariableHolder();
        [SerializeField] protected bool PreventUnmappedVariablePass = false;


        private VariableHolder CreateVariableHolder()
        {

            //OnCreate?.Invoke(this);
            try
            {
                return new VariableHolder(VariableSetup);
                
            }
            catch (Exception e)
            {
                Debug.LogError("Cannot create _VariableHolder", this);
                Debug.LogException(e, this);
                return null;
            }
        }

        [ContextMenu("Init")]
        protected override void Init()
        {

            StateActive = false;
            OnEnter += (x) => _VariableHolder?.Reset();
            //OnInit?.Invoke(this);
        }

        public void Enter()
        {
            IsEntering = true;
            if (!StateActive)
            {
                StateActive = true;
                try
                {
                    OnEnter?.Invoke(this);
                }
                catch (Exception e)
                {
                    Debug.LogError( "Entering OnEnterCallbacks Failed", this);
                    Debug.LogException(e, this);
                }
            }
            else Debug.LogWarning("Enter Rejected", this);
            IsEntering = false;
        }

        public void Exit()
        {
            //Debug.Log("Exiting", this);
            IsExiting = true;
            if (StateActive)
            {
                StateActive = false;
                OnExit?.Invoke(this);
            }
            else Debug.LogWarning("Exit Rejected", this);
            IsExiting = false;
        }

        public override bool SetVariable<T>(int id, T value)
        {
            return _VariableHolder.SetVariable(id, value);
        }

        public void LinkParameterUpdate<T>(int id, Action<T,T> action)
        {
            _VariableHolder.LinkParameterUpdate(id, action);
        }

        public override List<Variable<T>> GetVariables<T>(int id)
        {
            return _VariableHolder.GetVariables<T>(id);
        }
        public List<Variable<T>> GetVariables<T>()
        {
            return _VariableHolder.GetVariables<T>();
        }

        public override Variable<T> GetVariable<T>(int id)
        {
            return _VariableHolder.GetVariable<T>(id);
        }

        public override List<Variable> GetAllVariables()
        {
            return _VariableHolder.GetAll();
        }

        public virtual void LinkVariables(List<Variable> remoteVariables)
        {

            if (_VariableHolder == null)
                Debug.LogError("_VariableHolder is null", this);

            var all = _VariableHolder.GetAll();

            if (all == null)
                Debug.LogError("_VariableHolder.GetAll() is null", this);

            var remMppr = VariableSetup.RemoteMapper;

            var mapped = remMppr.MapVariables(remoteVariables, all, PreventUnmappedVariablePass);

            if (mapped == null)
                Debug.LogError("mapped is null", this);

            _VariableHolder.AddVariables(mapped);

        }

        public virtual bool HasChild(State state) => false;

        [ContextMenu("Log Variables")]
        private void LogVariables()
        {
            var variables = GetAllVariables();
            variables.ForEach(Debug.Log);
        }


#if UNITY_EDITOR
        [ContextMenu("Copy Variable Setups")]
        protected void CopyVariableSetups()
        {
            GUIUtility.systemCopyBuffer = JsonUtility.ToJson(VariableSetup, true);
        }

        [ContextMenu("Paste Variable Setups")]
        protected void PasteVariableSetups()
        {
            UnityEditor.Undo.RecordObject(this, "Variable Setups Pasted as Override");
            JsonUtility.FromJsonOverwrite(GUIUtility.systemCopyBuffer, VariableSetup);
        }

        [ContextMenu("Append Variable Setups")]
        protected void AppendVariableSetups()
        {
            UnityEditor.Undo.RecordObject(this, "Variable Setups Pasted as Override");
            VariableHolderSetup setup = new VariableHolderSetup();
            JsonUtility.FromJsonOverwrite(GUIUtility.systemCopyBuffer, setup);
            VariableHolderSetup.VariableHolderSetupEditorUtilities.AppendVariableSetup(setup, VariableSetup);
        }

        [ContextMenu("Paste Variable Setups", true)]
        protected bool PasteVariableSetupsValidate()
        {
            try
            {
                var deserialized = JsonUtility.FromJson<VariableHolderSetup>(GUIUtility.systemCopyBuffer);
                return deserialized != null;
            }
            catch (Exception e)
            {
                return false;
            }
        }
#endif
    }
}
