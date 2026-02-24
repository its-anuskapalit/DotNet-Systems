using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace LoggingManager
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Debug
    }
    public sealed class Logger : IDisposable
    {
        private readonly string _logDirectory;
        private readonly BlockingCollection<string> _queue = new();
        private readonly CancellationTokenSource _cts = new();
        private readonly Task _worker;
        private readonly long _maxFileSizeBytes = 5 * 1024 * 1024;
        private static Logger _instance;
        private static readonly object _lock = new();
        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("Logger not initialized. Call Initialize().");
                return _instance;
            }
        }
        private Logger(string logDirectory)
        {
            _logDirectory = logDirectory;
            Directory.CreateDirectory(_logDirectory);
            _worker = Task.Run(ProcessQueue);
        }
        public static void Initialize(string logDirectory = null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    logDirectory ??= Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                    _instance = new Logger(logDirectory);
                }
            }
        }
        public void Log(LogLevel level, string message, Exception ex = null)
        {
            var log = FormatMessage(level, message, ex);
            _queue.Add(log);
        }

        public void Info(string message) => Log(LogLevel.Info, message);
        public void Warning(string message) => Log(LogLevel.Warning, message);
        public void Debug(string message) => Log(LogLevel.Debug, message);
        public void Error(string message, Exception ex = null) => Log(LogLevel.Error, message, ex);
        private async Task ProcessQueue()
        {
            foreach (var log in _queue.GetConsumingEnumerable(_cts.Token))
            {
                try
                {
                    var path = GetLogFilePath();
                    await File.AppendAllTextAsync(path, log + Environment.NewLine, Encoding.UTF8);
                }
                catch
                {
                }
            }
        }
        private string GetLogFilePath()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var baseFile = Path.Combine(_logDirectory, $"log_{date}.txt");

            if (!File.Exists(baseFile))
                return baseFile;

            var info = new FileInfo(baseFile);
            if (info.Length < _maxFileSizeBytes)
                return baseFile;

            int index = 1;
            string newFile;
            do
            {
                newFile = Path.Combine(_logDirectory, $"log_{date}_{index}.txt");
                index++;
            }
            while (File.Exists(newFile));

            return newFile;
        }
        private static string FormatMessage(LogLevel level, string message, Exception ex)
        {
            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            sb.Append($" | {level.ToString().ToUpper()}");
            sb.Append($" | {message}");

            if (ex != null)
            {
                sb.Append($" | EX: {ex.Message}");
                sb.Append($" | STACK: {ex.StackTrace}");
            }

            return sb.ToString();
        }
        public static void EnableGlobalExceptionLogging()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (args.ExceptionObject is Exception ex)
                    Instance.Error("Unhandled exception occurred", ex);
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                Instance.Error("Unobserved task exception", args.Exception);
                args.SetObserved();
            };
        }
        public static void CaptureConsole()
        {
            Console.SetOut(new ConsoleInterceptor(Console.Out, msg =>
            {
                Instance.Info($"CONSOLE: {msg}");
            }));

            Console.SetError(new ConsoleInterceptor(Console.Error, msg =>
            {
                Instance.Error($"CONSOLE ERROR: {msg}");
            }));
        }
        public void Dispose()
        {
            _queue.CompleteAdding();
            _cts.Cancel();
            _worker.Wait(2000);
        }
    }
}