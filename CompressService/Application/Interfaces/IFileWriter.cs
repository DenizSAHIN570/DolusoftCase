using CompressService.Domain.Models;

namespace CompressService.Application.Interfaces
{
    public interface IFileWriter
    {
        void WriteLogEntriesToFile(List<LogEntry> logEntries, string filePath);
    }
}
