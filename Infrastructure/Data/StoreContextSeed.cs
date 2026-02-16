using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        Console.WriteLine("Seeding data...");
        if (!context.Products.Any())
        {
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);

            Console.WriteLine(products);
            Console.WriteLine("Products deserialized");
            
            if (products == null) return;

            context.Products.AddRange(products);
            Console.WriteLine("Products added to context");
            await context.SaveChangesAsync();
            Console.WriteLine("Changes saved");
        }
    }

}
