using CompressService.Domain.Models;

namespace CompressService.Application.Interfaces
{
    public interface IRedisService
    {
        void Publish(LogEntry logEntry);
        void Subscribe(Action<LogEntry> messageHandler);
    }
}