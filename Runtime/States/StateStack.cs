using System.Collections.Generic;

namespace StateMachineSystem
{
    public class StateStack
    {
        public static List<StateAction> Actions = new List<StateAction>(256);

        public class StateAction
        {
            public enum ActionType{ Create, Init, Enter, Exit, ForcedExit }

            public State ActionState;
            public ActionType Action;

            public StateAction(State state, ActionType action)
            {
                ActionState = state;
                Action = action;
            }
        }

        public static void LogCreateAction(State state) => Actions.Add(new StateAction(state, StateAction.ActionType.Create));
        public static void LogInitAction(State state) => Actions.Add(new StateAction(state, StateAction.ActionType.Init));
        public static void LogEnterAction(State state) => Actions.Add(new StateAction(state, StateAction.ActionType.Enter));
        public static void LogExitAction(State state) => Actions.Add(new StateAction(state, StateAction.ActionType.Exit));
        public static void LogForcedExitAction(State state) => Actions.Add(new StateAction(state, StateAction.ActionType.ForcedExit));
    }
}