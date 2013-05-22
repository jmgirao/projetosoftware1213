using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.DataBase.Adapters
{
    /// <summary>
    /// Adapter for Task Table
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public class TaskAdapter
    {
        public long TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }


        //public string TotalTime { get { return "XXX:XX"; } }
        //public string TodayTime { get { return "XX:XX"; } }
        //public string StopDate { get { return "12/41/1231"; } }

        public ObservableCollection<TaskTimeAdapter> Times { get; set; }
    }
}
