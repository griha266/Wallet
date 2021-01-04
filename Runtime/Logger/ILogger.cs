using System;

namespace WalletLib.Logger
{
    /// <summary>
    /// Interface for logging information
    /// </summary>
    /// <remarks>
    /// <para>Can log simple messages, warnings, exceptions and error messages</para>
    /// </remarks>
    public interface ILogger
    {
        /// <summary>
        /// Simple text logging 
        /// </summary>
        /// <param name="message">Message to log</param>
        void Log(string message);
        
        /// <summary>
        /// Log text as warning message
        /// </summary>
        /// <param name="message">Warning message</param>
        void LogWarning(string message);

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="exception">Exception to log</param>
        void LogException(Exception exception);

        /// <summary>
        /// Log text as error message
        /// </summary>
        /// <param name="message">Error message</param>
        void LogError(string message);
    }

}