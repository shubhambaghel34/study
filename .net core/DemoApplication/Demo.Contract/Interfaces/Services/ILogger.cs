namespace Demo.Contract.Interfaces.Services
{
    using System;

    public interface ILogger
    {
        void LogError(string Error);
        void LogError(Exception Exception);
        void LogWarning(string Warning);
        void LogInfo(string Info);
        void LogDebug(string Message);
    }
}
