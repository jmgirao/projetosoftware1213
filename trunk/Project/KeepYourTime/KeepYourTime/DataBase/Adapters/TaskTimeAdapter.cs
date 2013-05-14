using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.DataBase.Adapters
{
    public class TaskTimeAdapter
    {
        public int TimeId { get; set; }
        public int TaskId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

    }
}
