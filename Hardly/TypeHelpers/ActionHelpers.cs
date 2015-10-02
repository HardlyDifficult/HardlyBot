using System;

namespace Hardly {
    /// <summary>
    /// This creates an action that includes dynamic data.  I would love feedback on designs... seems fugly
    /// </summary>
   public class ActionWithStaticData<DataType, T1, T2> {
        DataType staticData;
        Action<DataType, T1, T2> actionMethod;

        public ActionWithStaticData(DataType staticData, Action<DataType, T1, T2> actionMethod) {
            this.staticData = staticData;
            this.actionMethod = actionMethod;
        }

        public static Action<T1, T2> For(Action<DataType, T1, T2> actionMethod, DataType staticData) {
            ActionWithStaticData<DataType, T1, T2> action = new ActionWithStaticData<DataType, T1, T2>(staticData, actionMethod);
            return action.DoAction;
        }

        void DoAction(T1 arg1, T2 arg2) {
            actionMethod(staticData, arg1, arg2);
        }
    }
}
