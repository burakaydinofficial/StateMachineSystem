using System;
using System.Collections.Generic;
using UnityEngine;

namespace VariableSystem
{
    public class VariableHolder  {


        private readonly List<Variable> _variables;

        public Variable<T> GetVariable<T>(int id)
        {
            //Debug.Log("Searching for variable ID: " + id);
            //foreach (var variable in _variables)
            //{
            //    Debug.LogFormat("Variable: {0} ID Match: {1} TypeMatch: {2}",variable, variable.ID == id, variable.GetValueType() == typeof(T));
            //}
            return _variables.Find(x => Variable.Check<T>(x, id)) as Variable<T>;
        }
        public List<Variable<T>> GetVariables<T>(int id)
        {
            return _variables.FindAll(x => Variable.Check<T>(x, id)).ConvertAll(x => x as Variable<T>);
        }

        public List<Variable> GetAll()
        {
            return new List<Variable>(_variables);
        }
        public List<Variable<T>> GetVariables<T>()
        {
            return _variables.FindAll(Variable.Check<T>).ConvertAll(x => x as Variable<T>);
        }

        public VariableHolder(VariableHolderSetup setup)
        {
            _variables = setup?.GetVariables() ?? new List<Variable>();
        }

        public void AddVariables(List<Variable> variables)
        {

            if (_variables == null)
                Debug.LogError("_variables in Add Variables are null ");

            _variables.AddRange(variables);
        }
        public bool SetVariable<T>(int id, T value)
        {
            Variable<T> variable = GetVariable<T>(id);
            if (variable) variable.SetValue(value);
            else Debug.LogWarningFormat("in SetVariable, Variable not found. ID: {0} Type: {2} Value: {1}", id, value, value.GetType());
            return variable;
            //else Variables.Add(new Variable<T>(id, value)); // TODO think about adding it or not. It will add a feature to system to add variables to a state in any type at anytime but it has a risk for setting a wrong default value
            // TODO Maybe we can add runtime variables, which will be removed on reset
        }

        public void LinkParameterUpdate<T>(int id, Action<T,T> action)
        {
            var variable = GetVariable<T>(id);
            if (variable)
            {
                variable.OnUpdate += action.Invoke;
            }
        }
        public void Reset()
        {
            _variables.ForEach(Reset);
        }
        private static void Reset(Variable variable) => variable.Reset(false);




    }
}