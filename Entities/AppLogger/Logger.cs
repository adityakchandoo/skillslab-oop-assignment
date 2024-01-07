using System;
using System.IO;

namespace Entities.AppLogger
{
    public class Logger : ILogger
    {
        private readonly string logFilePath;
        private readonly string directoryPath;
        public Logger(string filePath = "Logs\\log.txt")
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            directoryPath = Path.Combine(baseDirectory, Path.GetDirectoryName(filePath));
            logFilePath = Path.Combine(baseDirectory, filePath);
        }

        public void Log(string message)
        {
            string logMessage = $"{message}";

            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath);
                }

                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
                throw ex;
                throw;
            }
        }
        public void LogError(Exception ex)
        {
            string fullMessage = "--------------------------------------------------";
            fullMessage += Environment.NewLine + $"Timestamp: {DateTime.Now}";
            fullMessage += Environment.NewLine + $"Exception Type: {this.GetType().FullName}";
            fullMessage += Environment.NewLine + $"Message: {ex.Message}";
            fullMessage += Environment.NewLine + $"Inner Exception: {ex.InnerException}";
            fullMessage += Environment.NewLine + $"Stack Trace: {ex.StackTrace}";
            fullMessage += Environment.NewLine + "--------------------------------------------------";

            Log(fullMessage);
        }
    }
}
