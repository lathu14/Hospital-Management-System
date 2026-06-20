using System;
using System.IO;

namespace Hospital_Management_System.Helpers
{
    public static class Logger
    {
        private static readonly string LogFilePath;

        static Logger()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            LogFilePath = Path.Combine(baseDir, "logs", "hospital_backend.log");
        }

        public static void Log(string message, string type = "INFO")
        {
            try
            {
                string dir = Path.GetDirectoryName(LogFilePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{type}] {message}{Environment.NewLine}";
                File.AppendAllText(LogFilePath, logLine);
            }
            catch
            {
                // Suppress logging errors to prevent application crash
            }
        }

        public static void LogError(Exception ex, string context = "")
        {
            Log($"{context} - Error: {ex.Message} {Environment.NewLine}Stack Trace: {ex.StackTrace}", "ERROR");
        }
    }
}
