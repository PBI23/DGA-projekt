using ProduktFlow2.Test;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== Starter tests ===");

        var test = new ProductFlowTest();
        test.RunAll();

        Console.WriteLine("=== Alle tests er kørt ===");
        Console.WriteLine("Tryk en tast for at afslutte...");
        Console.ReadKey();
    }
}
