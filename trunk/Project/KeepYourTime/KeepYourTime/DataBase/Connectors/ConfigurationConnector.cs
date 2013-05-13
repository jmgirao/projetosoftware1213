using KeepYourTime.DataBase.Adapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.DataBase.Connectors
{
    class ConfigurationConnector
    {

        public MethodHandler ReadConfiguration(out ConfigurationAdapter Configuration)
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
                    InactivityTime = (int)dtConfigs.Rows[0]["InactivityTime"],
                    Inactivity = (bool)dtConfigs.Rows[0]["Inactivity"],
                    Shortcuts = new List<ShortcutAdapter>()
                };

                strQuery = "SELECT * FROM Shortcuts ORDER BY ShortcutId ";
                mhResult = DBUtils.SelectTable(strQuery, out dtConfigs);
                if (mhResult.Exits) return mhResult;

                foreach (DataRow dr in dtConfigs.Rows)
                {
                    Configuration.Shortcuts.Add(new ShortcutAdapter
                    {
                        Alt = (bool)dr["Alt"],
                        Ctrl = (bool)dr["Ctrl"],
                        Key = (char)dr["Key"],
                        Shift = (bool)dr["Shift"],
                        ShortcutId = (int)dr["ShortcutId"],
                        TaskId = (int)dr["TaskId"]
                    });
                }

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

        public MethodHandler SaveConfiguration(ConfigurationAdapter Configuration)
        {
            var mhResult = new MethodHandler();
            try
            {
                string strSql = "DELETE FORM Configuration ";
                //INSERT

                strSql = "INSERT INTO Configuration (Inactivity, InactivityTime) VALUES (" +
                    Configuration.Inactivity.ToDB() + ", " +
                    Configuration.InactivityTime + " " +
                    ") ";



                strSql = "DELETE FROM Shortcuts ";
                //INSERTS à bruta!

                foreach (ShortcutAdapter saShort in Configuration.Shortcuts)
                {
                    strSql = "INSERT INTO Shortcut (ShortcutId, Ctrl, Alt, Shift, Key, TaskId) VALUES (" +
                        " " + saShort.ShortcutId + ", " +
                        " " + saShort.Ctrl.ToDB() + ", " +
                        " " + saShort.Alt.ToDB() + ", " +
                        " " + saShort.Shift.ToDB() + ", " +
                        "'" + saShort.Key.ToString() + "'," +
                        " " + saShort.TaskId + ", " +
                        ") ";
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
