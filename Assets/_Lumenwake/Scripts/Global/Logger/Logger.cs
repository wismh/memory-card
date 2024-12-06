using System;
using System.Runtime.CompilerServices;

namespace Lumenwake
{
    public enum LogLevel
    {
        Debug,
        Log,
        Warning,
        Error,
        Critical
    }

    public static class LoggingSystem
    {
        public static LogLevel CurrentLogLevel
        {
            get; set;
        } =

#if UNITY_EDITOR

            LogLevel.Log;

#else

            LogLevel.Log;

#endif

        public static bool IsLoggingEnabled
        {
            get; set;
        } =

#if UNITY_EDITOR

            true;

#else

            true;

#endif

        private static void Message(LogLevel level, string message, UnityEngine.Object context = null,
            [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (!IsLoggingEnabled || level < CurrentLogLevel)
            {
                return;
            }

            // Prepare log message with additional context
            string formattedMessage;

            // Log based on level
            switch (level)
            {
                case LogLevel.Debug:

                    formattedMessage = $"<color=grey>[{level}] {message}" +
                    $"\nMethod: {memberName}" +
                    $"\nFile: {System.IO.Path.GetFileName(sourceFilePath)}" +
                    $"\nLine: {sourceLineNumber}</color>";

                    UnityEngine.Debug.Log(formattedMessage, context);
                    break;

                case LogLevel.Log:

                    formattedMessage = $"<color=white>[{level}] {message}" +
                    $"\nMethod: {memberName}" +
                    $"\nFile: {System.IO.Path.GetFileName(sourceFilePath)}" +
                    $"\nLine: {sourceLineNumber}</color>";

                    UnityEngine.Debug.Log(formattedMessage, context);
                    break;

                case LogLevel.Warning:

                    formattedMessage = $"<color=yellow>[{level}] {message}" +
                    $"\nMethod: {memberName}" +
                    $"\nFile: {System.IO.Path.GetFileName(sourceFilePath)}" +
                    $"\nLine: {sourceLineNumber}</color>";

                    UnityEngine.Debug.LogWarning(formattedMessage, context);
                    break;

                case LogLevel.Error:

                    formattedMessage = $"<color=red>[{level}] {message}" +
                    $"\nMethod: {memberName}" +
                    $"\nFile: {System.IO.Path.GetFileName(sourceFilePath)}" +
                    $"\nLine: {sourceLineNumber}</color>";
                    UnityEngine.Debug.LogError(formattedMessage, context);
                    break;

                case LogLevel.Critical:

                    formattedMessage = $"<color=red><b>[{level}] {message}" +
                    $"\nMethod: {memberName}" +
                    $"\nFile: {System.IO.Path.GetFileName(sourceFilePath)}" +
                    $"\nLine: {sourceLineNumber}</color></b>";

                    UnityEngine.Debug.LogError(formattedMessage, context);
                    break;
            }
        }

        // Public logging methods
        public static void Debug(string message, UnityEngine.Object context = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Message(LogLevel.Debug, message, context, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void Log(string message, UnityEngine.Object context = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Message(LogLevel.Log, message, context, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void LogWarning(string message, UnityEngine.Object context = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Message(LogLevel.Warning, message, context, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void LogError(string message, UnityEngine.Object context = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Message(LogLevel.Error, message, context, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void Critical(string message, UnityEngine.Object context = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Message(LogLevel.Critical, message, context, memberName, sourceFilePath, sourceLineNumber);
        }

        // Exception logging
        public static void LogException(Exception ex,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            string message = $"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}";
            Message(LogLevel.Critical, message, null, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}
