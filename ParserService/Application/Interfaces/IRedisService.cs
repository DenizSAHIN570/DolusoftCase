using ParserService.Domain.Models;

namespace ParserService.Application.Interfaces
{
    public interface IRedisService
    {
        void Publish(LogEntry logEntry);
        void Subscribe(Action<LogEntry> messageHandler);
    }
}