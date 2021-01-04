using System;
using UnityEngine;

namespace WalletLib.Logger.Unity
{
    /// <inheritdoc/>
    /// <summary>
    /// Class for logging via <see cref="UnityEngine.Debug"/>
    /// </summary>
    public class UnityLogger : ILogger
    {
        public void Log(string message) => Debug.Log(message);
        public void LogError(string message) => Debug.LogError(message);
        public void LogException(Exception exception) => Debug.LogException(exception);
        public void LogWarning(string message) => Debug.LogWarning(message);
    }
}
