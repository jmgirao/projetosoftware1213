using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeepYourTime.Utils;

namespace KeepYourTime
{

    /// <summary>
    /// Method Handler to Exception and Status Treatment
    /// </summary>
    /// <remarks>
    /// CREATED BY Rui Ganhoto
    /// </remarks>
    public class MethodHandler
    {
        public string Message { get; set; }
        public MethodStatus Status { get; set; }
        public string StackTrace { get; set; }
        public string Query { get; set; }
        public int AffectedLines { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodHandler"/> class.
        /// </summary>
        public MethodHandler()
        {
            Message = "";
            Status = MethodStatus.Sucess;
            StackTrace = "";
            Query = "";
            AffectedLines = 0;
        }

        /// <summary>
        /// Changes Status to Exception.
        /// </summary>
        /// <param name="Ex">The Exception</param>
        public void Exception(Exception Ex)
        {
            Status = MethodStatus.Exception;
            Message = Ex.Message;
            StackTrace = Ex.StackTrace;
        }

        /// <summary>
        /// Changes MethodHandler Status to Exception with a query.
        /// </summary>
        /// <param name="Ex">The Exception.</param>
        /// <param name="Query">The query.</param>
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
