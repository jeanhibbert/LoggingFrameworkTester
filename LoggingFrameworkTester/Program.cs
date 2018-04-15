using MyCompany.Core;
using MyCompany.Core.Serilog;
using Serilog;
using System;
using System.Threading;

namespace LoggingFrameworkTester
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += SerilogStartup.HandleUnhandledException;

            ILoggingFrameworkTesterConfig config = new LoggingFrameworkTesterConfig();

            SerilogStartup.InitialiseLogger(config);

            while (true)
            {
                for (int i = 1; i <= 20; i++)
                {
                    ComplexData complexData = new ComplexData { Sequence = i, CreatedDate = DateTime.UtcNow };
                    try
                    {
                        Log.Information("Ok {AdUserName} .... lets see what happens now {@ComplexData}", Environment.UserDomainName, complexData);
                        if (i % 3 == 0)
                        {
                            complexData.ErrorTypeId = 3;
                            Log.Warning("Was divisible by 3!! - {@ComplexData}", complexData);
                        }
                        if (i % 4 == 0)
                        {
                            complexData.ErrorTypeId = 4;
                            throw new NullReferenceException("Dodgy null reference exceptions! ... Cant wait for C#7!");
                        }
                        if (i % 5 == 0)
                        {
                            complexData.ErrorTypeId = 5;
                            throw new ArgumentException("Another argument exception! Should have used a constraint!");
                        }
                    }
                    catch (Exception ex)
                    {
                        if (complexData.ErrorTypeId == 4) Log.Error(ex, "Was divisible by 4!! - {@ComplexData}", complexData);
                        if (complexData.ErrorTypeId == 5) Log.Fatal(ex, "Was divisible by 5!! - {@ComplexData}", complexData);
                    }
                    Thread.Sleep(1000);
                }

                Console.WriteLine("Press ESCAPE to exit or change the logging level - choose between 0-5");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
                SerilogStartup.LoggingLevelSwitch.MinimumLevel = (Serilog.Events.LogEventLevel)(int)Char.GetNumericValue(key.KeyChar);
            }
        }
    }
}
