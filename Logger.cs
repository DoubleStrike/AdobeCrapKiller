using System;

namespace AdobeCrapKiller {
    /// <summary>
    /// Specifies the severity level of a log message.
    /// </summary>
    public enum LogLevel {
        /// <summary>
        /// Informational/debug messages.
        /// </summary>
        Info = 0,

        /// <summary>
        /// Warnings that may require attention.
        /// </summary>
        Warning = 1,

        /// <summary>
        /// Error conditions.
        /// </summary>
        Error = 2
    }

    /// <summary>
    /// Provides static methods for writing messages to the application log file with timestamps.
    /// </summary>
    /// <remarks>The Logger class is intended for simple logging scenarios where messages are written to a
    /// fixed log file. Logging errors are silently ignored to avoid interfering with application flow. This class is
    /// thread-safe for typical usage, but concurrent calls may result in interleaved log entries.</remarks>
    public static class Logger {
        const string logFilePath = @"e:\tmp\adobe_crap_killer.log";

        /// <summary>
        /// Gets or sets the minimum <see cref="LogLevel"/> that will be recorded. Messages with a lower severity are ignored.
        /// </summary>
        public static LogLevel MinimumLogLevel { get; set; } = LogLevel.Warning;

        /// <summary>
        /// Writes the specified message to the application log file, appending a timestamp.
        /// </summary>
        /// <remarks>If an error occurs while writing to the log file, the exception is suppressed and no
        /// message is logged. This method is thread-safe for typical usage, but concurrent writes may result in
        /// messages being interleaved in the log file.</remarks>
        /// <param name="message">The message to record in the log file. May be any string, including empty or null.</param>
        /// <param name="level">The severity level of the message. Messages below <see cref="MinimumLogLevel"/> are ignored.</param>
        public static void Log(string message, LogLevel level = LogLevel.Info) {
            if (level < MinimumLogLevel) {
                return;
            }

            try {
                using (System.IO.StreamWriter writer = System.IO.File.AppendText(logFilePath)) {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t\t{level}\t\t{message}");
                }
            } catch (Exception) {
                // Ignore logging errors
            }
        }
    }
}
