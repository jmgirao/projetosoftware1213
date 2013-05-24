using System;

namespace KeepYourTime.DataBase.Adapters
{
    /// <summary>
    /// Adapter for TaskTime Table
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public class TaskTimeAdapter
    {
        public long TimeId { get; set; }
        public long TaskId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }

    }
}
