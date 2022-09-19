using Fruit;

IFruitClient client = new FruitClient(
    new HttpClient() { BaseAddress = new Uri("https://localhost:7186") });

var created = await client.CreateFruitAsync("123",
    new Fruit.Fruit { Name = "Banana", Stock = 100 });
Console.WriteLine($"Created {created.Name}");

var fetched = await client.GetFruitAsync("123");
Console.WriteLine($"Fetched {fetched.Name}");
