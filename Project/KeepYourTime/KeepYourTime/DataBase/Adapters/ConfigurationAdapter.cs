using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.DataBase.Adapters
{
    /// <summary>
    /// Adapter for Configuration Table
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public class ConfigurationAdapter
    {
        public bool Inactivity { get; set; }
        public int InactivityTime { get; set; }

        public List<ShortcutAdapter> Shortcuts { get; set; }
    }
}
