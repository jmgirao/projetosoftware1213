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
            Time();
        }


        public void StartTimingTask(long TaskID)
        {
            var mhResult = new MethodHandler();
            try
            {
                TaskAdapter taTask = null;
                tsInitialTaskTime = new TimeSpan(0, 0, 0);
                mhResult = TaskConnector.ReadTask(TaskID, out taTask);
                if (mhResult.Exits) return;

                foreach (TaskTimeAdapter t in taTask.Times)
                {
                    tsInitialTaskTime.Add(t.StopTime.Subtract(t.StartTime));
                }
                lngTaskID = TaskID;
                CurrentTime = new TimeSpan();
                dtStartTiming = DateTime.Now;
                tmTaskTimer.Start();
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

        public bool isRunningTask()
        {
            return tmTaskTimer.Enabled;
        }

        public TaskTimeAdapter StopTimingTask()
        {

            tmTaskTimer.Stop();
            TaskTimeAdapter ttTime = new TaskTimeAdapter();
            ttTime.StartTime = dtStartTiming;
            ttTime.StopTime = DateTime.Now;
            ttTime.TaskId = lngTaskID;
            return ttTime;
        }

        public void Time()
        {
            if (CurrentTime != DateTime.Now.Subtract(dtStartTiming))
            {
                CurrentTime = DateTime.Now.Subtract(dtStartTiming);
                var tmTotalTime = CurrentTime.Add(tsInitialTaskTime);

                string strTimeString = tmTotalTime.TotalHours.ToString("00") + ":" +
                    tmTotalTime.Minutes.ToString("00") + ":" +
                    tmTotalTime.Seconds.ToString("00");

                if (onTimeChanged != null)
                    onTimeChanged(strTimeString);
            }
        }

        public delegate void TimeChangedHandler(string time);
        public event TimeChangedHandler onTimeChanged;

    }
}
