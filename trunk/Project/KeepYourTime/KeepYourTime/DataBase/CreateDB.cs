using System;
using System.IO;
using System.Data.SqlServerCe;

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
        /// <returns>Method Handler with the method status</returns>
        public MethodHandler CreateDatabase()
        {
            var mhResult = new MethodHandler();
            try
            {
                if (File.Exists(DBUtils.FileName)) return mhResult;

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

                //mhResult = InsertValues();
                //if (mhResult.Exits) return mhResult;

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            finally
            {
                if (mhResult.Status != MethodStatus.Success)
                {
                    File.Delete(DBUtils.FileName);
                }
            }
            return mhResult;
        }


        /// <summary>
        /// Creates the table task.
        /// </summary>
        /// <returns>Method Handler with the method status</returns>
        private MethodHandler CreateTableTask()
        {
            var mhResult = new MethodHandler();
            try
            {
                //The variable is initializated with the pretended query and will never be updated
                string strSQL = "CREATE TABLE Task( " +
                              "TaskID bigint IDENTITY(1,1) NOT NULL, " +
                              "TaskName NVARCHAR(50) NOT NULL, " +
                              "Description NVARCHAR(4000) NOT NULL, " +
                              "Active bit NOT NULL " +
                              ")";

                mhResult = DBUtils.ExecuteOperation(strSQL);
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
        /// <returns>Method Handler with the method status</returns>
        private MethodHandler CreateTableTaskTime()
        {
            var mhResult = new MethodHandler();
            try
            {
                //The variable is initializated with the pretended query and will never be updated
                string strSQL = "CREATE TABLE TaskTime( " +
                             "TimeId bigint IDENTITY(1,1) NOT NULL, " +
                             "TaskId bigint NOT NULL, " +
                             "StartTime datetime NOT NULL, " +
                             "StopTime datetime NULL " +
                             ")";

                mhResult = DBUtils.ExecuteOperation(strSQL);
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
        /// <returns>Method Handler with the method status</returns>
        private MethodHandler CreateTableConfiguration()
        {
            var mhResult = new MethodHandler();
            try
            {
                //The variable is initializated with the pretended query and will never be updated
                string strSQL = "CREATE TABLE Configuration ( " +
                             "InactivityEnabled bit NOT NULL, " +
                             "InactivityTime tinyint " +
                             "); ";
                mhResult = DBUtils.ExecuteOperation(strSQL);
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
        /// <returns>Method Handler with the method status</returns>
        private MethodHandler CreateTableShortcuts()
        {
            var mhResult = new MethodHandler();
            try
            {
                //The variable is initializated with the pretended query and will never be updated
                string strSQL = "CREATE TABLE Shortcut ( " +
                             "ShortcutId tinyint NOT NULL, " +
                             "TaskId bigint NULL, " +
                             "Ctrl bit NOT NULL, " +
                             "Alt bit NOT NULL, " +
                             "Shift bit NOT NULL, " +
                             "ShortcutKey NCHAR(1) NOT NULL " +
                             "); ";
                mhResult = DBUtils.ExecuteOperation(strSQL);
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
        /// <returns>Method Handler with the method status</returns>
        private MethodHandler ConfigureFields()
        {
            var mhResult = new MethodHandler();
            try
            {
                //The variable is initializated with the pretended query and will never be updated
                string strSQL = "ALTER TABLE Task ADD CONSTRAINT UQ_Task_TaskName UNIQUE (TaskName); ";
                mhResult = DBUtils.ExecuteOperation(strSQL);
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
        /// <returns>Method Handler with the method status</returns>
        private MethodHandler CreatePrimaryKeys()
        {
            var mhResult = new MethodHandler();
            try
            {
                string strSQL = "";
                strSQL = "ALTER TABLE Shortcut ADD CONSTRAINT PK_Shortcut PRIMARY KEY (ShortcutId);";
                mhResult = DBUtils.ExecuteOperation(strSQL);
                if (mhResult.Exits) return mhResult;

                strSQL = "ALTER TABLE Task ADD CONSTRAINT PK_Task PRIMARY KEY (TaskID);";
                mhResult = DBUtils.ExecuteOperation(strSQL);
                if (mhResult.Exits) return mhResult;

                strSQL = "ALTER TABLE TaskTime ADD CONSTRAINT PK_Time PRIMARY KEY (TimeId);";
                mhResult = DBUtils.ExecuteOperation(strSQL);
                if (mhResult.Exits) return mhResult;
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
        /// <returns>Method Handler with the method status</returns>
        private MethodHandler CreateForeignKeys()
        {

            var mhResult = new MethodHandler();
            try
            {
                string strSQL = "";
                strSQL = "ALTER TABLE Shortcut ADD CONSTRAINT FK_Shortcut_Task FOREIGN KEY (TaskId) REFERENCES Task (TaskID);";
                mhResult = DBUtils.ExecuteOperation(strSQL);
                if (mhResult.Exits) return mhResult;

                strSQL = "ALTER TABLE TaskTime ADD CONSTRAINT FK_Time_Task FOREIGN KEY (TaskId) REFERENCES Task (TaskID);";
                mhResult = DBUtils.ExecuteOperation(strSQL);
                if (mhResult.Exits) return mhResult;
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
        /// <returns>Method Handler with the method status</returns>
        //private MethodHandler InsertValues()
        //{
        //    var mhResult = new MethodHandler();
        //    try
        //    {
        //        //string strSQL = "INSERT INTO Configuration";
        //        //Insert nas UI
        //        //Insert na BD

        //    }
        //    catch (Exception ex)
        //    {
        //        mhResult.Exception(ex);
        //    }
        //    return mhResult;
        //}

    }
}
