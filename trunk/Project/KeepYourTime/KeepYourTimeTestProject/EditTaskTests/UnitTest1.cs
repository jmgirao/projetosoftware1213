using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeepYourTime.ViewControls.TaskDetailsControls;
using System.Collections.ObjectModel;

namespace KeepYourTimeTestProject.EditTaskTests
{
    /// <summary>
    /// Unit Tests to edit task
    /// </summary>
    /// <remarks>Created by Carla Machado</remarks>
    [TestClass]
    public class UnitTest1
    {    

        [TestMethod]
        public void ValidateTaskTimesNullTest ()
        {
            EditTask editTaskTest = new EditTask();
            TaskTimeAdapterUI times = new TaskTimeAdapterUI();

            Assert.IsFalse(editTaskTest.ValidateTaskTime(times));
        }

        [TestMethod]
        public void ValidateTaskTimesStopStartTest()
        {
            EditTask editTaskTest = new EditTask();
            TaskTimeAdapterUI times = new TaskTimeAdapterUI() { StartTime = DateTime.Now.AddDays(1.0), StopTime = DateTime.Now };

            Assert.IsFalse(editTaskTest.ValidateTaskTime(times));
        }

        [TestMethod]
        public void ValidateTaskTimesStopStartSameTest()
        {
            EditTask editTaskTest = new EditTask();
            TaskTimeAdapterUI times = new TaskTimeAdapterUI() { StartTime = DateTime.Now, StopTime = DateTime.Now };

            Assert.IsFalse(editTaskTest.ValidateTaskTime(times));
        }

        [TestMethod]
        public void ValidateTaskTimesTest()
        {
            EditTask editTaskTest = new EditTask();
            TaskTimeAdapterUI times = new TaskTimeAdapterUI() { StartTime = DateTime.Now, StopTime = DateTime.Now.AddDays(1.0) };

            Assert.IsTrue(editTaskTest.ValidateTaskTime(times));
        }


        [TestMethod]
        public void ValidateCoincidentStartTimesTest()
        {
            EditTask editTaskTest = new EditTask();
            ObservableCollection<TaskTimeAdapterUI> times = new ObservableCollection<TaskTimeAdapterUI>();
            times.Add(new TaskTimeAdapterUI() { StartTime = DateTime.Now, StopTime = DateTime.Now.AddHours(2.0), TimeId = 1 });
            times.Add(new TaskTimeAdapterUI() { StartTime = DateTime.Now, StopTime = DateTime.Now.AddHours(1.0), TimeId = 2 });

            Assert.IsFalse(editTaskTest.ValidateDistinctTime(times));
        }

        [TestMethod]
        public void ValidateCoincidentStoptTimesTest()
        {
            EditTask editTaskTest = new EditTask();
            ObservableCollection<TaskTimeAdapterUI> times = new ObservableCollection<TaskTimeAdapterUI>();
            times.Add(new TaskTimeAdapterUI() { StartTime = DateTime.Now, StopTime = DateTime.Now.AddHours(4.0), TimeId=1 });
            times.Add(new TaskTimeAdapterUI() { StartTime = DateTime.Now.AddHours(1.5), StopTime = DateTime.Now.AddHours(4.0), TimeId = 2 });

            Assert.IsFalse(editTaskTest.ValidateDistinctTime(times));
        }

        [TestMethod]
        public void ValidateCoincidentIntervalTimesTest()
        {
            EditTask editTaskTest = new EditTask();
            ObservableCollection<TaskTimeAdapterUI> times = new ObservableCollection<TaskTimeAdapterUI>();
            times.Add(new TaskTimeAdapterUI() { StartTime = DateTime.Now, StopTime = DateTime.Now.AddHours(4.0), TimeId = 1 });
            times.Add(new TaskTimeAdapterUI() { StartTime = DateTime.Now.AddHours(1.0), StopTime = DateTime.Now.AddHours(2.0), TimeId = 2 });

            Assert.IsFalse(editTaskTest.ValidateDistinctTime(times));
        }

        [TestMethod]
        public void ValidateCoincidentIntervalTimesTest2()
        {
            EditTask editTaskTest = new EditTask();
            ObservableCollection<TaskTimeAdapterUI> times = new ObservableCollection<TaskTimeAdapterUI>();
            times.Add(new TaskTimeAdapterUI() { StartTime = DateTime.Now, StopTime = DateTime.Now.AddHours(1.5), TimeId = 1 });
            times.Add(new TaskTimeAdapterUI() { StartTime = DateTime.Now.AddHours(1.0), StopTime = DateTime.Now.AddHours(4.0), TimeId = 2 });

            Assert.IsFalse(editTaskTest.ValidateDistinctTime(times));
        }

        [TestMethod]
        public void ValidateCoincidentIntervalTimesTest3()
        {
            EditTask editTaskTest = new EditTask();
            ObservableCollection<TaskTimeAdapterUI> times = new ObservableCollection<TaskTimeAdapterUI>();
            times.Add(new TaskTimeAdapterUI() { StartTime = DateTime.Now.AddHours(1.5), StopTime = DateTime.Now.AddHours(6.0), TimeId = 1 });
            times.Add(new TaskTimeAdapterUI() { StartTime = DateTime.Now, StopTime = DateTime.Now.AddHours(4.0), TimeId = 2 });

            Assert.IsFalse(editTaskTest.ValidateDistinctTime(times));
        }
    }
}
