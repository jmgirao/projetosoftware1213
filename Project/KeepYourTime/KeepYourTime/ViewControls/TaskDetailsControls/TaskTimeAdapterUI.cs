using KeepYourTime.DataBase.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeepYourTime.Utils;
using System.ComponentModel;

namespace KeepYourTime.ViewControls.TaskDetailsControls
{
    /// <summary>
    /// Class TaskTimeAdapterUI
    /// </summary>
    /// <remarks>CREATED BY João Girão</remarks>
    public class TaskTimeAdapterUI : TaskTimeAdapter 
    {      

        public string TimeSpent
        {
            get
            {
                TimeSpan tsTimeSpent = this.StopTime.Subtract(this.StartTime);
                return ((int)tsTimeSpent.TotalHours).ToString("###00") + ":" + tsTimeSpent.Minutes.ToString("00") + ":" + tsTimeSpent.Seconds.ToString("00");
            }
            //set { CalculateStopTime(value); }
        }
      
        /// <summary>
        /// function that updates the stoptime and the time spent update in the interface
        /// </summary>
        /// <remarks>Created by Carla Machado & Rui Ganhoto</remarks>
        public DateTime TriggerStoptTime {
            get
            {
                return StopTime;
            }
            set
            {
                if (StopTime != value)
                {
                    NotifyPropertyChanged("TriggerStopTime");
                    StopTime = value;
                    NotifyPropertyChanged("TimeSpent");
                }
            }
        }

        /// <summary>
        /// function that updates the StartTime and assures the time spent update in the interface
        /// </summary>
        /// <remarks>Created by Carla Machado</remarks>
        public DateTime TriggerStartTime
        {
            get
            {
                return StartTime;
            }
            set
            {
                if (StartTime != value)
                {
                    NotifyPropertyChanged("TriggerStartTime");
                    StartTime = value;
                    NotifyPropertyChanged("TimeSpent");
                }
            }
        }

        public TaskTimeAdapterUI() { }

        public TaskTimeAdapterUI(TaskTimeAdapter baseTaskTimer)
        {
            this.TimeId = baseTaskTimer.TimeId;
            this.TaskId = baseTaskTimer.TaskId;
            this.StartTime = baseTaskTimer.StartTime;
            this.StopTime = baseTaskTimer.StopTime;
        }

        //public bool CalculateStopTime(string TimeSpent)
        //{            

        //    if (string.IsNullOrEmpty(TimeSpent) && StartTime != null)
        //    {
        //        TriggerStoptTime = (DateTime)StartTime;
        //        return true;
        //    }
        //    else if (StartTime != null)
        //    {
        //        var dtStopTime = StartTime;
        //        var timeSplit = TimeSpent.Split(':');
                
        //        if (timeSplit.Length == 3)
        //        {

        //            double dbHours = Convert.ToDouble(timeSplit[0]);
        //            dtStopTime = dtStopTime.AddHours(dbHours);
        //            dtStopTime = dtStopTime.AddMinutes(Convert.ToDouble(timeSplit[1]));
        //            //dtStopTime = dtStopTime.AddSeconds(Convert.ToDouble(timeSplit[2]));
        //            TriggerStoptTime = dtStopTime;

        //            return true;
        //        }
        //        return false;
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// trigger of changes by the user
        ///// </summary>
        ///// <param name="PropertyName"></param>
        /////  /// <remarks>Created by Carla Machado & Rui Ganhoto</remarks>
        //private void NotifyPropertyChanged(string PropertyName)
        //{
        //    if(PropertyChanged!=null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        //}

        ///// <summary>
        ///// event for when a property is changed by the user
        ///// </summary>
        ///// <remarks>Created by Carla Machado & Rui Ganhoto</remarks>
        //public event PropertyChangedEventHandler PropertyChanged;
    }
}
