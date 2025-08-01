using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YiraApi.Common.CustomException
{
    /// <summary>
    /// Custom Exception class extension method which will be and should be used for catching all the Database Exceptions in the Application.
    /// </summary>
    [Serializable]
    class YiraDbContextException : Exception
    {
        Logger log = LogManager.GetLogger("ELog");

        public YiraDbContextException()
        {

        }

        public YiraDbContextException(string tableName, Exception exception)
            : base(String.Format(tableName + exception))
        {
            log.Trace(String.Format("Database Exception is hit in {0}", tableName));
            log.Error(exception, tableName);
        }
    }
}
