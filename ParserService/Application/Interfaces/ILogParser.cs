using ParserService.Domain.Models;

namespace ParserService.Application.Interfaces
{
    public interface ILogParser
    {
        List<LogEntry> Parse(string filePath);
    }
}
