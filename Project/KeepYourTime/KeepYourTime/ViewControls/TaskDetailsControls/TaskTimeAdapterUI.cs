using KeepYourTime.DataBase.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeepYourTime.ViewControls.TaskDetailsControls
{
    /// <summary>
    /// Class TaskTimeAdapterUI
    /// </summary>
    /// <remarks>CREATED BY João Girão</remarks>
    class TaskTimeAdapterUI : TaskTimeAdapter
    {

        public string TimeSpent
        {
            get
            {
                TimeSpan tsTimeSpent = this.StopTime.Subtract(this.StartTime);
                return tsTimeSpent.Hours + ":" + tsTimeSpent.Minutes + ":" + tsTimeSpent.Seconds;
            }
            set { }
        }

        public TaskTimeAdapterUI(TaskTimeAdapter baseTaskTimer)
        {
            this.TimeId = baseTaskTimer.TimeId;
            this.TaskId = baseTaskTimer.TaskId;
            this.StartTime = baseTaskTimer.StartTime;
            this.StopTime = baseTaskTimer.StopTime;
        }
    }
}
