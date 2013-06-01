using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.ViewWindows;
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
        public string TotalTime
        {
            get
            {
                String taskAdapTotalTime = "";
                var mhResult = new MethodHandler();

                try
                {

                    foreach (TaskAdapter t in MainWindow.lstTaskAdapt)
                    {
                        if (t.TaskId == this.TaskId)
                        {
                            TimeSpan tsTime = TimeSpan.FromSeconds(t.TotalTime);
                            //taskAdapTotalTime = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                            taskAdapTotalTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                tsTime.Hours,
                                tsTime.Minutes,
                                tsTime.Seconds);

                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    mhResult.Exception(ex);
                    return taskAdapTotalTime;
                }
                finally
                {
                    MessageWindow.ShowMethodHandler(mhResult, false);
                }
                return taskAdapTotalTime;
            }
        }
        public string TodayTime
        {
            get
            {
                String taskAdapTodayTime = "";
                var mhResult = new MethodHandler();

                try
                {
                    foreach (TaskAdapter t in MainWindow.lstTaskAdapt)
                    {
                        if (t.TaskId == this.TaskId)
                        {
                            TimeSpan tsTime = TimeSpan.FromSeconds(t.TodayTime);
                            taskAdapTodayTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                tsTime.Hours,
                                tsTime.Minutes,
                                tsTime.Seconds);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    mhResult.Exception(ex);
                    return taskAdapTodayTime;
                }
                finally
                {
                    MessageWindow.ShowMethodHandler(mhResult, false);
                }
                return taskAdapTodayTime;
            }
        }
        public string StopTime
        {
            get
            {
                String taskAdapStopDate = "";
                var mhResult = new MethodHandler();
                DateTime dtDateSystem = DateTime.Now;
                string stDate = dtDateSystem.ToShortDateString();

                try
                {
                    foreach (TaskAdapter t in MainWindow.lstTaskAdapt)
                    {
                        if (t.TaskId == this.TaskId)
                        {
                            //converter 1/12/2013 para 01/12/2013
                            string strStopDate = t.StopTime.ToShortDateString();
                            if (stDate != strStopDate)
                            {
                                taskAdapStopDate = t.StopTime.ToString();
                                break;
                            }
                            else
                            {
                                string strStopTime = t.StopTime.ToShortTimeString();
                                taskAdapStopDate = strStopTime;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    mhResult.Exception(ex);
                    return taskAdapStopDate;
                }
                finally
                {
                    MessageWindow.ShowMethodHandler(mhResult, false);
                }
                return taskAdapStopDate;
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
        }

        /// <summary>
        /// Occurs when [on task deactivated].
        /// </summary>
        public event EventHandler OnTaskDeactivated;
        public event EventHandler OnTaskDetails;
    }
}
