using KeepYourTime.DataBase.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.ViewControls.MainWindowControls
{
    class TaskAdapterUI : TaskAdapter
    {
        public string TotalTime { get { return "XXX:XX"; } }
        public string TodayTime { get { return "XX:XX"; } }
        public string StopDate { get { return "12/41/1231"; } }

        public bool ActiveChange
        {
            get
            {
                return this.Active;
            }
            set
            {
                this.Active = value;
            }
        }

        public TaskAdapterUI()
        {


        }

        public TaskAdapterUI(TaskAdapter taskBase)
        {
            this.TaskId = taskBase.TaskId;
            this.TaskName = taskBase.TaskName;
            this.Active = taskBase.Active;
            this.Description = taskBase.Description;
        }


    }
}
