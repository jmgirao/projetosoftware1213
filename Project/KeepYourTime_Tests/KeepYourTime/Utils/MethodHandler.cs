using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeepYourTime.Utils;

namespace KeepYourTime
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    class MethodHandler
    {
        public string Message { get; set; }
        public MethodStatus Status { get; set; }
        public string StackTrace { get; set; }
        public string Query { get; set; }
        public int AffectedLines { get; set; }

        public MethodHandler()
        {
            Message = "";
            Status = MethodStatus.Sucess;
            StackTrace = "";
            Query = "";
            AffectedLines = 0;
        }


        public void Exception(Exception Ex)
        {
            Status = MethodStatus.Exception;
            Message = Ex.Message;
            StackTrace = Ex.StackTrace;
        }


        public void Exception(Exception Ex, string Query)
        {
            Exception(Ex);
            this.Query = Query;
        }

        public bool Exits
        {
            get
            { return (Status != MethodStatus.Sucess); }
        }
    }
}
