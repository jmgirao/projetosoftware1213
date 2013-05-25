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
    class InactivityReaction
    {
        Hooks.ActivityHook ahInactivity;

        bool isInactive { get; set; }
        int inactiveTime { get; set; }

        public InactivityReaction(int InactiveTime)
        {
            ahInactivity = new Hooks.ActivityHook();
            isInactive = false;
            this.inactiveTime = InactiveTime;
            ahInactivity.InactiveTimeRefresh += ahInactivity_InactiveTimeRefresh;
        }

        void ahInactivity_InactiveTimeRefresh(int InactiveSeconds)
        {
           // if(InactiveSeconds
        }

        public delegate void InactiveResponseHandler(bool Inactive);
        public event InactiveResponseHandler onInactiveResponse;

    }
}
