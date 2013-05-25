using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace KeepYourTime.ViewControls.MainWindowControls
{
    class TaskTimer
    {
        Timer tmTaskTimer;

        public TaskTimer()
        {
            tmTaskTimer = new Timer(100);
            tmTaskTimer.Elapsed += tmTaskTimer_Elapsed;
        }

        private TimeSpan tsInitialTaskTime;
        private DateTime dtStartTiming;
        private TimeSpan CurrentTime;
        private long lngTaskID;

        void tmTaskTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

        }


        public void StartTimingTask(long TaskID)
        {
            var mhResult = new MethodHandler();
            try
            {
                TaskAdapter taTask = null;

                TaskConnector.ReadTask(TaskID, out taTask);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            finally
            {
                ViewWindows.MessageWindow.ShowMethodHandler(mhResult, false);
            }
        }

        public void StopTimingTask()
        {

        }

        public void Time()
        {

        }


    }
}
