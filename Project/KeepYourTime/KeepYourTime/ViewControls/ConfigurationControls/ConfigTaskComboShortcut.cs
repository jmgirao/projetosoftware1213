using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.ViewControls.ConfigurationControls
{
    class ConfigTaskComboShortcut
    {
        public string TaskName { get; set; }
        public long TaskID { get; set; }

        public override string ToString()
        {
            return TaskName;
        }

    }
}
