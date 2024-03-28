using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace LogProvider
{
    [ProviderAlias("FileLogger")]
    class FileLoggerProvider : ILoggerProvider, IFileLogWriter
    {
        private bool _disposed;

        private LogLevel _logLevel;

        private FileStream _fileStream;

        private StreamWriter _fileWriter;

        private object _fileLock = new object();

        private ConcurrentDictionary<string, ILogger> _loggers = new (StringComparer.OrdinalIgnoreCase);

        public FileLoggerProvider(string filePath, LogLevel logLevel)
        {
            _logLevel = logLevel;
            _fileStream = new FileStream(filePath, FileMode.Append);
            _fileWriter = new StreamWriter(_fileStream);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, (name) => new FileLogger(this, name, _logLevel));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _fileWriter.Dispose();
                    _fileStream.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FileLoggerProvider()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void WriteMessage(string msg)
        {
            lock (_fileLock)
            {
                _fileWriter.Write(msg);
            }
        }
    }

    static class FileLoggerProviderExtension
    {
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, string logFilePath, LogLevel logLevel = LogLevel.Information)
        {
            var service = ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>(_ => new FileLoggerProvider(logFilePath, logLevel));
            builder.Services.Add(service);
            return builder;
        }
    }
}
