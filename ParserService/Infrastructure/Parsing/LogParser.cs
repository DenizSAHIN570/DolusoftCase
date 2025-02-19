using ParserService.Application.Interfaces;
using ParserService.Domain.Models;
using System.Globalization;

namespace ParserService.Infrastructure.Parsing
{
    public class LogParser : ILogParser
    {
        private const string TimestampFormat = "yyyy-MM-ddTHH:mm:ssZ";
        private readonly IFileService _fileService;

        public LogParser(IFileService fileService)
        {
            _fileService = fileService;
        }

        public List<LogEntry> Parse(string filePath)
        {
            // Validate the file path before proceeding
            ValidateFilePath(filePath);

            var logEntries = new List<LogEntry>();

            try
            {
                // Use the injected file service to read lines
                string[] lines = _fileService.ReadAllLines(filePath);

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
                            throw new ApplicationException($"Could not parse timestamp in line: {line}", ex);
                        }
                    }
                    else
                    {
                        throw new ApplicationException($"Invalid log entry format: {line}");
                    }
                }
            }
            catch (IOException ex)
            {
                throw new ApplicationException($"Error reading log file: {ex.Message}", ex);
            }

            return logEntries;
        }

        private void ValidateFilePath(string filePath)
        {
            if (!_fileService.FileExists(filePath))
            {
                throw new FileNotFoundException($"Log file not found at path: {filePath}");
            }
        }
    }
}
