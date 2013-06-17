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

        #region delegates

        public delegate void TaskIDHanlder(long TaskID);
        public delegate void TaskHanlder(TaskAdapter TaskAdapt);

        #endregion

        #region OnTaskListChanged


        public static event EventHandler OnTaskListChanged;

        public static void RaiseEventOnTaskListChanged()
        {
            if (OnTaskListChanged != null) OnTaskListChanged(null, new EventArgs());
        }

        #endregion

        #region OnTaskUpdated

        public static event TaskIDHanlder OnTaskUpdated;

        public static void RaiseEventOnTaskUpdated(long TaskID)
        {
            if (OnTaskUpdated != null) OnTaskUpdated(TaskID);
        }
        
        #endregion

        #region OnTaskUpdatedTask

        public static event TaskHanlder OnTaskUpdatedTask;
        public static void RaiseEventOnTaskUpdatedTask(TaskAdapter Task)
        {
            if (OnTaskUpdatedTask != null) OnTaskUpdatedTask(Task);
        }

        #endregion

        #region OnTaskStarted
        
        public static event TaskIDHanlder OnTaskStarted;
        public static void RaiseEventOnTaskStarted(long TaskID)
        {
            if (OnTaskStarted != null) OnTaskStarted(TaskID);
        }

        #endregion

        #region OnTimeAdded
       
        public static event TaskIDHanlder OnTimeAdded;
        public static void RaiseEventOnTimeAdded(long TaskID)
        {
            if (OnTimeAdded != null) OnTimeAdded(TaskID);
        }

        #endregion

        #region OnTaskDeleted

        public static event TaskIDHanlder OnTaskDeleted;
        public static void RaiseEventOnTaskDeleted(long TaskID)
        {
            if (OnTaskDeleted != null) OnTaskDeleted(TaskID);
        }

        #endregion

        #region OnStartTaskPressed

        public static event TaskIDHanlder OnStartTaskPressed;
        public static void RaiseEventOnStartTaskPressed(long TaskID)
        {
            if (OnStartTaskPressed != null) OnStartTaskPressed(TaskID);
        }

        #endregion

        #region OnStopTaskPressed

        public static event TaskIDHanlder OnStopTaskPressed;
        public static void RaiseEventOnStopTaskPressed(long TaskID)
        {
            if (OnStopTaskPressed != null) OnStopTaskPressed(TaskID);
        }

        #endregion

        #region OnTaskCreated

        public static event TaskHanlder OnTaskCreated;
        public static void RaiseEventOnTaskCreated(TaskAdapter Task)
        {
            if (OnTaskCreated != null) OnTaskCreated(Task);
        }

        #endregion

    }
}
