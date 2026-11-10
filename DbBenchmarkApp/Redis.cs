using StackExchange.Redis;

namespace DbBenchmarkApp;

public class RedisDemo
{
    private readonly ConnectionMultiplexer _redis;

    public RedisDemo(string connectionString)
    {
        _redis = ConnectionMultiplexer.Connect(connectionString);
    }

    public void RunBasicQueries()
    {
        var db = _redis.GetDatabase();

        Console.WriteLine("basic queriesRedis");
        string viewKey = "product:105:views";
        
        db.StringSet(viewKey, 100);
        Console.WriteLine($"Views set for product 105: 100");
        
        int views = (int)db.StringGet(viewKey);
        Console.WriteLine($"Current number of views: {views}");
        
        long updatedViews = db.StringIncrement(viewKey);
        Console.WriteLine($"User viewed a product. New views: {updatedViews}\n");

        
        string cartKey = "cart:session_abc123";
        
        db.HashSet(cartKey, new HashEntry[]
        {
            new HashEntry("product_1", 2),
            new HashEntry("product_42", 1)
        });
        Console.WriteLine("Items have been added to the cart for session session_abc123");
        
        HashEntry[] cartItems = db.HashGetAll(cartKey);
        Console.WriteLine("Cart contents:");
        foreach (var item in cartItems)
        {
            Console.WriteLine($"  - Product ID: {item.Name}, Amount: {item.Value}");
        }
        Console.WriteLine();
        
        db.KeyExpire(cartKey, TimeSpan.FromSeconds(1800));
        
        TimeSpan? ttl = db.KeyTimeToLive(cartKey);
        Console.WriteLine($"Time until cart session expires: {ttl?.TotalSeconds} s");
        
        db.KeyDelete(cartKey);
        Console.WriteLine($"The session cart was successfully deleted");

        bool exists = db.KeyExists(cartKey);
        Console.WriteLine($"Does the Recycle Bin exist after deletion? {exists}");
    }
}