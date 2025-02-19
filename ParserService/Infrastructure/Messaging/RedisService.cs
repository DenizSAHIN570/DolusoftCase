using ParserService.Application.Interfaces;
using ParserService.Domain.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace ParserService.Infrastructure.Messaging
{
    public class RedisService : IRedisService
    {
        private readonly ISubscriber _subscriber;
        private string v;
        private const string ChannelName = "log_channel";

        public RedisService(ConnectionMultiplexer redis)
        {
            _subscriber = redis.GetSubscriber();
        }

        public RedisService(string v)
        {
            this.v = v;
        }

        public void Publish(LogEntry logEntry)
        {
            try
            {
                string message = JsonSerializer.Serialize(logEntry);
                _subscriber.Publish(ChannelName, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Redis Error] Failed to publish message: {ex.Message}");
            }
        }

        public void Subscribe(Action<LogEntry> messageHandler)
        {
            _subscriber.Subscribe(ChannelName, (channel, message) =>
            {
                try
                {
                    var logEntry = JsonSerializer.Deserialize<LogEntry>(message);
                    messageHandler?.Invoke(logEntry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Redis Error] Failed to process message: {ex.Message}");
                }
            });
        }
    }
}
