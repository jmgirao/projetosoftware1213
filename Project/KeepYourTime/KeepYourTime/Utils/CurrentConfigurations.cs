using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
