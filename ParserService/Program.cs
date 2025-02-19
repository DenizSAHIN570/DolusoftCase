using System.Globalization;
using Newtonsoft.Json;
using ParserService.Domain.Models;

namespace ParserService
{
    public class Program
    {
        // Configurable timestamp format
        private const string TimestampFormat = "yyyy-MM-ddTHH:mm:ssZ";

        public static void Main(string[] args)
        {
            string logPath = "../../../Logs/Logs.txt";
            try
            {   
                ValidateFilePath(logPath);

                var logEntries = Parse(logPath);

                ConvertToJson(logEntries);
            }
            catch (Exception ex)
            {
                LogError("An error occurred in the main program", ex);
            }
        }

        private static void ValidateFilePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Log file not found at path: {filePath}");
            }
        }

        public static void ConvertToJson(List<LogEntry> logEntries)
        {
            string json = JsonConvert.SerializeObject(logEntries, Formatting.Indented);
            Console.WriteLine(json);
        }

        public static List<LogEntry> Parse(string filePath)
        {
            List<LogEntry> logEntries = new List<LogEntry>();

            try
            {
                // Read all lines from the log file
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    // Split the input line into timestamp and message parts
                    string[] parts = line.Split(new[] { " - " }, StringSplitOptions.None);

                    if (parts.Length == 2)
                    {
                        try
                        {
                            logEntries.Add(new LogEntry
                            {
                                Timestamp = DateTime.ParseExact(parts[0], TimestampFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal),
                                Message = parts[1]
                            });
                        }
                        catch (FormatException ex)
                        {
                            LogError($"Could not parse timestamp in line: {line}", ex);
                        }
                    }
                    else
                    {
                        LogError($"Invalid log entry format: {line}", null);
                    }
                }
            }
            catch (IOException ex)
            {
                LogError("Error reading log file", ex);
                throw new Exception($"Error reading log file: {ex.Message}", ex);
            }

            return logEntries;
        }


        private static void LogError(string message, Exception ex)
        {
            string errorMessage = $"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            if (ex != null)
            {
                errorMessage += $"\nException: {ex.Message}";
            }
            Console.Error.WriteLine(errorMessage); //Temp error logging to the console.
        }
    }
}
