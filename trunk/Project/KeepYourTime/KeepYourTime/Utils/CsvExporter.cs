using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeepYourTime.DataBase.Connectors;
using System.Collections.ObjectModel;
using KeepYourTime.DataBase.Adapters;
using System.Security;
using System.Security.Permissions;
namespace KeepYourTime.Utils
{
    /// <summary>
    /// Methods to export the database to CSV
    /// </summary>
    /// <remarks>CREATED BY Filipe Brandão</remarks>
    public class CsvExporter
    {
        private static string TABLE_SEPARATOR = "$$\n";

        /// <summary>
        /// Exports the database to csv
        /// </summary>
        /// <param name="Path">The path to save the csv file.</param>
        public static void ExportDatabaseToCSV(string Path)
        {
            string tasksCsv = "";
            string configCsv = "";

            tasksCsv = getTasksAsCsv();
            configCsv = getConfigAsCsv();

            string csv = tasksCsv + TABLE_SEPARATOR + configCsv;

            writeToFile(Path, csv);

        }

        /// <summary>
        /// get Tasks table as csv 
        /// </summary>
        /// <returns> String that contains the CSV lines from the Tasks table</returns>
        private static string getTasksAsCsv()
        {
            string Data = "";

            ObservableCollection<TaskAdapter> taskList = new ObservableCollection<TaskAdapter>();
            TaskConnector.ReadTaskList(out taskList, true);

            //first row is the column names
            Data = TaskConnector.COLUMN_ID + "," + TaskConnector.COLUMN_NAME + "," + TaskConnector.COLUMN_DESCRIPTION + "," + TaskConnector.COLUMN_ACTIVE + "\n";

            foreach (TaskAdapter task in taskList)
            {
                Data += "\"" + task.TaskId + "\",\"" + task.TaskName + "\",\"" + task.Description + "\",\"" + task.Active + "\"\n";
            }

            return Data;
        }

        /// <summary>
        /// get Config table as csv 
        /// </summary>
        /// <returns> String that contains the CSV lines from the Config table</returns>
        private static string getConfigAsCsv()
        {
            String Data = "";
            ConfigurationAdapter config;
            ConfigurationConnector.ReadConfiguration(out config);

            List<ShortcutAdapter> shortcutList = config.Shortcuts;

            //header with the colum names
            Data = ConfigurationConnector.COLUMN_INACTIVITY_ACTIVE + "," + ConfigurationConnector.COLUMN_INACTIVITY_TIME + "\n";

            Data += "\"" + config.Inactivity + "\",\"" + config.InactivityTime + "\"\n";
            Data += TABLE_SEPARATOR;

            //header with the colum names
            Data += ConfigurationConnector.COLUMN_SHORTCUT_ID + "," + ConfigurationConnector.COLUMN_SHORTCUT_TASK_ID + "," + ConfigurationConnector.COLUMN_SHORTCUT_CTRL + "," + ConfigurationConnector.COLUMN_SHORTCUT_ALT + "," + ConfigurationConnector.COLUMN_SHORTCUT_SHIFT + "," + ConfigurationConnector.COLUMN_SHORTCUT_KEY + "\n";

            foreach (ShortcutAdapter shortcut in shortcutList)
            {
                Data += "\"" + shortcut.ShortcutId + "\",\"" + shortcut.TaskId + "\",\"" + shortcut.Ctrl + "\",\"" + shortcut.Alt + "\",\"" + shortcut.Shift + "\",\"" + shortcut.ShortcutKey + "\"\n";
            }

            return Data;

        }

        /// <summary>
        /// Writes text to file 
        /// </summary>
        /// <param name="Path">Path to write to</param>
        /// <param name="Path">Content to write on the file</param>

        private static void writeToFile(string Path, string ToWrite)
        {
            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter(Path);
            file.WriteLine(ToWrite);
            file.Close();
        }

    }


}
