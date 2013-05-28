using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.Utils
{
    public class StaticEvents
    {
        public static event EventHandler OnTaskListChanged;
        public static void RaiseEventOnTaskListChanged()
        {
            if (OnTaskListChanged != null) OnTaskListChanged(null, new EventArgs());
        }

        public delegate void TaskIDHanlder(long TaskID);

        public static event TaskIDHanlder OnTaskUpdated;

        public static void RaiseEventOnTaskUpdated(long TaskID)
        {
            if (OnTaskUpdated != null) OnTaskUpdated(TaskID);
        }

        public static event TaskIDHanlder OnTaskStarted;

        public static void RaiseEventOnTaskStarted(long TaskID)
        {
            if (OnTaskStarted != null) OnTaskStarted(TaskID);
        }

    }
}
