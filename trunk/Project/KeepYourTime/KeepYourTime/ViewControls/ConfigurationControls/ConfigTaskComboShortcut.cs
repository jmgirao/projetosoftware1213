using KeepYourTime.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.ViewControls.ConfigurationControls
{
    class ConfigTaskComboShortcut : INotifyPropertyChanged
    {

        public ConfigTaskComboShortcut()
        {
            StaticEvents.OnTaskUpdatedTask += StaticEvents_OnTaskUpdatedTask;
        }

        void StaticEvents_OnTaskUpdatedTask(DataBase.Adapters.TaskAdapter TaskAdapt)
        {
            if (TaskAdapt.TaskId == TaskID)
            {
                TaskName = TaskAdapt.TaskName;
            }
        }

        private string _TaskName = "";

        public string TaskName {
            get { return _TaskName; }
            set
            {
                if (value != _TaskName)
                {
                    _TaskName = value;
                    NotifyPropertyChanged("TaskName");
                }
            }
        }

        private long _TaskID = 0;
        public long TaskID
        {
            get { return _TaskID; }
            set
            {
                if (value != _TaskID)
                {
                    _TaskID = value;
                    NotifyPropertyChanged("TaskID");
                }
            }
        }



        public override string ToString()
        {
            return TaskName;
        }


        private void NotifyPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
