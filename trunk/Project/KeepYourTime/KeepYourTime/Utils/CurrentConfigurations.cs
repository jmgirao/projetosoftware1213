using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeepYourTime.Utils
{
    class CurrentConfigurations
    {

        public static ConfigurationAdapter allConfig;

        public static MethodHandler getConfigurations()
        {
            var mhResult = new MethodHandler();
            allConfig = new ConfigurationAdapter();

            try
            {
                mhResult = ConfigurationConnector.ReadConfiguration(out allConfig);
                if (mhResult.Exits) return mhResult;
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
            }
            return mhResult;

        }

        public static MethodHandler getConfigurations(ConfigurationAdapter caConfig)
        {
            var mhResult = new MethodHandler();

            try
            {
                if (caConfig == null)
                    throw new NullReferenceException();

                allConfig = caConfig;
                mhResult.Status = MethodStatus.Sucess;
                return mhResult;
            }
            catch (Exception ex)
            {
                mhResult.Exception(ex);
                return mhResult;

            }



        }


        private static List<Hooks.Hotkey> lstHotKeys = new List<Hooks.Hotkey>();

        public static KeepYourTime.ViewWindows.MainWindow mw;

        public static void ConfigureHotKeys()
        {

            while (lstHotKeys.Count > 0)
            {
                lstHotKeys.First().Unregister();
                lstHotKeys.RemoveAt(0);
            }


            foreach (ShortcutAdapter sc in allConfig.Shortcuts)
            {
                if ((sc.ShortcutKey != ""))
                {
                    var hkHotKey = new Hotkey(GetKeyByString(sc.ShortcutKey), sc.Shift, sc.Ctrl, sc.Alt, false);
                    hkHotKey.TaskID = sc.TaskId;

                    hkHotKey.Pressed += hkHotKey_Pressed;



                    hkHotKey.Register(mw);
                }

            }
        }

        static void hkHotKey_Pressed(object sender, System.ComponentModel.HandledEventArgs e)
        {
            //TODO: Start Task
            long TaskID = (sender as Hotkey).TaskID;

            //TODO: IF TASK NOT AVAILABLE MAKE SOUND

            e.Handled = true;
        }

        static private Keys GetKeyByString(string c)
        {

            if (c.Length == 0)
                return Keys.None;
            c = c.ToUpper();
            return (Keys)c[0];
        }

    }
}
