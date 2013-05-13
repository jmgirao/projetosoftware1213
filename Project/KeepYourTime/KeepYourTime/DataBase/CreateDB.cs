using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;
using System.Data;

namespace KeepYourTime.DataBase
{
    /// <summary>
    /// Class to Handle Database Creation
    /// </summary>
    /// <remarks>
    /// CREATED BY Rui Ganhoto
    /// </remarks>
    class CreateDB
    {

        /// <summary>
        /// Determines whether database is created.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if database is created; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDatabaseCreated()
        {
            return (File.Exists(DBUtils.FileName));
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        public void CreateDatabase()
        {
            string connectionString = DBUtils.GetConnectionString();
            SqlCeEngine en = new SqlCeEngine(connectionString);
            en.CreateDatabase();
            en.Dispose();

            CreateTableTask();
            CreateTableTaskTime();

            //TODO: se der estoiro, deve apagar a BD.


        }

        /// <summary>
        /// Creates the table task.
        /// </summary>
        private void CreateTableTask()
        {
            string strSQL = "CREATE TABLE Task( " +
                "TaskID bigint IDENTITY(1,1) NOT NULL, " +
                "TaskName varchar(50) NOT NULL, " +
                "Description VARCHAR(8000) NOT NULL, " +
                "Active bit NOT NULL, " +
                ")";

            DBUtils.ExecuteOperation(strSQL);
        }

        /// <summary>
        /// Creates the table task time.
        /// </summary>
        private void CreateTableTaskTime()
        {
            string strSQL = "CREATE TABLE TaskTime( " +
               "TimeId bigint IDENTITY(1,1) NOT NULL, " +
               "TaskId bigint NOT NULL, " +
               "StartTime datetime NOT NULL, " +
               "StopTime datetime NULL, " +
               ")";

            DBUtils.ExecuteOperation(strSQL);
        }

        /// <summary>
        /// Creates the table configuration.
        /// </summary>
        private void CreateTableConfiguration()
        {
            string strSQL = "CREATE TABLE Configuration ( " +
                "InactivityEnabled bit NOT NULL, " +
                "InactivityTime tinyint " +
                "); ";
            DBUtils.ExecuteOperation(strSQL);
        }


        /// <summary>
        /// Creates the table shortcuts.
        /// </summary>
        private void CreateTableShortcuts()
        {
            string strSQL = "CREATE TABLE Shortcut ( " +
                "ShortcutId tinyint NOT NULL, " +
                "TaskId bigint NOT NULL, " +
                "Ctrl bit NOT NULL, " +
                "Alt bit NOT NULL, " +
                "Shift bit NOT NULL, " +
                "Key CHAR(1) NOT NULL " +
                "); ";
            DBUtils.ExecuteOperation(strSQL);

        }


        /// <summary>
        /// Configures the fields.
        /// </summary>
        private void ConfigureFields()
        {
            string strSQL = "ALTER TABLE Task ADD CONSTRAINT UQ_Task_TaskName UNIQUE (TaskName); ";
            DBUtils.ExecuteOperation(strSQL);
        }

        /// <summary>
        /// Creates the primary keys.
        /// </summary>
        private void CreatePrimaryKeys()
        {
            string strSQL = "ALTER TABLE Shortcut ADD CONSTRAINT PK_Shortcut PRIMARY KEY CLUSTERED (ShortcutId);";
            DBUtils.ExecuteOperation(strSQL);

            strSQL = "ALTER TABLE Task ADD CONSTRAINT PK_Task PRIMARY KEY CLUSTERED (TaskID);";
            DBUtils.ExecuteOperation(strSQL);

            strSQL = "ALTER TABLE TaskTime ADD CONSTRAINT PK_Time PRIMARY KEY CLUSTERED (TimeId);";
            DBUtils.ExecuteOperation(strSQL);

        }

        /// <summary>
        /// Creates the foreign keys.
        /// </summary>
        private void CreateForeignKeys()
        {
            string strSQL = "ALTER TABLE Shortcut ADD CONSTRAINT FK_Shortcut_Task FOREIGN KEY (TaskId) REFERENCES Task (TaskID);";
            DBUtils.ExecuteOperation(strSQL);

            strSQL = "ALTER TABLE TaskTime ADD CONSTRAINT FK_Time_Task FOREIGN KEY (TaskId) REFERENCES Task (TaskID);";
            DBUtils.ExecuteOperation(strSQL);
        
        }

        /// <summary>
        /// Inserts the values.
        /// </summary>
        private void InsertValues()
        { 
            //Insert nas UI
            //Insert na BD

        
        }

    }
}
