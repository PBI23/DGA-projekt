using Microsoft.Extensions.DependencyInjection;
using ProduktFlow2.Core.Repositories;
using ProduktFlow2.Core.Services;
using ProduktFlow2.Core.Models;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starter op...");

        // 1. Dependency Injection setup
        var services = new ServiceCollection();

        string connectionString = "Server=.;Database=ProduktDb;Trusted_Connection=True;";
        services.AddSingleton<IProductRepository>(provider => new ProductRepositoryDb(connectionString));
        services.AddSingleton<ProductService>();

        var serviceProvider = services.BuildServiceProvider();

        // 2. Resolve ProductService
        var productService = serviceProvider.GetRequiredService<ProductService>();

        // 3. Kald en metode for at teste at det virker
        var drafts = productService.GetAllDrafts();

        Console.WriteLine($"Fundet {drafts.Count} uafsluttede produktoprettelser.");
    }
}
