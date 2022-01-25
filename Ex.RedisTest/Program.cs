using StackExchange.Redis;

namespace Ex.RedisTest
{
    class Program
    {
        //static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
        //    new ConfigurationOptions
        //    {
        //        EndPoints = { "localhost:6379" }
        //    });

        static async Task Main(string[] args)
        {
            ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("112.153.1.31:6379");
            var db = redis.GetDatabase();
            var pong = await db.PingAsync();
            Console.WriteLine(pong);


            db.StringSet("hello", "world");
            db.StringSet("code", "hard");
            db.StringSet("redis", "easy");

            string value = db.StringGet("test");
            Console.WriteLine(value);
            Console.WriteLine(db.StringGet("hello"));
            Console.WriteLine(db.StringGet("redis"));

            ISubscriber sub = redis.GetSubscriber();
            sub.Subscribe("messages", (channel, message) =>
            {
                Console.WriteLine($"Recived Message {message}");
                Console.WriteLine($"-------------------------");
            });

            Thread.Sleep(3000);
            sub.Subscribe("messages").OnMessage(async channelMessage => {
                await Task.Delay(1000);
                Console.WriteLine($"Async Message {channelMessage}");
            });

            sub.Publish("messages", "hello!!");
            Thread.Sleep(10000);
        }
    }
}