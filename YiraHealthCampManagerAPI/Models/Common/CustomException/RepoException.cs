using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YiraApi.Common.CustomException
{
    /// <summary>
    /// Custom Exception class extension method which will be and should be used for catching all the Repository Exceptions in the Application.
    /// </summary>
    [Serializable]
    class RepoException : Exception
    {
        Logger log = LogManager.GetLogger("ELog");

        public RepoException()
        {

        }

        public RepoException(string name)
        : base(String.Format("Dashboard Repository: {0}", name))
        {

        }

        public RepoException(string repoName, Exception exception)
            : base(String.Format(repoName + exception))
        {
            log.Trace(String.Format("New Exception got hit in Repository {0}", repoName));
            log.Error(exception, repoName);
        }
    }
}
