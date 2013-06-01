using KeepYourTime.DataBase.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.Utils
{
    public class StaticEvents
    {


        public delegate void TaskIDHanlder(long TaskID);
        public delegate void TaskHanlder(TaskAdapter TaskAdapt);

        public static event EventHandler OnTaskListChanged;

        public static void RaiseEventOnTaskListChanged()
        {
            if (OnTaskListChanged != null) OnTaskListChanged(null, new EventArgs());
        }


        public static event TaskIDHanlder OnTaskUpdated;

        public static void RaiseEventOnTaskUpdated(long TaskID)
        {
            if (OnTaskUpdated != null) OnTaskUpdated(TaskID);
        }


        public static event TaskHanlder OnTaskUpdatedTask;
        public static void RaiseEventOnTaskUpdatedTask(TaskAdapter Task)
        {
            if (OnTaskUpdatedTask != null) OnTaskUpdatedTask(Task);
        }

        public static event TaskIDHanlder OnTaskStarted;

        public static void RaiseEventOnTaskStarted(long TaskID)
        {
            if (OnTaskStarted != null) OnTaskStarted(TaskID);
        }


        public static event TaskIDHanlder OnTimeAdded;
        public static void RaiseEventOnTimeAdded(long TaskID)
        {
            if (OnTimeAdded != null) OnTimeAdded(TaskID);
        }



    }
}
