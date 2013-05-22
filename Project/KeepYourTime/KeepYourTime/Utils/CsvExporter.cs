using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeepYourTime.DataBase.Connectors;
using System.Collections.ObjectModel;
using KeepYourTime.DataBase.Adapters;
namespace KeepYourTime.Utils
{

    class CsvExporter
    {
        private static const string FILENAME = "export.csv";
        private static const string TABLE_SEPARATOR = "$$\n";

        public static MethodHandler ExportDatabaseToCSV(string Path)
        {
            var mhResult = new MethodHandler();
            string tasksCsv= "";
            string configCsv= "";

            getTasksAsCsv(out tasksCsv);            
            getConfigAsCsv(out configCsv);

            string csv = tasksCsv + TABLE_SEPARATOR + configCsv;

            writeToFile(Path,csv);

            return mhResult;
        }

        private static MethodHandler getTasksAsCsv(out string Data)
        {

            var mhResult = new MethodHandler();

            ObservableCollection<TaskAdapter> taskList = new ObservableCollection<TaskAdapter>();
            TaskConnector.ReadTaskList(out taskList,true);

            //first row is the column names
            Data =  TaskConnector.COLUMN_ID + "," + TaskConnector.COLUMN_NAME + "," + TaskConnector.COLUMN_DESCRIPTION + "," + TaskConnector.COLUMN_ACTIVE + "\n";
            
            foreach(TaskAdapter task in taskList) 
            {
                Data += "\"" + task.TaskId + "\"," + task.TaskName + "\"," + task.Description + "\"," + task.Active + "\"\n";
            }

            
            return mhResult;
        }

        private static MethodHandler getConfigAsCsv(out string Data)
        {
            var mhResult = new MethodHandler();

            ConfigurationAdapter config;
            ConfigurationConnector.ReadConfiguration(out config);
        
            List<ShortcutAdapter> shortcutList = config.Shortcuts;
            
            //header with the colum names
            Data =  ConfigurationConnector.COLUMN_INACTIVITY_ACTIVE + "," + ConfigurationConnector.COLUMN_INACTIVITY_TIME + "\n";
            
            Data += config.Inactivity +","+config.InactivityTime+"\n";
            Data += "\n";
   
            //header with the colum names
            Data += ConfigurationConnector.COLUMN_SHORTCUT_ID + "," + ConfigurationConnector.COLUMN_SHORTCUT_TASK_ID + "," + ConfigurationConnector.COLUMN_SHORTCUT_CTRL + "," + ConfigurationConnector.COLUMN_SHORTCUT_ALT + "," + ConfigurationConnector.COLUMN_SHORTCUT_SHIFT + "," + ConfigurationConnector.COLUMN_SHORTCUT_KEY + "," + "\n";
            
            foreach(ShortcutAdapter shortcut in shortcutList) 
            {
                Data += "\"" + shortcut.ShortcutId + "\"," + shortcut.TaskId + "\"," + shortcut.Ctrl+ "\"," + shortcut.Alt + "\"," +  shortcut.Shift + "\"," + shortcut.ShortcutKey + "\"\n";
            }
            
            return mhResult;
        }


        private static MethodHandler writeToFile(string Path,string ToWrite) {
            var mhResult = new MethodHandler();

            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter(Path + "/NOME.CSV");
            file.WriteLine(ToWrite);

            file.Close();

            return mhResult;
        }


    
}
