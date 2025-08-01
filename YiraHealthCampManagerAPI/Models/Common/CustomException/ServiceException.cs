using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace YiraApi.Common.CustomException
{
    /// <summary>
    /// Custom Exception class extension method which will be and should be used for catching all the Service Exceptions in the Application.
    /// </summary>
    [Serializable]
    public class ServiceException : Exception
    {
        Logger log = LogManager.GetLogger("ELog");

        public ServiceException()
        {
        }

        public ServiceException(string name)
         : base(String.Format(": {0}", name))
        {

        }

        public ServiceException(string serviceName, Exception exception)
            : base($"ServiceException in {serviceName}: {exception.Message}", exception)
        {
            log.Trace($"Service Exception is hit in: {serviceName}");
            log.Error(exception, serviceName);
        }
    }
}
