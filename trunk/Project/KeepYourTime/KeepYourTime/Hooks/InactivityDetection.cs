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
        //TaskTimer ttTimer;
        static bool isInactive;
        //long TaskId;
        public static int RemoveSeconds = 0;


        /// <summary>
        /// Initializes a new instance of the <see cref="InactivityDetection"/> class.
        /// </summary>
        /// <param name="TaskId">The task id.</param>
        /// <param name="Timer">The timer.</param>
        //public InactivityDetection(long TaskId)
        //{
        //    // ahHook = new Hooks.ActivityHook();
        //    isInactive = false;
        //    this.TaskId = TaskId;
        //}

        /// <summary>
        /// Checks the inactive.
        /// </summary>
        /// <param name="InactiveSeconds">The inactive seconds.</param>
        /// <returns><c>true</c>  <c>false</c> otherwise</returns>
        public static bool CheckInactive(int InactiveSeconds)
        {
            if(!Utils.CurrentConfigurations.allConfig.Inactivity) return false;
            if (InactiveSeconds >= Utils.CurrentConfigurations.allConfig.InactivityTime * 60 && !isInactive)
            {
                isInactive = true;
                return false;
            }

            if (InactiveSeconds < 5 && isInactive)
            {
                isInactive = false;
                return true;
            }

            if (isInactive)
                RemoveSeconds = InactiveSeconds;


            return false;

        }

    }
}
