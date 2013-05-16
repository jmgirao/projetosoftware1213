using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.DataBase.Adapters
{
    /// <summary>
    /// Adapter for TaskTime Table
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public class TaskTimeAdapter
    {
        public long TimeId { get; set; }
        public long TaskId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

    }
}
