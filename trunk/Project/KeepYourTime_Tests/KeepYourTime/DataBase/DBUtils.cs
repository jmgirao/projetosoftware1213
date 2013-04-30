using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.DataBase
{
    class DBUtils
    {

        public const string FileName = "db.sdf";

        public const string Password = "ZK8setbx";

        public static string GetConnectionString()
        {
            return string.Format("DataSource=\"{0}\"; Password='{1}'", FileName, Password);
        }

        public static SqlCeConnection OpenSqlConnection()
        {
            var conn = new SqlCeConnection(GetConnectionString());
            conn.Open();
            return conn;
        }

        public static int ExecuteOperation(string SqlQuery)
        {
            var conn = OpenSqlConnection();
            int intReturn = ExecuteOperation(SqlQuery, conn, null);
            conn.Close();
            return intReturn;
        }

        public static int ExecuteOperation(string SqlQuery, SqlCeConnection connection, SqlCeTransaction transaction)
        {
            var cmd = new SqlCeCommand(SqlQuery, connection);
            if (transaction != null)
                cmd.Transaction = transaction;
            int intAffectedLines = cmd.ExecuteNonQuery();
            return intAffectedLines;
        }

        public static DataTable SelectTable(string SqlQuery)
        {
            var conn = OpenSqlConnection();
            DataTable dtReturn = SelectTable(SqlQuery, conn, null);
            conn.Close();
            return dtReturn;
        }

        public static DataTable SelectTable(string SqlQuery, SqlCeConnection connection, SqlCeTransaction transaction)
        {

            var da = new SqlCeDataAdapter(SqlQuery, connection);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static object SelectValue(string SqlQuery)
        {
            var conn = OpenSqlConnection();
            object objReturn = SelectValue(SqlQuery, conn, null);
            conn.Close();
            return objReturn;
        }

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
