using CompressService.Application.Interfaces;
using CompressService.Domain.Models;
using System.Text.Json;
using ZstdNet;

namespace CompressService.Infrastructure.Services
{
    public class FileWriter : IFileWriter
    {
        private readonly string _outputFolder;

        public FileWriter(string outputFolder = "../../../Output")
        {
            _outputFolder = outputFolder;

            // Ensure the Output folder exists
            if (!Directory.Exists(_outputFolder))
            {
                Directory.CreateDirectory(_outputFolder);
            }
        }
        public void WriteLogEntriesToFile(List<LogEntry> logEntries, string fileName)
        {
            try
            {
                // Serialize the log entries to JSON
                string json = JsonSerializer.Serialize(logEntries);

                // Compress the JSON using Zstandard
                byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
                byte[] compressedBytes = Compress(jsonBytes);

                // Combine the output folder and file name to create the full path
                string filePath = Path.Combine(_outputFolder, fileName);

                // Write the compressed data to the file
                File.WriteAllBytes(filePath, compressedBytes);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FileWriter Error] Failed to write log entries: {ex.Message}");
            }
        }

        private byte[] Compress(byte[] data)
        {
            using (var compressor = new Compressor())
            {
                return compressor.Wrap(data);
            }
        }
    }
}