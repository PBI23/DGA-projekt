using Microsoft.Extensions.DependencyInjection;
using ProduktFlow2.Core.Repositories;
using ProduktFlow2.Core.Services;
using ProduktFlow2.Core.Models;
using System.Data;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starter op...");

        // 1. Dependency Injection setup
        var services = new ServiceCollection();

        string connectionString = "Server=localhost;Database=DGA_ProductDB;User Id=SA;Password=Projekt4@;TrustServerCertificate=true;";
        services.AddSingleton<IProductRepository>(provider => new ProductRepositoryDb(connectionString));
        services.AddSingleton<ProductService>();

        var serviceProvider = services.BuildServiceProvider();

        // 2. Resolve ProductService
        var productService = serviceProvider.GetRequiredService<ProductService>();

        // 3. Kald en metode for at teste at det virker
        var drafts = productService.GetAllDrafts();
        Console.WriteLine($"Fundet {drafts.Count} uafsluttede produktoprettelser.");


        // -- TESTS --
        // -- STEP 1 --
        var step1 = new Step1Dto
        {
            Name = "Test Produkt",
            Season = "Summer 2025",
            DgaItemNo = "DGA-TEST",
            CountryOfOrigin = "Denmark",
            Supplier = "Nordic Supplies",
            Designer = "Anna Møller",
            Description = "Test beskrivelse",
            ColiSize = "6",
            ProductGroup = "Home"
        };

        int productId = productService.HandleStep1(step1);
        Console.WriteLine($"Produkt oprettet med ID: {productId}");

        // -- STEP 2 --
        var step2 = new Step2Dto
        {
            ProductId = 3,
            Answers = new Dictionary<string, bool>
            {
                { "IsFoodApproved", true },
                { "GMP_Certified", false },
                { "FSC100", true },
                { "FSCMix70", false }
            }
        };

        productService.SaveStep2Answers(step2);
        Console.WriteLine("Step 2 svar gemt for produkt ID " + step2.ProductId);


        //-- STEP 3 --
        var step3 = new Step3Dto
        {
            ProductId = productId, // produkt fra step 1

            SupplierProductNo = 1234,
            CustomerClearanceNo = 5678,
            CustomerClearancePercent = 12.5m,
            CostPrice = 19.95m,

            InnerCarton = 10,
            OuterCarton = "20",

            GrossWeight = 1.25m,
            PackingHeight = 15.0m,
            PackingWidthLength = 10.0m,
            PackingDepth = 5.0m,

            DishwasherSafe = true,
            MicrowaveSafe = false,

            Svanemaerket = true,
            GrunerPunkt = false,
            FSC100 = true,
            FSCMix70 = false,

            ABC = "A",
            ProductLogo = "Minimalist",
            HangtagsAndStickers = "Yes",
            Series = "Summer2025"
        };

        productService.SaveStep3Answers(step3);
        Console.WriteLine("Step 3 gemt.");



        //-- STEP 4
        var step4 = new Step4Dto
        {
            ProductId = productId,
            DgaColorGroupName = "Standard",
            DgaSalCatGroup = "Seasonal",
            PantonePantone = "Classic Blue",
            DgaVendItemCodeCode = "SUPP-2025-A",
            Assorted = true,
            AdditionalInformation = "Extra details here.",
            GsmWeight = 300,
            BurningTimeHours = 48,
            AntidopingRegulation = false,
            Subcategory = "Tableware",
            OtherInformation2 = "Notes...",
            GsmWeight2 = 350
        };

        productService.SaveStep4Answers(step4);
        Console.WriteLine("Step 4 gemt.");



        // STEP 5
        int productIdToAprove = 3;

        var step5 = new Step5Dto
        {
            ProductId = productIdToAprove,
            IsApproved = true,
            ApprovedBy = "Nick",
            ApprovedAt = DateTime.Now
        };

        productService.SaveStep5Answers(step5);

        Console.WriteLine($"Produkt {productId} er nu godkendt af {step5.ApprovedBy}.");

        // GetFieldsForStep

        var stepNumber = 2;
        var fieldDefs = productService.GetFieldsForStep(stepNumber);

        Console.WriteLine($"Felter for step {stepNumber}:");
        foreach (var field in fieldDefs)
        {
            Console.WriteLine($"- {field.FieldName} ({field.Datatype}), Required: {field.IsRequired}, DependsOn: {field.DependsOn ?? "None"}");
        }


        // GetTriggeredFields
        Console.WriteLine("Tester GetTriggeredFields...");

        var parentField = "IsFoodApproved";
        var triggerValue = "true";
        var step = 2;

        var triggeredFields = productService.GetTriggeredFields(parentField, triggerValue, step);

        Console.WriteLine($"Fundet {triggeredFields.Count} afhængige felter for {parentField} = {triggerValue} på trin {step}");

        foreach (var field in triggeredFields)
        {
            Console.WriteLine($" - {field.FieldName} (Type: {field.Datatype}, Required: {field.IsRequired})");
        }

        
        //GetProductById
        int testProductId = 3; 
        var product = productService.GetProductById(testProductId);

        Console.WriteLine("Produktdetaljer:");
        Console.WriteLine($"ID: {product.ProductId}");
        Console.WriteLine($"Navn: {product.Name}");
        Console.WriteLine($"Season: {product.Season}");
        Console.WriteLine($"DGA Item No: {product.DgaItemNo}");
        Console.WriteLine($"Land: {product.CountryOfOrigin}");
        Console.WriteLine($"Leverandør: {product.Supplier}");
        Console.WriteLine($"Designer: {product.Designer}");
        Console.WriteLine($"Beskrivelse: {product.Description}");
        Console.WriteLine($"ColiSize: {product.ColiSize}");
        Console.WriteLine($"Produktgruppe: {product.ProductGroup}");
        Console.WriteLine($"Status: {product.Status}");

        //UpdateStatus
        productService.UpdateStatus(productId, "Approved");
        Console.WriteLine($"Status opdateret til 'Approved' for produkt {productId}");


    }
}
