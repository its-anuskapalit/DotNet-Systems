using System;
using System.Threading.Tasks;
using LoggingManager;

Logger.Initialize();
Logger.EnableGlobalExceptionLogging();
Logger.CaptureConsole();

Console.WriteLine("SECOND APP STARTED");
Logger.Instance.Info("Info from second app");
Logger.Instance.Warning("Warning from second app");

try
{
    int a = 100;
    int b = 0;
    int c = a / b;
}
catch (Exception ex)
{
    Logger.Instance.Error("Handled exception in second app", ex);
}

Task.Run(() =>
{
    throw new Exception("Background crash in second app");
});
Console.WriteLine("SECOND APP FINISHED");

Console.ReadLine();