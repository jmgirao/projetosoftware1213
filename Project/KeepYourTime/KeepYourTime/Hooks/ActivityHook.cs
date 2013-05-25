using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace KeepYourTime.Hooks
{
    class ActivityHook
    {


        Timer inactivityTime;
        public int InactivityMinutes = 10;

        public ActivityHook()
        {
            inactivityTime = new Timer(1000);
            inactivityTime.Elapsed += inactivityTime_Elapsed;
        }

        void inactivityTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (InactiveTimeRefresh != null)
                InactiveTimeRefresh(InactiveTime());
            //MessageBox.Show(InactiveTime().ToString());
        }

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(out LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public static readonly int SizeOf =
                   Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public int dwTime;
        }


        public delegate void InactiveTimeHandler(int InactiveSeconds);

        public event InactiveTimeHandler InactiveTimeRefresh;

        public void InitTimer()
        {
            inactivityTime.Start();
        }

        public void StopTimer()
        {
            inactivityTime.Stop();
        }

        public int InactiveTime()
        {
            int idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            int envTicks = Environment.TickCount;

            if (GetLastInputInfo(out lastInputInfo))
            {
                int lastInputTick = lastInputInfo.dwTime;
                idleTime = envTicks - lastInputTick;
            }

            int a;

            if (idleTime > 0)
                a = idleTime / 1000;
            else
                a = idleTime;
            return a;
        }
    }
}
