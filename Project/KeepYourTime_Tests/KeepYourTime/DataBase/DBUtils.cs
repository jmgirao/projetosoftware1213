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
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    class DBUtils
    {

        public const string FileName = "db.sdf";

        public const string Password = "ZK8setbx";

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns>The connection String</returns>
        public static string GetConnectionString()
        {
            return string.Format("DataSource=\"{0}\"; Password='{1}'", FileName, Password);
        }

        /// <summary>
        /// Opens the SQL connection.
        /// </summary>
        /// <returns></returns>
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
        public static int ExecuteOperation(string SqlQuery)
        {
            var conn = OpenSqlConnection();
            int intReturn = ExecuteOperation(SqlQuery, conn, null);
            conn.Close();
            return intReturn;
        }

        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public static int ExecuteOperation(string SqlQuery, SqlCeConnection connection, SqlCeTransaction transaction)
        {
            var cmd = new SqlCeCommand(SqlQuery, connection);
            if (transaction != null)
                cmd.Transaction = transaction;
            int intAffectedLines = cmd.ExecuteNonQuery();
            return intAffectedLines;
        }

        /// <summary>
        /// Selects the table.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <returns></returns>
        public static DataTable SelectTable(string SqlQuery)
        {
            var conn = OpenSqlConnection();
            DataTable dtReturn = SelectTable(SqlQuery, conn, null);
            conn.Close();
            return dtReturn;
        }

        /// <summary>
        /// Selects the table.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public static DataTable SelectTable(string SqlQuery, SqlCeConnection connection, SqlCeTransaction transaction)
        {

            var da = new SqlCeDataAdapter(SqlQuery, connection);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Selects the value.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <returns></returns>
        public static object SelectValue(string SqlQuery)
        {
            var conn = OpenSqlConnection();
            object objReturn = SelectValue(SqlQuery, conn, null);
            conn.Close();
            return objReturn;
        }

        /// <summary>
        /// Selects the value.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public static object SelectValue(string SqlQuery, SqlCeConnection connection, SqlCeTransaction transaction)
        {
            var cmd = new SqlCeCommand(SqlQuery, connection);
            if (transaction != null)
                cmd.Transaction = transaction;
            object objReturn = cmd.ExecuteScalar();
            return objReturn;
        }





    }
}
