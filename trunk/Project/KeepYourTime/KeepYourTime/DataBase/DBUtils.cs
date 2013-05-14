using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.DataBase
{
    /// <summary>
    /// Database Utils
    /// </summary>
    /// <remarks>
    /// CREATED BY Rui Ganhoto
    /// </remarks>
    class DBUtils
    {

        public const string FileName = "db.sdf";

        public const string Password = "ZK8setbx";


        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return string.Format("DataSource=\"{0}\"; Password='{1}'", FileName, Password);
        }


        /// <summary>
        /// Opens the SQL connection.
        /// </summary>
        /// <returns>Sql Connection</returns>
        public static SqlCeConnection OpenSqlConnection()
        {
            var conn = new SqlCeConnection(GetConnectionString());
            conn.Open();
            return conn;
        }


        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <returns></returns>
        public static MethodHandler ExecuteOperation(string SqlQuery)
        {
            MethodHandler mhResult = new MethodHandler();
            try
            {
                var conn = OpenSqlConnection();
                mhResult = ExecuteOperation(SqlQuery, conn, null);
                conn.Close();
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex, SqlQuery);
            }
            return mhResult;
        }


        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
        public static MethodHandler ExecuteOperation(string SqlQuery, SqlCeParameter[]  Parameters)
        {
            MethodHandler mhResult = new MethodHandler();
            try
            {
                var conn = OpenSqlConnection();
                mhResult = ExecuteOperation(SqlQuery, conn, null,Parameters);
                conn.Close();
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex, SqlQuery);
            }
            return mhResult;
        }


        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public static MethodHandler ExecuteOperation(string SqlQuery, SqlCeConnection connection, SqlCeTransaction transaction)
        {
            MethodHandler mhResult = new MethodHandler();
            try
            {
                var cmd = new SqlCeCommand(SqlQuery, connection);
                if (transaction != null)
                    cmd.Transaction = transaction;
                mhResult.AffectedLines = cmd.ExecuteNonQuery(); ;
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex, SqlQuery);
            }
            return mhResult;
        }


        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
        public static MethodHandler ExecuteOperation(string SqlQuery, SqlCeConnection connection, SqlCeTransaction transaction, SqlCeParameter[] Parameters)
        {
            MethodHandler mhResult = new MethodHandler();
            try
            {
                var cmd = new SqlCeCommand(SqlQuery, connection);
                if (transaction != null)
                    cmd.Transaction = transaction;
                foreach (SqlCeParameter p in Parameters)
                    cmd.Parameters.Add(p);
                mhResult.AffectedLines = cmd.ExecuteNonQuery(); ;
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex, SqlQuery);
            }
            return mhResult;
        }


        /// <summary>
        /// Selects the table.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="ResultData">The result data.</param>
        /// <returns></returns>
        public static MethodHandler SelectTable(string SqlQuery, out DataTable ResultData)
        {
            MethodHandler mhResult = new MethodHandler();
            ResultData = null;
            try
            {
                var conn = OpenSqlConnection();
                mhResult = SelectTable(SqlQuery, conn, null, out ResultData);
                conn.Close();
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex, SqlQuery);
            }
            return mhResult;
        }


        /// <summary>
        /// Selects the table.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="Connection">The connection.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="ResultData">The result data.</param>
        /// <returns></returns>
        public static MethodHandler SelectTable(string SqlQuery, SqlCeConnection Connection, SqlCeTransaction Transaction, out DataTable ResultData)
        {
            MethodHandler mhResult = new MethodHandler();
            ResultData = null;
            try
            {
                
                var da = new SqlCeDataAdapter(SqlQuery, Connection);
                var dt = new DataTable();
                da.Fill(dt);
                ResultData = dt;
                mhResult.AffectedLines = dt.Rows.Count;
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex, SqlQuery);
            }
            return mhResult;
        }


        /// <summary>
        /// Selects the value.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="ResultObject">The result object.</param>
        /// <returns></returns>
        public static MethodHandler SelectValue(string SqlQuery, out object ResultObject)
        {
            MethodHandler mhResult = new MethodHandler();
            ResultObject = null;
            try
            {
                var conn = OpenSqlConnection();
                mhResult = SelectValue(SqlQuery, conn,null, null, out ResultObject);
                conn.Close();
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex, SqlQuery);
            }
            return mhResult;
        }


        /// <summary>
        /// Selects the value.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <param name="ResultObject">The result object.</param>
        /// <returns></returns>
        public static MethodHandler SelectValue(string SqlQuery, SqlCeParameter [] Parameters, out object ResultObject)
        {
            MethodHandler mhResult = new MethodHandler();
            ResultObject = null;
            try
            {
                var conn = OpenSqlConnection();
                mhResult = SelectValue(SqlQuery, conn, null, Parameters, out ResultObject);
                conn.Close();
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex, SqlQuery);
            }
            return mhResult;
        }


        /// <summary>
        /// Selects the value.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <param name="ResultObject">The result object.</param>
        /// <returns></returns>
        public static MethodHandler SelectValue(string SqlQuery, SqlCeConnection connection, SqlCeTransaction transaction, SqlCeParameter[] Parameters, out object ResultObject)
        {
            MethodHandler Result = new MethodHandler();
            ResultObject = null;
            try
            {
                var cmd = new SqlCeCommand(SqlQuery, connection);
                if (transaction != null)
                    cmd.Transaction = transaction;

                if (Parameters != null)
                    foreach (SqlCeParameter p in Parameters)
                        cmd.Parameters.Add(p);
                
                object objReturn = cmd.ExecuteScalar();
                ResultObject = objReturn;
            }
            catch (Exception ex)
            {
                Result.Exception(ex, SqlQuery);
            }
            return Result;
        }

    }
}
