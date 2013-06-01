using System;
using System.ComponentModel;

namespace KeepYourTime.DataBase.Adapters
{
    /// <summary>
    /// Adapter for TaskTime Table
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public class TaskTimeAdapter : INotifyPropertyChanged
    {

        private long lngTimeId = 0;
        private long lngTaskId = 0;
        private DateTime dtStartTime = DateTime.MinValue;
        private DateTime dtStopTime = DateTime.MinValue;


        public long TimeId
        {
            get { return lngTimeId; }
            set
            {
                if (lngTimeId != value)
                {
                    lngTimeId = value;
                    NotifyPropertyChanged("TimeId");
                }
            }
        }
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
        public DateTime StartTime
        {
            get { return dtStartTime; }
            set
            {
                if (dtStartTime != value)
                {
                    dtStartTime = value;
                    NotifyPropertyChanged("StartTime");
                }
            }
        }
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
