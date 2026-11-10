using System.Diagnostics;
using Dapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Npgsql;

namespace DbBenchmarkApp;

public class BenchmarkRunner
{
    private readonly string _pgConnString;
    private readonly string _mongoConnString;

    public BenchmarkRunner(string pgConnString, string mongoConnString)
    {
        _pgConnString = pgConnString;
        _mongoConnString = mongoConnString;
    }

    public async Task RunBenchmarkAsync(int iterations = 100)
    {
        Console.WriteLine($"Iterations count: {iterations}\n");
        
        using var pgConn = new NpgsqlConnection(_pgConnString);
        await pgConn.OpenAsync();

        string sqlQuery = @"
            SELECT p.Id, p.SKU, p.Name
            FROM Product_NoSQL_Test p
            JOIN Product_Attribute_Value pav1 ON p.Id = pav1.ProductId
            JOIN Attribute_Dict ad1 ON pav1.AttributeId = ad1.Id AND ad1.Name = 'Color'
            JOIN Product_Attribute_Value pav2 ON p.Id = pav2.ProductId
            JOIN Attribute_Dict ad2 ON pav2.AttributeId = ad2.Id AND ad2.Name = 'Memory'
            WHERE p.CategoryName = 'Smartphones'
              AND pav1.Value = 'Black'
              AND pav2.Value = '256GB'
            LIMIT 100;";
        
        for (int i = 0; i < 10; i++)
        {
            var _ = await pgConn.QueryAsync(sqlQuery);
        }

        var pgTimes = new List<double>();
        var sw = new Stopwatch();

        for (int i = 0; i < iterations; i++)
        {
            sw.Restart();
            var result = await pgConn.QueryAsync(sqlQuery);
            sw.Stop();
            pgTimes.Add(sw.Elapsed.TotalMilliseconds);
        }
        
        var mongoClient = new MongoClient(_mongoConnString);
        var mongoDb = mongoClient.GetDatabase("storedb_nosql");
        var collection = mongoDb.GetCollection<BsonDocument>("products");

        var builder = Builders<BsonDocument>.Filter;
        var mongoFilter = builder.Eq("category", "Smartphones") &
                          builder.Eq("attributes.color", "Black") &
                          builder.Eq("attributes.memory", "256GB");
        
        for (int i = 0; i < 10; i++)
        {
            var _ = await collection.Find(mongoFilter).Limit(100).ToListAsync();
        }

        var mongoTimes = new List<double>();

        for (int i = 0; i < iterations; i++)
        {
            sw.Restart();
            var result = await collection.Find(mongoFilter).Limit(100).ToListAsync();
            sw.Stop();
            mongoTimes.Add(sw.Elapsed.TotalMilliseconds);
        }
        
        Console.WriteLine("\nPostgreSQL:");
        Console.WriteLine($"Avg: {pgTimes.Average():F3} ms");
        Console.WriteLine($"Min: {pgTimes.Min():F3} ms");
        Console.WriteLine($"Max: {pgTimes.Max():F3} ms");
        Console.WriteLine("\nMongoDB:");
        Console.WriteLine($"Avg: {mongoTimes.Average():F3} ms");
        Console.WriteLine($"Min: {mongoTimes.Min():F3} ms");
        Console.WriteLine($"Max: {mongoTimes.Max():F3} ms");
    }
}