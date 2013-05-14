using KeepYourTime.DataBase.Adapters;
using System;
using System.Collections.Generic;
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
    class TaskConnector
    {

        /// <summary>
        /// Reads the task.
        /// </summary>
        /// <param name="TaskID">The task ID.</param>
        /// <param name="Task">The task.</param>
        /// <returns></returns>
        public MethodHandler ReadTask(int TaskID, out TaskAdapter Task)
        {
            var mhResult = new MethodHandler();
            Task = null;
            try
            {
                DataTable dtTask = null;
                string strQuery = "SELECT TaskId, Name, Description, Active FROM Task WHERE Task = " + TaskID.ToString() + " ";
                mhResult = DBUtils.SelectTable(strQuery, out dtTask);
                if (mhResult.Exits) return mhResult;

                if (mhResult.AffectedLines == 0)
                {
                    mhResult.Status = Utils.MethodStatus.Cancel;
                    mhResult.Message = String.Format("Task {0} not found", TaskID);
                    return mhResult;
                }

                Task = new TaskAdapter()
                {
                    TaskId = (int)dtTask.Rows[0]["TaskId"],
                    Name = (string)dtTask.Rows[0]["Name"],
                    Description = (string)dtTask.Rows[0]["Description"],
                    Active = (bool)dtTask.Rows[0]["Active"]
                };


                //TODO: Read Times
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
        /// <returns></returns>
        public MethodHandler CreateTask(string Name, out int TaskID)
        {
            var mhResult = new MethodHandler();
            TaskID = 0;
            try
            {
                string strSQL = "SELECT 1 FROM Task WHERE Name = @Name ";


                //TODO: INSERT Task
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
        /// <returns></returns>
        public MethodHandler EditTask(TaskAdapter Task)
        {
            var mhResult = new MethodHandler();
            try
            {

                //TODO: UPDATE Task
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
        /// <returns></returns>
        public MethodHandler DeleteTask(int TaskID)
        {
            var mhResult = new MethodHandler();
            try
            {
                //TODO:Delete TASK
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }


        /// <summary>
        /// Reads the task list.
        /// </summary>
        /// <param name="TaskList">The task list.</param>
        /// <param name="ReadInactiveTasks">if set to <c>true</c> will read inactive tasks.</param>
        /// <returns></returns>
        public MethodHandler ReadTaskList(out List<TaskAdapter> TaskList, bool ReadInactiveTasks)
        {
            var mhResult = new MethodHandler();
            TaskList = new List<TaskAdapter>();
            try
            {

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
        /// <returns></returns>
        public MethodHandler AddTime(TaskTimeAdapter Time)
        {
            var mhResult = new MethodHandler();
            try
            {
                object objTaskExists = null;
                string strSQL = "SELECT 1 FROM Task WHERE TaskID = " + Time.TaskId.ToString() + " ";

                mhResult = DBUtils.SelectValue(strSQL, out objTaskExists);
                if (mhResult.Exits) return mhResult;

                if (objTaskExists.ToString() != "1")
                {
                    mhResult.Status = Utils.MethodStatus.Cancel;
                    mhResult.Message = string.Format("The task {0} is invalid", Time.TaskId);
                    return mhResult;
                }

                strSQL = "INSERT INTO TaskTime (TaskId, StartTime, StopTime)  " +
                    "VALUES (" + Time.TaskId.ToString() + ", @StartTime, @StopTime)";
                SqlCeParameter[] ParamArray = new SqlCeParameter[2];
                ParamArray[0] = new SqlCeParameter("StartTime", Time.Start);
                ParamArray[0].DbType = DbType.DateTime;

                ParamArray[1] = new SqlCeParameter("StopTime", Time.End);
                ParamArray[1].DbType = DbType.DateTime;

                mhResult = DBUtils.ExecuteOperation(strSQL, ParamArray);
                if (mhResult.Exits) return mhResult;

                object objTimeID = null;
                strSQL = "SELECT IDENT_CURRENT('TaskTime')";
                mhResult = DBUtils.SelectValue(strSQL, out objTimeID);
                if (mhResult.Exits) return mhResult;

                Time.TimeId = (int)objTimeID;

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }


    }
}
