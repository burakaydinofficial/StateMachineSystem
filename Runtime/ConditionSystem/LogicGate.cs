using UnityEngine;
using System;

namespace ConditionSystem
{
    public abstract class LogicGate : ConditionEngine
    {
        [Header("An empty gate will return true")]
        [SerializeField] protected ConditionEngine[] SourceEngines;
        
    }
}