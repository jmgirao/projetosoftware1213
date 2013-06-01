using System.Collections.ObjectModel;
using System;
using System.ComponentModel;

namespace KeepYourTime.DataBase.Adapters
{
    /// <summary>
    /// Adapter for Task Table
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public class TaskAdapter : INotifyPropertyChanged
    {


        private long lngTaskId = 0;
        private string strTaskName = "";
        private string strDescription = "";
        private bool blnActive = true;
        private long lngTotalTime = 0;
        private long lngTodayTime = 0;
        private DateTime dtStopTime = DateTime.MinValue;



        public long TaskId
        {
            get { return lngTaskId; }
            set
            {
                if (lngTaskId != value)
                {
                    lngTaskId = value;
                    NotifyPropertyChanged("TaskId");
                }
            }
        }
        public string TaskName
        {
            get { return strTaskName; }
            set
            {
                if (strTaskName != value)
                {
                    strTaskName = value;
                    NotifyPropertyChanged("TaskName");
                }
            }
        }
        public string Description
        {
            get { return strDescription; }
            set
            {
                if (strDescription != value)
                {
                    strDescription = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }
        public bool Active
        {
            get { return blnActive; }
            set
            {
                if (blnActive != value)
                {
                    blnActive = value;
                    NotifyPropertyChanged("Active");
                }
            }
        }

        public long TotalTime
        {
            get { return lngTotalTime; }
            set
            {
                if (lngTotalTime != value)
                {
                    lngTotalTime = value;
                    NotifyPropertyChanged("TotalTime");
                }
            }
        } // total task time in seconds
        public long TodayTime
        {
            get { return lngTodayTime; }
            set
            {
                if (lngTodayTime != value)
                {
                    lngTodayTime = value;
                    NotifyPropertyChanged("TodayTime");
                }
            }
        }  //today time in seconds

        public DateTime StopTime
        {
            get { return dtStopTime; }
            set
            {
                if (dtStopTime != value)
                {
                    dtStopTime = value;
                    NotifyPropertyChanged("StopTime");
                }
            }
        }
        //public bool IsRunning { get; set; }

        public ObservableCollection<TaskTimeAdapter> Times { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
