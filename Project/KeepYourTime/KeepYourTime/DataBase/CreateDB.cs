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
    public class CreateDB
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
        /// <returns></returns>
        public MethodHandler CreateDatabase()
        {

            var mhResult = new MethodHandler();
            try
            {
                string connectionString = DBUtils.GetConnectionString();
                SqlCeEngine en = new SqlCeEngine(connectionString);
                en.CreateDatabase();
                en.Dispose();

                mhResult = CreateTableTask();
                if (mhResult.Exits) return mhResult;

                mhResult = CreateTableTaskTime();
                if (mhResult.Exits) return mhResult;

                mhResult = CreateTableConfiguration();
                if (mhResult.Exits) return mhResult;

                mhResult = CreateTableShortcuts();
                if (mhResult.Exits) return mhResult;
                mhResult = ConfigureFields();
                if (mhResult.Exits) return mhResult;

                mhResult = CreatePrimaryKeys();
                if (mhResult.Exits) return mhResult;

                mhResult = CreateForeignKeys();
                if (mhResult.Exits) return mhResult;

                mhResult = InsertValues();
                if (mhResult.Exits) return mhResult;

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            finally
            {
                if (mhResult.Status != Utils.MethodStatus.Sucess)
                {
                    File.Delete(DBUtils.FileName);
                }
            }
            return mhResult;
        }


        /// <summary>
        /// Creates the table task.
        /// </summary>
        /// <returns></returns>
        private MethodHandler CreateTableTask()
        {
            var mhResult = new MethodHandler();
            try
            {
                string strSQL = "CREATE TABLE Task( " +
                              "TaskID bigint IDENTITY(1,1) NOT NULL, " +
                              "TaskName varchar(50) NOT NULL, " +
                              "Description VARCHAR(8000) NOT NULL, " +
                              "Active bit NOT NULL, " +
                              ")";

                DBUtils.ExecuteOperation(strSQL);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;

        }


        /// <summary>
        /// Creates the table task time.
        /// </summary>
        /// <returns></returns>
        private MethodHandler CreateTableTaskTime()
        {
            var mhResult = new MethodHandler();
            try
            {
                string strSQL = "CREATE TABLE TaskTime( " +
                             "TimeId bigint IDENTITY(1,1) NOT NULL, " +
                             "TaskId bigint NOT NULL, " +
                             "StartTime datetime NOT NULL, " +
                             "StopTime datetime NULL, " +
                             ")";

                DBUtils.ExecuteOperation(strSQL);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;

        }


        /// <summary>
        /// Creates the table configuration.
        /// </summary>
        /// <returns></returns>
        private MethodHandler CreateTableConfiguration()
        {
            var mhResult = new MethodHandler();
            try
            {
                string strSQL = "CREATE TABLE Configuration ( " +
                             "InactivityEnabled bit NOT NULL, " +
                             "InactivityTime tinyint " +
                             "); ";
                DBUtils.ExecuteOperation(strSQL);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;

        }



        /// <summary>
        /// Creates the table shortcuts.
        /// </summary>
        /// <returns></returns>
        private MethodHandler CreateTableShortcuts()
        {
            var mhResult = new MethodHandler();
            try
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
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;


        }



        /// <summary>
        /// Configures the fields.
        /// </summary>
        /// <returns></returns>
        private MethodHandler ConfigureFields()
        {
            var mhResult = new MethodHandler();
            try
            {
                string strSQL = "ALTER TABLE Task ADD CONSTRAINT UQ_Task_TaskName UNIQUE (TaskName); ";
                DBUtils.ExecuteOperation(strSQL);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;

        }


        /// <summary>
        /// Creates the primary keys.
        /// </summary>
        /// <returns></returns>
        private MethodHandler CreatePrimaryKeys()
        {
            var mhResult = new MethodHandler();
            try
            {
                string strSQL = "ALTER TABLE Shortcut ADD CONSTRAINT PK_Shortcut PRIMARY KEY CLUSTERED (ShortcutId);";
                DBUtils.ExecuteOperation(strSQL);

                strSQL = "ALTER TABLE Task ADD CONSTRAINT PK_Task PRIMARY KEY CLUSTERED (TaskID);";
                DBUtils.ExecuteOperation(strSQL);

                strSQL = "ALTER TABLE TaskTime ADD CONSTRAINT PK_Time PRIMARY KEY CLUSTERED (TimeId);";
                DBUtils.ExecuteOperation(strSQL);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;


        }


        /// <summary>
        /// Creates the foreign keys.
        /// </summary>
        /// <returns></returns>
        private MethodHandler CreateForeignKeys()
        {

            var mhResult = new MethodHandler();
            try
            {
                string strSQL = "ALTER TABLE Shortcut ADD CONSTRAINT FK_Shortcut_Task FOREIGN KEY (TaskId) REFERENCES Task (TaskID);";
                DBUtils.ExecuteOperation(strSQL);

                strSQL = "ALTER TABLE TaskTime ADD CONSTRAINT FK_Time_Task FOREIGN KEY (TaskId) REFERENCES Task (TaskID);";
                DBUtils.ExecuteOperation(strSQL);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }


        /// <summary>
        /// Inserts the values.
        /// </summary>
        /// <returns></returns>
        private MethodHandler InsertValues()
        {
            var mhResult = new MethodHandler();
            try
            {
                //Insert nas UI
                //Insert na BD
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }

    }
}
