using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;

namespace MyCompany.Core.Serilog
{
    public class SerilogStartup
    {
        public static ILogger Logger { get; set; }

        public static void InitialiseLogger(ILoggingFrameworkTesterConfig config)
        {
            // dynamic logging level, minimum level can be altered at runtime...nice!
            var levelSwitch = new LoggingLevelSwitch();

            var loggerConfig = CreateCoreLogger()
                .MinimumLevel.ControlledBy(levelSwitch);

            // Only log to Seq if it is turned on in config file
            if (config.LogToSeqServer)
            {
                loggerConfig.WriteTo.Seq(config.SeqServerUrl, apiKey: config.SeqApiKey);
            }

            // If event log logging it turned on in app settings then only log warnings and above to event log
            if (config.LogToEventLog)
            {
                loggerConfig.WriteTo.EventLog(
                    config.ApplicationName,
                    "Application",
                    restrictedToMinimumLevel: LogEventLevel.Warning);
            }

            Logger = loggerConfig.CreateLogger();
            Log.Logger = Logger;
        }

        private static LoggerConfiguration CreateCoreLogger()
        {
            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .WriteTo.ColoredConsole()
                .Enrich.FromLogContext()
                .Enrich.With<ApplicationVersionEnricher>()
                .Enrich.WithMachineName();

            return loggerConfig;
        }

        public static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var logger = Logger ?? (Logger = CreateCoreLogger().CreateLogger());
            logger.Fatal(
                (Exception)e.ExceptionObject,
                "Unhandled Exception! CLR Terminating: {IsTerminating}",
                e.IsTerminating);
        }

    }
}
