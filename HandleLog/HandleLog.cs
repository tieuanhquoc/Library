using Microsoft.Extensions.Logging;

namespace HandleLog;

public class HandleLog : ILogger
{
    private readonly string _filePath;
    private static readonly object Lock = new();

    public HandleLog(string path)
    {
        _filePath = path;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (formatter != null)
        {
            lock (Lock)
            {
                var fullFilePath = Path.Combine(_filePath, DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt");
                var n = Environment.NewLine;
                var exc = "";
                if (exception != null)
                    exc = n + exception.GetType() + ": " + exception.Message + n + exception.StackTrace + n;
                File.AppendAllText(fullFilePath,
                    logLevel + ": " + DateTime.Now + " " + formatter(state, exception) + n + exc);
            }
        }
    }
}

public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly string _path;

    public FileLoggerProvider(string path)
    {
        path = string.IsNullOrEmpty(path) ? "Logs" : path;
        this._path = path;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new HandleLog(_path);
    }

    public void Dispose()
    {
        //
    }
}