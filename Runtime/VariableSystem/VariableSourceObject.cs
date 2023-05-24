using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StateMachineSystem.Base;

namespace VariableSystem
{
    public abstract class VariableSourceObject : StateMachineSystemObject, IVariableSource
    {
        public abstract Variable<T> GetVariable<T>(int id);
        public abstract List<Variable<T>> GetVariables<T>(int id);
        public abstract bool SetVariable<T>(int id, T value);
        public abstract List<Variable> GetAllVariables();
    }
}
