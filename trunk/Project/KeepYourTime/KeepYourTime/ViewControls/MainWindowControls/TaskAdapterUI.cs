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
        /*public string TotalTime { get { return "XXX:XX"; } }
        public string TodayTime { get { return "XX:XX"; } }
        public string StopDate { get { return "12/41/1231"; } }*/

        public TaskAdapterUI()
        {
            
          
        }

        public TaskAdapterUI(TaskAdapter taskBase)
        {
            //base(taskBase.TotalTime, taskBase.TodayTime, taskBase, StopDate);

        }
        
        
    }
}
