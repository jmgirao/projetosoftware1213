using System;
using System.Data;
using System.Data.SqlServerCe;

namespace KeepYourTime.DataBase
{
    /// <summary>
    /// Database Utils
    /// </summary>
    /// <remarks>
    /// CREATED BY Rui Ganhoto
    /// </remarks>
    public class DBUtils
    {
        //The database filename
        public const string FileName = "db.sdf";

        //The database password
        public const string Password = "ZK8setbx";


        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns>The Connection String</returns>
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
        /// <returns>Method Handler with the method status</returns>
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
        /// <param name="Parameters">The parameters. Parameter List to include in query.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler ExecuteOperation(string SqlQuery, SqlCeParameter[] Parameters)
        {
            MethodHandler mhResult = new MethodHandler();
            try
            {
                var conn = OpenSqlConnection();
                mhResult = ExecuteOperation(SqlQuery, conn, null, Parameters);
                conn.Close();
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex, SqlQuery);
            }
            return mhResult;
        }


        /// <summary>
        /// Executes an operation.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="Connection">The connection.</param>
        /// <param name="Transaction">The transaction. If <c>null</c> works without transaction</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler ExecuteOperation(string SqlQuery, SqlCeConnection Connection, SqlCeTransaction Transaction)
        {
            MethodHandler mhResult = new MethodHandler();
            try
            {
                var cmd = new SqlCeCommand(SqlQuery, Connection);
                if (Transaction != null)
                    cmd.Transaction = Transaction;
                mhResult.AffectedLines = cmd.ExecuteNonQuery();
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
        /// <param name="Connection">The connection.</param>
        /// <param name="Transaction">The transaction. If <c>null</c> works without transaction</param>
        /// <param name="Parameters">The parameters. Parameter List to include in query.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler ExecuteOperation(string SqlQuery, SqlCeConnection Connection, SqlCeTransaction Transaction, SqlCeParameter[] Parameters)
        {
            MethodHandler mhResult = new MethodHandler();
            try
            {
                var cmd = new SqlCeCommand(SqlQuery, Connection);
                if (Transaction != null)
                    cmd.Transaction = Transaction;
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
        /// <returns>Method Handler with the method status</returns>
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
        /// <param name="Transaction">The transaction. If <c>null</c> works without transaction</param>
        /// <param name="ResultData">The result data.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler SelectTable(string SqlQuery, SqlCeConnection Connection, SqlCeTransaction Transaction, out DataTable ResultData)
        {
            MethodHandler mhResult = new MethodHandler();
            ResultData = null;
            try
            {

                var da = new SqlCeDataAdapter(SqlQuery, Connection);
                if (Transaction != null)
                    da.SelectCommand.Transaction = Transaction;
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
        /// Selects the value of the first column on first row in the select.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="ResultObject">The result object.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler SelectValue(string SqlQuery, out object ResultObject)
        {
            MethodHandler mhResult = new MethodHandler();
            ResultObject = null;
            try
            {
                var conn = OpenSqlConnection();
                mhResult = SelectValue(SqlQuery, conn, null, null, out ResultObject);
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
        /// <param name="Parameters">The parameters. Parameter List to include in query.</param>
        /// <param name="ResultObject">The result object.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler SelectValue(string SqlQuery, SqlCeParameter[] Parameters, out object ResultObject)
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
        /// Selects the value of the first column on first row in the select.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="Connection">The connection.</param>
        /// <param name="Transaction">The transaction. If <c>null</c> works without transaction</param>
        /// <param name="Parameters">The parameters. Parameter List to include in query.</param>
        /// <param name="ResultObject">The result object.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler SelectValue(string SqlQuery, SqlCeConnection Connection, SqlCeTransaction Transaction, SqlCeParameter[] Parameters, out object ResultObject)
        {
            MethodHandler Result = new MethodHandler();
            ResultObject = null;
            try
            {
                var cmd = new SqlCeCommand(SqlQuery, Connection);
                if (Transaction != null)
                    cmd.Transaction = Transaction;

                if (Parameters != null)
                    foreach (SqlCeParameter p in Parameters)
                        cmd.Parameters.Add(p);

                ResultObject = cmd.ExecuteScalar();
                Result.AffectedLines = (ResultObject == null) ? 0 : 1;
            }
            catch (Exception ex)
            {
                Result.Exception(ex, SqlQuery);
            }
            return Result;
        }

    }
}
