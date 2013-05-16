using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeepYourTimeTestProject.DatabaseModuleTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cdb = new KeepYourTime.DataBase.CreateDB();
            Assert.AreEqual(KeepYourTime.Utils.MethodStatus.Sucess, cdb.CreateDatabase().Status);
        }

        [TestMethod]
        public void TestMethod2()
        {
            long taskId = 0;
            Assert.AreEqual(KeepYourTime.Utils.MethodStatus.Sucess, KeepYourTime.DataBase.Connectors.TaskConnector.CreateTask("Name2", out taskId).Status);
        }

    }
}
