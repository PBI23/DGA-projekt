using Microsoft.Extensions.DependencyInjection;
using ProduktFlow2.Core.Repositories;
using ProduktFlow2.Core.Services;
using ProduktFlow2.Test;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Starter testmiljø...");

        // DI container
        var services = new ServiceCollection();

        // 💡 Brug database-repository
        string connectionString = "Server=.;Database=ProduktDb;Trusted_Connection=True;";
        services.AddSingleton<IProductRepository>(provider => new ProductRepositoryDb(connectionString));

        // Registrer services
        services.AddSingleton<ProductService>();
        services.AddSingleton<ProductFlowTest>();

        // Build provider
        var serviceProvider = services.BuildServiceProvider();

        // Kør testflow
        var testRunner = serviceProvider.GetRequiredService<ProductFlowTest>();
        testRunner.RunAll();

        Console.WriteLine("\n✅ Alle testkørsler afsluttet.");
    }
}
