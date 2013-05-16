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
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public ObservableCollection<TaskTimeAdapter> Times { get; set; }
    }
}
