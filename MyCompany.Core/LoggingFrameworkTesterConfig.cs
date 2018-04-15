using System;
using System.Configuration;

namespace MyCompany.Core
{
    public class LoggingFrameworkTesterConfig : ILoggingFrameworkTesterConfig
    {
        public string ApplicationName { get { return ConfigurationManager.AppSettings["ApplicationName"]; } }

        public bool LogToSeqServer { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["Logging.LogToSeqServer"] ?? bool.FalseString); } }

        public bool LogToEventLog { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["Logging.LogToEventLog"] ?? bool.FalseString); } }

        public bool LogToFile { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["Logging.LogToFile"] ?? bool.FalseString); } }

        public string SeqServerUrl { get { return ConfigurationManager.AppSettings["Seq.ServerUrl"] ?? "http://localhost:5341/"; } }

        public string SeqApiKey { get { return ConfigurationManager.AppSettings["Seq.ApiKey"] ?? string.Empty; } }
    }
}
