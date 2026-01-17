using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeCrapKiller {
    public static class Logger {
        const string logFilePath = @"e:\tmp\adobe_crap_killer.log";

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
