using CompressService.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using CompressService.Application.Interfaces;
using StackExchange.Redis;
using CompressService.Domain.Models;
using CompressService.Infrastructure.Services;

namespace CompressService.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection()
                .AddScoped<IRedisService, RedisService>()
                .AddScoped<IFileWriter, FileWriter>();

            //Redis initialization
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            IRedisService redisService = new RedisService(redis);

            IFileWriter fileWriter = new FileWriter();

            bool running = true;
            Console.WriteLine("Subscribed to log_channel. Press 'Q' to quit...");

            // Create a list to store log entries
            List<LogEntry> logEntries = new List<LogEntry>();

            // Subscribe to the log_channel and handle incoming messages
            redisService.Subscribe(logEntry =>
            {
                // Add the received log entry to the list
                logEntries.Add(logEntry);
                Console.WriteLine($"Received LogEntry: {logEntry.Timestamp} - {logEntry.Message}");
            });

            while (running)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    running = false; // Exit the loop
                }

                // Periodically write log entries to a file and compress it
                if (logEntries.Count > 0)
                {
                    // Generate a unique file name with a timestamp
                    string fileName = $"logs_{DateTime.Now:yyyyMMddHHmmss}.json.zstd";

                    // Write the log entries to the file
                    fileWriter.WriteLogEntriesToFile(logEntries, fileName);

                    // Clear the list after writing to the file
                    logEntries.Clear();
                }

                // Sleep for a short time to avoid high CPU usage
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
}
