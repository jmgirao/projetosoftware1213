using KeepYourTime.DataBase.Adapters;
using System;
using System.Collections.Generic;
using System.Data;
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

        public MethodHandler ReadTask(int TaskID, out TaskAdapter Task)
        {
            var mhResult = new MethodHandler();
            Task = null;
            try
            {
                DataTable dtTask = null;
                string strQuery = "SELECT * FROM Task WHERE Task = " + TaskID + " ";
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
                    Active = (bool)dtTask.Rows[0]["Active"],
                    Description = (string)dtTask.Rows[0]["Description"],
                    Name = (string)dtTask.Rows[0]["Name"]
                };
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }


        public MethodHandler CreateTask()
        {
            var mhResult = new MethodHandler();
            try
            {

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }


        public MethodHandler EditTask()
        {
            var mhResult = new MethodHandler();
            try
            {

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }

        public MethodHandler ReadTaskList()
        {
            var mhResult = new MethodHandler();
            try
            {

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }

        public MethodHandler StopTask()
        {
            var mhResult = new MethodHandler();
            try
            {

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }

        public MethodHandler AddIactiveTime()
        {
            var mhResult = new MethodHandler();
            try
            {

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }

        public MethodHandler IgnoreInactiveTime()
        {
            var mhResult = new MethodHandler();
            try
            {

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }
    }
}
