using System.Collections.Generic;

namespace VariableSystem
{
    public interface IVariableSource
    {
        bool SetVariable<T>(int id, T value);
        List<Variable<T>> GetVariables<T>(int id);
        List<Variable> GetAllVariables();
        Variable<T> GetVariable<T>(int id);
    }
}
