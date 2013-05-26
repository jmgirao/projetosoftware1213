using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.ViewControls.MainWindowControls
{
    /// <summary>
    /// Class InactivityReaction
    /// </summary>
    /// <remarks>CREATED BY João Girão</remarks>
    class InactivityDetection
    {

       // Hooks.ActivityHook ahHook;
        TaskTimer ttTimer;
        bool isInactive { get; set; }
        long TaskId { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="InactivityDetection"/> class.
        /// </summary>
        /// <param name="TaskId">The task id.</param>
        /// <param name="Timer">The timer.</param>
        public InactivityDetection(long TaskId, TaskTimer Timer)
        {
           // ahHook = new Hooks.ActivityHook();
            isInactive = false;
            this.TaskId = TaskId;
            ttTimer = Timer;
        }

        /// <summary>
        /// Checks the inactive.
        /// </summary>
        /// <param name="InactiveSeconds">The inactive seconds.</param>
        /// <returns><c>true</c>  <c>false</c> otherwise</returns>
        public bool CheckInactive(int InactiveSeconds)
        {
            if (InactiveSeconds >= Utils.CurrentConfigurations.allConfig.InactivityTime*60 && !isInactive)
            {
                ttTimer.StopTimingTask();
                ttTimer.StartTimingTask(TaskId);
                isInactive = true;
            }

            if (InactiveSeconds == 0 && isInactive)
                isInactive = false;
       
            return isInactive;

        }

    }
}
