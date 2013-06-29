using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.ViewWindows;
using KeepYourTime.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.ViewControls.MainWindowControls
{
    /// <summary>
    /// Class TaskAdapterUI
    /// </summary>
    class TaskAdapterUI : TaskAdapter
    {
        public int TaskRunning {
            get {
               if( MinimalViewControl.CurrentTaskId == this.TaskId)
                   return 1;
               return 0;
            }
        }
        public string TotalTimeString
        {
            get
            {

                TimeSpan tsTime = TimeSpan.FromSeconds(this.TotalTime);
                //taskAdapTotalTime = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                return string.Format("{0:00}:{1:D2}:{2:D2}",
                    (int)tsTime.TotalHours,
                    tsTime.Minutes,
                    tsTime.Seconds);

            }
        }
        public string TodayTimeString
        {
            get
            {

                TimeSpan tsTime = TimeSpan.FromSeconds(this.TodayTime);
                return string.Format("{0:D2}:{1:D2}:{2:D2}",
                    tsTime.Hours,
                    tsTime.Minutes,
                    tsTime.Seconds);



            }
        }
        public string StopTimeString
        {
            get
            {
                //TODO:converter 1/12/2013 para 01/12/2013
                if (StopTime.Date != DateTime.Today)
                {
                    return this.StopTime.ToString("dd/MM/yyyy");
                }
                else
                {
                    return this.StopTime.ToShortTimeString();
                }
            }
        }

        public bool ActiveChange
        {
            get
            {
                return (!this.Active);
            }
            set
            {
                var mhResult = new MethodHandler();

                if (MinimalViewControl.CurrentTaskId != this.TaskId)
                {

                    try
                    {

                        mhResult = TaskConnector.ActivateTask(this.TaskId, !value);
                        if (mhResult.Exits) return;
                        this.Active = !value;

                        if (this.Active == false)
                            if (OnTaskDeactivated != null)
                                OnTaskDeactivated((object)this, new EventArgs());
                    }
                    catch (Exception ex)
                    {
                        mhResult.Exception(ex);
                    }
                    finally
                    {
                        MessageWindow.ShowMethodHandler(mhResult, false);
                    }
                }
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TaskAdapterUI"/> class.
        /// </summary>
        public TaskAdapterUI()
        {
            InitializeEvents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskAdapterUI"/> class.
        /// </summary>
        /// <param name="taskBase">The task base.</param>
        public TaskAdapterUI(TaskAdapter taskBase)
        {
            this.TaskId = taskBase.TaskId;
            this.TaskName = taskBase.TaskName;
            this.Active = taskBase.Active;
            this.Description = taskBase.Description;
            this.TotalTime = taskBase.TotalTime;
            this.TodayTime = taskBase.TodayTime;
            this.StopTime = taskBase.StopTime;
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            StaticEvents.OnTaskStarted += StaticEvents_OnTaskStarted;
            StaticEvents.OnTimeAdded += StaticEvents_OnTimeAdded;
            StaticEvents.OnTaskUpdatedTask += StaticEvents_OnTaskUpdatedTask;
        }

        void StaticEvents_OnTaskUpdatedTask(TaskAdapter TaskAdapt)
        {
            if (TaskAdapt.TaskId == this.TaskId)
            {
                this.TaskName = TaskAdapt.TaskName;
            }
        }

        void StaticEvents_OnTimeAdded(long TaskID)
        {
            if (this.TaskId == TaskID)
            {
                NotifyPropertyChanged("TaskRunning");
            }
        }

        void StaticEvents_OnTaskStarted(long TaskID)
        {
            if (this.TaskId == TaskID)
            {
                NotifyPropertyChanged("TaskRunning");
            }
        }
        
        /// <summary>
        /// Occurs when [on task deactivated].
        /// </summary>
        public event EventHandler OnTaskDeactivated;
        //public event EventHandler OnTaskDetails;
    }
}
