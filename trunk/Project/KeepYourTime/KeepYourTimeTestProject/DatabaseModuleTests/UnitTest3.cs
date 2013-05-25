using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeepYourTime.Utils;
using System.IO;

namespace KeepYourTimeTestProject.DatabaseModuleTests
{
    [TestClass]
    public class UnitTestExportCSV
    {
        String testPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/testFile.csv";

        [TestMethod]
        public void CreateFile()
        {
            CsvExporter.ExportDatabaseToCSV(testPath);
            Assert.IsTrue(File.Exists(testPath));
        }

        [TestMethod]
        public void FileIsNotEmpty()
        {
            var fileToTest = new FileInfo(testPath);
            Assert.IsTrue(fileToTest.Length != 0);

            Cleanup();
        }

        public void Cleanup()
        {
            File.Delete(testPath);
        }


    }
}
