using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;

namespace MyCompany.Core.Serilog
{
    public class SerilogStartup
    {
        public static ILogger Logger { get; set; }

        public static LoggingLevelSwitch LoggingLevelSwitch { get; set; }

        public static void InitialiseLogger(ILoggingFrameworkTesterConfig config)
        {
            // dynamic logging level, minimum level can be altered at runtime...nice!
            LoggingLevelSwitch = new LoggingLevelSwitch();
            LoggingLevelSwitch.MinimumLevel = LogEventLevel.Verbose;

            var loggerConfig = CreateCoreLogger()
                .MinimumLevel.ControlledBy(LoggingLevelSwitch);

            // Not necessary in code since rolling file logging has been set up in config file
            //if (config.LogToFile)
            //{
            //    loggerConfig.WriteTo.RollingFile(pathFormat: @"Logs/lft-{Date}.txt", retainedFileCountLimit: 7);
            //}

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
                .ReadFrom.AppSettings() // Read serilog entries from config file
                .WriteTo.ColoredConsole() // Write out serilog logging entries to the console and color code them
                
                // meta data for seq entries
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
