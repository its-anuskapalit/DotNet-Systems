using System;
using LoggingManager;

Logger.Initialize();
Logger.EnableGlobalExceptionLogging();
Logger.CaptureConsole();

Console.WriteLine("FIRST APP STARTED");
Console.WriteLine("Application started");
Console.WriteLine("Processing data...");

try
{
    int x = 10;
    int y = 0;
    int z = x / y;
}
catch (Exception ex)
{
    Logger.Instance.Error("Manual error caught", ex);
}

Task.Run(() =>
{
    throw new Exception("Background task failed");
});

Console.WriteLine("Program finished");
Console.ReadLine();