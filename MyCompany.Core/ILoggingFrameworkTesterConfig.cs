namespace MyCompany.Core
{
    public interface ILoggingFrameworkTesterConfig
    {
        string ApplicationName { get; }
        bool LogToSeqServer { get; }
        bool LogToEventLog { get; }
        string SeqServerUrl { get; }
        string SeqApiKey { get; }
        bool LogToFile { get; }
    }
}
