namespace Demo.Services.Logger
{
    using System;
    using ILogger = Demo.Contract.Interfaces.Services.ILogger;

    public class Logger : ILogger
    {
        public static NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger(); 

        public void LogError(string Error)
        {
            _logger.Error($"Error: {Error}");
        }

        public void LogError(Exception exception)
        {
            _logger.Error(exception, exception.Message);
        }

        public void LogInfo(string Info)
        {
            _logger.Info(Info);
        }

        public void LogDebug(string Message)
        {
            _logger.Debug(Message);
        }

        public void LogWarning(string Warning)
        {
            _logger.Warn(Warning);
        }
    }
}
