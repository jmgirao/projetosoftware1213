using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.DataBase.Adapters
{
    class ConfigurationAdapter
    {
        public bool Inactivity { get; set; }
        public int InactivityTime { get; set; }
    }
}
