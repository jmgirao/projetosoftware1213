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
    class CreateDB
    {
        public static bool IsDbCreated()
        {
            return (File.Exists(DBUtils.FileName));
        }

        public void CreateDatabase()
        {
            string connectionString = DBUtils.GetConnectionString();
            SqlCeEngine en = new SqlCeEngine(connectionString);
            en.CreateDatabase();

            CreateTableTarefa();
            CreateTableTempo();

        }

        private void CreateTableTask()
        {
            string strSQL = "CREATE TABLE Task( " + 
                "ID BIGINT IDENTITY NOT NULL, " +
                "" +
                "" +
                "" +
                "" +
                "CONSTRAINT PK_Task PRIMARY KEY (ID) " +
                ")";

            DBUtils.ExecuteOperation(strSQL);
        }

        private void CreateTableTime()
        {
            string strSQL = "CREATE TABLE Time( " +
                "ID BIGINT IDENTITY NOT NULL, " +
                "IDTarefa BIGINT NOT NULL, " +
                "" +
                "" +
                "" +
                "" +
                "CONSTRAINT PK_Time PRIMARY KEY (ID) " +
                ")";

            DBUtils.ExecuteOperation(strSQL);
        }

        private void CreateTableDefinitions()
        { }

        private void CreateForeignKeys()
        {
            var strSQL = "";

        }

        private void InsertValues()
        { }

    }
}
