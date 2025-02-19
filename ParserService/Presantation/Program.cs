using Microsoft.Extensions.DependencyInjection;
using ParserService.Application.Interfaces;
using ParserService.Infrastructure.FileService;
using ParserService.Infrastructure.Messaging;
using ParserService.Infrastructure.Parsing;
using StackExchange.Redis;

namespace ParserService.Presantation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Service initialization
            var serviceCollection = new ServiceCollection()
                .AddScoped<IFileService, FileService>()
                .AddScoped<IRedisService, RedisService>()
                .AddScoped<ILogParser, LogParser>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            //Temp log path for parsing
            string logPath = "../../../Logs/Logs.txt";

            //Redis initialization
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            IRedisService redisService = new RedisService(redis);
            var logParser = serviceProvider.GetService<ILogParser>();

            try
            {
                var logEntries = logParser.Parse(logPath);

                //Publishing messages
                foreach (var logEntry in logEntries)
                {
                    redisService.Publish(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
