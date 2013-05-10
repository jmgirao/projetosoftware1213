using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.DataBase
{
    public static class DBExtentionMethods
    {
        public static string ToDB(this bool BooleanValue)
        {
            if (BooleanValue) return "1";
            else return "0";
        }
    }

}
