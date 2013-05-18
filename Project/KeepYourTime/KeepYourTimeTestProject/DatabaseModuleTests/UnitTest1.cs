using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeepYourTime.DataBase.Adapters;

namespace KeepYourTimeTestProject.DatabaseModuleTests
{
    /// <summary>
    /// Unit Tests for Database
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Creates the data base.
        /// </summary>
        [TestMethod]
        public void CreateDataBase()
        {
            var cdb = new KeepYourTime.DataBase.CreateDB();
            Assert.AreEqual(KeepYourTime.Utils.MethodStatus.Sucess, cdb.CreateDatabase().Status);
        }


        /// <summary>
        /// Inserts the task.
        /// </summary>
        [TestMethod]
        public void InsertTask()
        {
            long taskId = 0;
            Assert.AreEqual(KeepYourTime.Utils.MethodStatus.Sucess, KeepYourTime.DataBase.Connectors.TaskConnector.CreateTask("Tarefa: " + DateTime.Now.ToString("yyMMdd_hhmmss"), out taskId).Status);
        }


        /// <summary>
        /// Reads the task.
        /// </summary>
        [TestMethod]
        public void ReadTask()
        {
            TaskAdapter ta = null;
            Assert.AreEqual(KeepYourTime.Utils.MethodStatus.Sucess, KeepYourTime.DataBase.Connectors.TaskConnector.ReadTask(1, out ta).Status);            
        }


        /// <summary>
        /// Creates the time.
        /// </summary>
        [TestMethod]
        public void CreateTime()
        {
            var tt = new TaskTimeAdapter();
            tt.StartTime = DateTime.Now;
            tt.StopTime = DateTime.Now.AddMinutes(1);
            tt.TaskId = 1;
            Assert.AreEqual(KeepYourTime.Utils.MethodStatus.Sucess, KeepYourTime.DataBase.Connectors.TaskConnector.AddTime(tt).Status);            
        }


        /// <summary>
        /// Reads the configs.
        /// </summary>
        [TestMethod]
        public void ReadConfigs()
        { 
            ConfigurationAdapter cf = null;
            Assert.AreEqual(KeepYourTime.Utils.MethodStatus.Sucess, KeepYourTime.DataBase.Connectors.ConfigurationConnector.ReadConfiguration(out cf).Status);
        }


        /// <summary>
        /// Changes the config.
        /// </summary>
        [TestMethod]
        public void ChangeConfig()
        {
            var cf = new ConfigurationAdapter();
            cf.Inactivity = true;
            cf.InactivityTime = 5;
            cf.Shortcuts =  new System.Collections.Generic.List<ShortcutAdapter>();
            Assert.AreEqual(KeepYourTime.Utils.MethodStatus.Sucess, KeepYourTime.DataBase.Connectors.ConfigurationConnector.ReadConfiguration(out cf).Status);
        }


        /// <summary>
        /// Changes the config2.
        /// </summary>
        [TestMethod]
        public void ChangeConfig2()
        {
            var cf = new ConfigurationAdapter();
            cf.Inactivity = true;
            cf.InactivityTime = 5;
            cf.Shortcuts = null;
            Assert.AreEqual(KeepYourTime.Utils.MethodStatus.Sucess, KeepYourTime.DataBase.Connectors.ConfigurationConnector.ReadConfiguration(out cf).Status);
        }


        //[TestMethod]
        //public void ReadTask()
        //{
        //    TaskAdapter ta = null;
        //    Assert.AreEqual(KeepYourTime.Utils.MethodStatus.Sucess, KeepYourTime.DataBase.Connectors.TaskConnector.ReadTask(1, out ta));
        //}



    }
}
