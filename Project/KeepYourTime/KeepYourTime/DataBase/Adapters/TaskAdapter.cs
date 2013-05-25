using System.Collections.ObjectModel;

namespace KeepYourTime.DataBase.Adapters
{
    /// <summary>
    /// Adapter for Task Table
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public class TaskAdapter
    {
        public long TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public long TotalTime { get; set; } // total task time in seconds
        public long TodayTime { get; set; }  //today time in seconds

        public bool IsRunning { get; set; }

        public ObservableCollection<TaskTimeAdapter> Times { get; set; }
    }
}
