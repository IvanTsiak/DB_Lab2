using Microsoft.Extensions.Configuration;
using DbBenchmarkApp;

namespace DbBenchmarkApp;

class Program
{
    static async Task Main(string[] args)
    {
        // var config = new ConfigurationBuilder()
        //     .AddUserSecrets<Program>()
        //     .Build();
        //
        // string pgConnectionString = config.GetConnectionString("PostgresConnection") 
        //                             ?? throw new InvalidOperationException("PostgreSQL connection string not found!");
        //     
        // string mongoConnectionString = config.GetConnectionString("MongoConnection") 
        //                                ?? throw new InvalidOperationException("MongoDB connection string not found!");
        //
        // var runner = new BenchmarkRunner(pgConnectionString, mongoConnectionString);
        //
        // await runner.RunBenchmarkAsync(1000);
        
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        string redisConn = config.GetConnectionString("RedisConnection") 
                           ?? "localhost:6379";

        var redisDemo = new RedisDemo(redisConn);
        redisDemo.RunBasicQueries();
    }
}