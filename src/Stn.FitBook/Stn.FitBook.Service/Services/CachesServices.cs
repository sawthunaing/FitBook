using Microsoft.Extensions.Logging;
using Stn.FitBook.Domain.IServices;
using StackExchange.Redis;
using Google.Protobuf.WellKnownTypes;

namespace Stn.FitBook.Service.Services
{
    public class CachesServices : ICachesService
    {
        private readonly ILogger<CachesServices> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public CachesServices(ILogger<CachesServices> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _connectionMultiplexer = connectionMultiplexer;

        }

        public async Task<string> GetClassScheduleSlots(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await EnsureKeyExists(key, db);
            var value = await db.StringGetAsync(key);
            var test = value.ToString();
            return test;
        }
        public async Task SetClassScheduleSlots(string key, string value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(key, value);
           
        }
        public async Task RemoveClassScheduleSlots(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await EnsureKeyExists(key, db);

            await db.KeyDeleteAsync(key);
                  
           
        }
        public async Task UpdateClassScheduleSlots(string key,string value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await EnsureKeyExists(key, db);

            await db.StringSetAsync(key, value);
        }

        private async Task EnsureKeyExists(string key, IDatabase db)
        {
            var result = await db.StringGetAsync(key);
           
        }
    }
}
