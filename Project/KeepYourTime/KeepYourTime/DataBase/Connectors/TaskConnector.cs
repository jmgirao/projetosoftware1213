using KeepYourTime.DataBase.Adapters;
using KeepYourTime.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.DataBase.Connectors
{
    /// <summary>
    /// Connection betwen Tasks and DB
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public class TaskConnector
    {
        public static string TASK_COLUMN_ID = "TaskID";
        public static string TASK_COLUMN_NAME = "TaskName";
        public static string TASK_COLUMN_DESCRIPTION = "Description";
        public static string TASK_COLUMN_ACTIVE = "Active";

        public static string TIME_COLUMN_ID = "TaskID";
        public static string TIME_COLUMN_START = "StartTime";
        public static string TIME_COLUMN_STOP = "StopTime";
        public static string TIME_COLUMN_TASKID = "Active";


        /// <summary>
        /// Reads the task.
        /// </summary>
        /// <param name="TaskID">The task ID.</param>
        /// <param name="Task">The task.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler ReadTask(long TaskID, out TaskAdapter Task)
        {
            var mhResult = new MethodHandler();
            Task = null;
            try
            {
                DataTable dtTask = null;
                string strQuery = "";
                strQuery = "SELECT TaskId, TaskName, Description, Active FROM Task WHERE TaskId = " + TaskID.ToString() + " ";
                mhResult = DBUtils.SelectTable(strQuery, out dtTask);
                if (mhResult.Exits) return mhResult;

                if (mhResult.AffectedLines == 0)
                {
                    mhResult.Status = MethodStatus.Cancel;
                    mhResult.Message = String.Format(Languages.Language.TaskNotFound, TaskID);
                    return mhResult;
                }

                Task = new TaskAdapter()
                {
                    TaskId = (long)dtTask.Rows[0]["TaskId"],
                    TaskName = (string)dtTask.Rows[0]["TaskName"],
                    Description = (string)dtTask.Rows[0]["Description"],
                    Active = (bool)dtTask.Rows[0]["Active"],
                    Times = new ObservableCollection<TaskTimeAdapter>()
                };

                strQuery = "SELECT TimeId, StartTime, StopTime FROM TaskTime WHERE TaskID = " + TaskID.ToString() + " ORDER BY StartTime";
                DataTable dtTaskTime = null;

                mhResult = DBUtils.SelectTable(strQuery, out dtTaskTime);
                if (mhResult.Exits) return mhResult;

                foreach (DataRow dr in dtTaskTime.Rows)
                {
                    Task.Times.Add(new TaskTimeAdapter
                    {
                        TaskId = TaskID,
                        TimeId = (long)dr["TimeId"],
                        StartTime = (DateTime)dr["StartTime"],
                        StopTime = (DateTime)dr["StopTime"]
                    });
                }

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }

        /// <summary>
        /// Creates the task.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="TaskID">The task ID.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler CreateTask(string Name, out long TaskID)
        {
            var mhResult = new MethodHandler();
            TaskID = 0;
            try
            {

                //Verify if task with same name exists
                string strSQL = "SELECT 1 FROM Task WHERE TaskName = @Name ";
                object objReturn = null;

                SqlCeParameter[] scpParams = new SqlCeParameter[1];
                scpParams[0] = new SqlCeParameter("Name", Name);
                scpParams[0].DbType = DbType.String;

                mhResult = DBUtils.SelectValue(strSQL, scpParams, out objReturn);
                if (mhResult.Exits) return mhResult;
                if (mhResult.AffectedLines == 1)
                {
                    mhResult.Status = MethodStatus.Cancel;
                    mhResult.Message = Languages.Language.TaskExists;
                    return mhResult;
                }

                //Create new task
                strSQL = "INSERT INTO Task (TaskName, Description, Active ) VALUES (@Name, '', 1)";
                scpParams[0] = new SqlCeParameter("Name", Name);
                scpParams[0].DbType = DbType.String;
                mhResult = DBUtils.ExecuteOperation(strSQL, scpParams);
                if (mhResult.Exits) return mhResult;
                if (mhResult.AffectedLines == 0)
                {
                    mhResult.Status = MethodStatus.Cancel;
                    mhResult.Message = Languages.Language.UnpsecifiedErrorCreateTask;
                    return mhResult;
                }


                //Get Task ID
                strSQL = "SELECT MAX(TaskID) FROM Task ";
                mhResult = DBUtils.SelectValue(strSQL, out objReturn);
                if (mhResult.Exits) return mhResult;
                TaskID = (long)objReturn;
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }

        /// <summary>
        /// Edits the task.
        /// </summary>
        /// <param name="Task">The task.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler EditTask(TaskAdapter Task)
        {
            var mhResult = new MethodHandler();
            SqlCeConnection conSQL = null;
            SqlCeTransaction traSQL = null;
            try
            {

                //Verify if task with same name exists
                string strSQL = "SELECT TaskID FROM Task WHERE TaskName = @Name ";
                object objReturn = null;

                SqlCeParameter[] scpParams = new SqlCeParameter[1];
                scpParams[0] = new SqlCeParameter("Name", Task.TaskName);
                scpParams[0].DbType = DbType.String;

                mhResult = DBUtils.SelectValue(strSQL, scpParams, out objReturn);
                if (mhResult.Exits) return mhResult;
                if (mhResult.AffectedLines == 1 && (long)objReturn != Task.TaskId)
                {
                    mhResult.Status = MethodStatus.Cancel;
                    mhResult.Message = Languages.Language.TaskExists;
                    return mhResult;
                }


                strSQL = "UPDATE Task SET " +
                    "TaskName = @Name, " +
                    "Description = @Description  " +
                    "WHERE TaskID = " + Task.TaskId.ToString() + " ";

                scpParams = new SqlCeParameter[2];
                scpParams[0] = new SqlCeParameter("Name", Task.TaskName);
                scpParams[0].DbType = DbType.String;

                scpParams[1] = new SqlCeParameter("Description", Task.Description);
                scpParams[1].DbType = DbType.String;


                conSQL = DBUtils.OpenSqlConnection();
                traSQL = conSQL.BeginTransaction();

                mhResult = DBUtils.ExecuteOperation(strSQL, conSQL, traSQL, scpParams);
                if (mhResult.Exits) return mhResult;

                if (mhResult.AffectedLines == 0)
                {
                    mhResult.Status = MethodStatus.Cancel;
                    mhResult.Message = string.Format(Languages.Language.TaskNotFound, Task.TaskId);
                    return mhResult;
                }

                //Changing Times
                strSQL = "DELETE FROM TaskTime Where TaskID =" + Task.TaskId.ToString() + " ";
                mhResult = DBUtils.ExecuteOperation(strSQL, conSQL, traSQL);
                if (mhResult.Exits) return mhResult;

                strSQL = "INSERT INTO TaskTime (TaskId, StartTime, StopTime)  " +
                    "VALUES (" + Task.TaskId.ToString() + ", @StartTime, @StopTime)";

                foreach (TaskTimeAdapter t in Task.Times)
                {

                    t.StartTime = t.StartTime.AddMilliseconds((t.StartTime.Millisecond < 500) ? -t.StartTime.Millisecond : 1000 - t.StartTime.Millisecond);
                    t.StopTime = t.StopTime.AddMilliseconds((t.StopTime.Millisecond < 500) ? -t.StopTime.Millisecond : 1000 - t.StopTime.Millisecond);


                    scpParams = new SqlCeParameter[2];
                    t.TaskId = Task.TaskId;
                    scpParams[0] = new SqlCeParameter("StartTime", t.StartTime);
                    scpParams[0].DbType = DbType.DateTime;

                    scpParams[1] = new SqlCeParameter("StopTime", t.StopTime);
                    scpParams[1].DbType = DbType.DateTime;

                    mhResult = DBUtils.ExecuteOperation(strSQL, conSQL, traSQL, scpParams);
                    if (mhResult.Exits) return mhResult;
                }

                traSQL.Commit();
                traSQL = null;
                conSQL.Close();
                conSQL = null;

                StaticEvents.RaiseEventOnTaskUpdatedTask(Task);

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            finally
            {
                if (traSQL != null) traSQL.Rollback();
                if (conSQL != null) conSQL.Close();
            }
            return mhResult;
        }

        /// <summary>
        /// Changes the task to an Active/Inactive State.
        /// </summary>
        /// <param name="TaskID">The task ID.</param>
        /// <param name="Active">if set to <c>true</c> active else inactive.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler ActivateTask(long TaskID, bool Active)
        {
            var mhResult = new MethodHandler();
            try
            {

                string strSQL = "UPDATE Task SET " +
                    "Active = " + Active.ToDB() + " " +
                    "WHERE TaskID = " + TaskID.ToString() + " ";
                mhResult = DBUtils.ExecuteOperation(strSQL);

                if (mhResult.AffectedLines == 0)
                {
                    mhResult.Status = MethodStatus.Cancel;
                    mhResult.Message = string.Format(Languages.Language.TaskNotFound, TaskID);
                    return mhResult;
                }

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }

        /// <summary>
        /// Deletes the task.
        /// </summary>
        /// <param name="TaskID">The task ID.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler DeleteTask(long TaskID)
        {
            var mhResult = new MethodHandler();
            SqlCeConnection conSql = null;
            SqlCeTransaction traSql = null;
            try
            {

                conSql = DBUtils.OpenSqlConnection();
                traSql = conSql.BeginTransaction();

                string strSQL = "";
                strSQL = "UPDATE Shortcut SET TaskID = null WHERE TaskID = " + TaskID.ToString() + " ";
                mhResult = DBUtils.ExecuteOperation(strSQL, conSql, traSql);

                strSQL = "DELETE FROM TaskTime WHERE TaskID = " + TaskID.ToString() + " ";
                mhResult = DBUtils.ExecuteOperation(strSQL, conSql, traSql);

                strSQL = "DELETE FROM Task WHERE TaskID = " + TaskID.ToString() + " ";
                mhResult = DBUtils.ExecuteOperation(strSQL, conSql, traSql);

                if (mhResult.AffectedLines == 0)
                {
                    mhResult.Status = MethodStatus.Cancel;
                    mhResult.Message = string.Format(Languages.Language.TaskNotFound, TaskID);
                    return mhResult;
                }

                traSql.Commit();
                traSql = null;
                conSql.Close();
                conSql = null;

                StaticEvents.RaiseEventOnTaskDeleted(TaskID);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            finally
            {
                if (traSql != null)
                    traSql.Rollback();
                if (conSql != null)
                    conSql.Close();
            }
            return mhResult;
        }

        /// <summary>
        /// Reads the task list.
        /// </summary>
        /// <param name="TaskList">The task list.</param>
        /// <param name="ReadInactiveTasks">if set to <c>true</c> will read inactive tasks.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler ReadTaskList(out ObservableCollection<TaskAdapter> TaskList, bool ReadInactiveTasks)
        {
            var mhResult = new MethodHandler();
            TaskList = new ObservableCollection<TaskAdapter>();
            try
            {

                DataTable dtTasks = null;
                var strQuery = "SELECT T.TaskID, T.TaskName, T.Description, T.Active, " +
                    "CAST(COALESCE(TDAY.Time,0) AS BIGINT) [TodayTime], " +
                    "CAST(COALESCE(TTOT.Time,0) AS BIGINT) [TotalTime], TTOT.StopTime " +
                    "FROM Task T " +
                    "LEFT JOIN (SELECT TT.TaskID, SUM(DATEDIFF(S,TT.StartTime,TT.StopTime)) Time " +
                    "	FROM TaskTime TT " +
                    "	WHERE CONVERT(nvarchar(10), TT.StartTime,102) = CONVERT(nvarchar(10), GETDATE(),102) " +
                    "	GROUP BY TT.TaskID " +
                    ") AS TDAY ON TDAY.TaskID = T.TaskID " +
                    "LEFT JOIN (SELECT TT.TaskID, MAX(TT.StopTime) StopTime, SUM(DATEDIFF(S,TT.StartTime,TT.StopTime)) Time " +
                    "	FROM TaskTime TT " +
                    "	GROUP BY TT.TaskID " +
                    ") AS TTOT ON TTOT.TaskID = T.TaskID ";
                //var strQuery = "SELECT T.TaskId, MAX(T.TaskName) [TaskName], " +
                //                    "MAX(T.Description) Description, T.Active, " +
                //                    "(SELECT CAST(COALESCE(SUM(DATEDIFF(S,TT.StartTime,TT.StopTime)),0) AS BIGINT)) FROM TaskTime WHERE TaskID = T.TaskID )  TotalTime, " +
                //                    "CAST(COALESCE(SUM(DATEDIFF(S,TToday.StartTime,TToday.StopTime)),0) AS BIGINT) TodayTime " +
                //                    "FROM Task T " +
                //                    "LEFT JOIN TaskTime TToday ON TToday.TaskID = T.TaskID AND CONVERT(nvarchar(10), TToday.StartTime,102) = CONVERT(nvarchar(10), GETDATE(),102) ";
                if (!ReadInactiveTasks) strQuery += "WHERE T.Active = 1 ";
                //strQuery += "GROUP BY T.TaskID, T.Active ";
                strQuery += "ORDER BY T.Active DESC, TTOT.StopTime DESC ";

                mhResult = DBUtils.SelectTable(strQuery, out dtTasks);
                if (mhResult.Exits) return mhResult;

                foreach (DataRow dr in dtTasks.Rows)
                {

                    TaskList.Add(new TaskAdapter()
                    {
                        TaskId = (long)dr["TaskId"],
                        TaskName = (string)dr["TaskName"],
                        Description = (string)dr["Description"],
                        TotalTime = (long)dr["TotalTime"],
                        TodayTime = (long)dr["TodayTime"],
                        Active = (bool)dr["Active"],
                        StopTime = (dr["StopTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["StopTime"],
                        Times = new ObservableCollection<TaskTimeAdapter>()
                    });
                }
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }

        /// <summary>
        /// Stops the task.
        /// </summary>
        /// <param name="Time">The time.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler AddTime(TaskTimeAdapter Time)
        {
            var mhResult = new MethodHandler();
            try
            {
                object objTaskExists = null;
                string strSQL = "SELECT 1 FROM Task WHERE TaskID = " + Time.TaskId.ToString() + " ";

                mhResult = DBUtils.SelectValue(strSQL, out objTaskExists);
                if (mhResult.Exits) return mhResult;

                if (mhResult.AffectedLines == 0)
                {
                    mhResult.Status = MethodStatus.Cancel;
                    mhResult.Message = string.Format(Languages.Language.TaskNotFound, Time.TaskId);
                    return mhResult;
                }


                Time.StartTime = Time.StartTime.AddMilliseconds((Time.StartTime.Millisecond < 500) ? -Time.StartTime.Millisecond : 1000 - Time.StartTime.Millisecond);
                Time.StopTime = Time.StopTime.AddMilliseconds((Time.StopTime.Millisecond < 500) ? -Time.StopTime.Millisecond : 1000 - Time.StopTime.Millisecond);

                strSQL = "INSERT INTO TaskTime (TaskId, StartTime, StopTime)  " +
                    "VALUES (" + Time.TaskId.ToString() + ", @StartTime, @StopTime)";
                SqlCeParameter[] scpParams = new SqlCeParameter[2];
                scpParams[0] = new SqlCeParameter("StartTime", Time.StartTime);
                scpParams[0].DbType = DbType.DateTime;

                scpParams[1] = new SqlCeParameter("StopTime", Time.StopTime);
                scpParams[1].DbType = DbType.DateTime;

                mhResult = DBUtils.ExecuteOperation(strSQL, scpParams);
                if (mhResult.Exits) return mhResult;

                object objTimeID = null;
                strSQL = "SELECT MAX(TimeId) FROM TaskTime ";
                mhResult = DBUtils.SelectValue(strSQL, out objTimeID);
                if (mhResult.Exits) return mhResult;

                Time.TimeId = (long)objTimeID;

                StaticEvents.RaiseEventOnTimeAdded(Time.TaskId);
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }

    }
}
