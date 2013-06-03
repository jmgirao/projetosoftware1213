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
    /// Handles The Configuration Connection with the Database
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public class ConfigurationConnector
    {
        public const string COLUMN_INACTIVITY_ACTIVE = "Inactivity";
        public const string COLUMN_INACTIVITY_TIME = "InactivityTime";

        public const string COLUMN_SHORTCUT_SHIFT = "Shift";
        public const string COLUMN_SHORTCUT_ALT = "Alt";
        public const string COLUMN_SHORTCUT_CTRL = "Ctrl";
        public const string COLUMN_SHORTCUT_KEY = "ShortcutKey";
        public const string COLUMN_SHORTCUT_ID = "ShortcutId";
        public const string COLUMN_SHORTCUT_TASK_ID = "TaskId";

        /// <summary>
        /// Reads the configuration.
        /// </summary>
        /// <param name="Configuration">The configuration.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler ReadConfiguration(out ConfigurationAdapter Configuration)
        {
            var mhResult = new MethodHandler();
            Configuration = null;
            try
            {
                DataTable dtConfigs = null;
                string strQuery = "SELECT * FROM Configuration ";
                mhResult = DBUtils.SelectTable(strQuery, out dtConfigs);
                if (mhResult.Exits) return mhResult;

                if (mhResult.AffectedLines == 0)
                {
                    Configuration = new ConfigurationAdapter();
                    Configuration.Shortcuts = new List<ShortcutAdapter>();
                    Configuration.Shortcuts.Add(new ShortcutAdapter());
                    Configuration.Shortcuts.Add(new ShortcutAdapter());
                    Configuration.Shortcuts.Add(new ShortcutAdapter());
                    Configuration.Shortcuts.Add(new ShortcutAdapter());
                    Configuration.Shortcuts.Add(new ShortcutAdapter());
                    return mhResult;
                }

                Configuration = new ConfigurationAdapter
                {

                    InactivityTime = Convert.ToInt32((byte)dtConfigs.Rows[0]["InactivityTime"]),
                    Inactivity = (bool)dtConfigs.Rows[0]["InactivityEnabled"],
                    Shortcuts = new List<ShortcutAdapter>()
                };

                strQuery = "SELECT ShortcutId, COALESCE(TaskId,0) TaskId, Shift, ShortcutKey, Ctrl, Alt  FROM Shortcut ORDER BY ShortcutId ";
                mhResult = DBUtils.SelectTable(strQuery, out dtConfigs);
                if (mhResult.Exits) return mhResult;

                foreach (DataRow dr in dtConfigs.Rows)
                {
                    Configuration.Shortcuts.Add(new ShortcutAdapter
                    {
                        Alt = (bool)dr["Alt"],
                        Ctrl = (bool)dr["Ctrl"],
                        ShortcutKey = (string)dr["ShortcutKey"],
                        Shift = (bool)dr["Shift"],
                        ShortcutId = Configuration.Shortcuts.Count + 1,
                        TaskId = (long)dr["TaskId"]
                    });
                }

                //this runs if there is less than 5 shortcuts in the database
                while (Configuration.Shortcuts.Count < 5)
                {
                    Configuration.Shortcuts.Add(new ShortcutAdapter()
                    {
                        ShortcutId = Configuration.Shortcuts.Count + 1
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
        /// Saves the configuration.
        /// </summary>
        /// <param name="Configuration">The configuration.</param>
        /// <returns>Method Handler with the method status</returns>
        public static MethodHandler SaveConfiguration(ConfigurationAdapter Configuration)
        {
            var mhResult = new MethodHandler();
            try
            {
                string strSql = "";
                strSql = "DELETE FROM Configuration ";
                mhResult = DBUtils.ExecuteOperation(strSql);
                if (mhResult.Exits) return mhResult;

                strSql = "INSERT INTO Configuration (InactivityEnabled, InactivityTime) VALUES (" +
                    Configuration.Inactivity.ToDB() + ", " +
                    Configuration.InactivityTime + " " +
                    ") ";
                mhResult = DBUtils.ExecuteOperation(strSql);
                if (mhResult.Exits) return mhResult;


                strSql = "DELETE FROM Shortcut ";
                mhResult = DBUtils.ExecuteOperation(strSql);
                if (mhResult.Exits) return mhResult;

                foreach (ShortcutAdapter saShort in Configuration.Shortcuts)
                {
                    strSql = "INSERT INTO Shortcut (ShortcutId, Ctrl, Alt, Shift, ShortcutKey, TaskId) VALUES (" +
                        " " + saShort.ShortcutId + ", " +
                        " " + saShort.Ctrl.ToDB() + ", " +
                        " " + saShort.Alt.ToDB() + ", " +
                        " " + saShort.Shift.ToDB() + ", " +
                        "'" + saShort.ShortcutKey.ToString() + "'," +
                        " " + saShort.TaskId + " " +
                        ") ";
                    mhResult = DBUtils.ExecuteOperation(strSql);
                    if (mhResult.Exits) return mhResult;

                }

            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;
        }

    }
}
