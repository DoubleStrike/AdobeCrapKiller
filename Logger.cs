using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeCrapKiller {
    public static class Logger {
        const string logFilePath = @"e:\tmp\adobe_crap_killer.log";

        /// <summary>
        /// Writes the specified message to the application log file, appending a timestamp.
        /// </summary>
        /// <remarks>If an error occurs while writing to the log file, the exception is suppressed and no
        /// message is logged. This method is thread-safe for typical usage, but concurrent writes may result in
        /// messages being interleaved in the log file.</remarks>
        /// <param name="message">The message to record in the log file. May be any string, including empty or null.</param>
        public static void Log(string message) {
            try {
                using (var writer = System.IO.File.AppendText(logFilePath)) {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            } catch (Exception) {
                // Ignore logging errors
            }
        }
    }
}
