using StackExchange.Redis;

namespace Repository.Redis
{
    public class RedisService
    {
        public IDatabase Database;

        public RedisService(string url)
        {
            var connectionMultiplaxer = ConnectionMultiplexer.Connect(url);

            connectionMultiplaxer.ConnectionFailed += ConnectionMultiplexer_ConnectionFailed;

            Database = connectionMultiplaxer.GetDatabase(1);
        }

        private void ConnectionMultiplexer_ConnectionFailed(object? sender, ConnectionFailedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}