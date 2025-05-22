using ProduktFlow2.Core.Models;
using ProduktFlow2.Core.Repositories;
using ProduktFlow2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktFlow2.Test
{
    public class ProductFlowTest
    {
        private readonly ProductService _service;
        private readonly ProductRepositoryDb _repo;

        public ProductFlowTest()
        {
            _repo = new ProductRepositoryDb("your_connection_string_here");
            _service = new ProductService(_repo);
        }

        public void RunAll()
        {
            Console.WriteLine("?? Kører trin 1 test...");
            TestStep1();
            Console.WriteLine("? Trin 1 OK\n");

            Console.WriteLine("?? Kører trin 2 test...");
            TestStep2();
            Console.WriteLine("? Trin 2 OK\n");

            Console.WriteLine("?? Kører trin 3 test...");
            TestStep3();
            Console.WriteLine("? Trin 3 OK\n");

            Console.WriteLine("?? Kører trin 4 test...");
            TestStep4();
            Console.WriteLine("? Trin 4 OK\n");

            Console.WriteLine("?? Kører trin 5 test...");
            TestStep5();
            Console.WriteLine("? Trin 5 OK\n");
        }

        private void TestStep1()
        {
            var dto = new Step1Dto
            {
                Name = "Test Produkt",
                Season = "Forår",
                DgaItemNo = "12345",
                CountryOfOrigin = "Danmark",
                Supplier = "Test Leverandør",
                Designer = "Test Designer",
                Description = "Beskrivelse",
                ColiSize = "12",
                ProductGroup = "Køkken"
            };

            int productId = _service.HandleStep1(dto);
            var product = _repo.GetProductById(productId);

            Console.WriteLine($"✅ Produkt oprettet med ID: {productId}");
            Console.WriteLine($"Navn: {product.Name}, Land: {product.CountryOfOrigin}, Leverandør: {product.Supplier}");

            if (product == null || product.Name != dto.Name)
                throw new Exception("Trin 1 fejlede: Produkt blev ikke gemt korrekt");
        }

        private void TestStep2()
        {
            var productId = _repo.GetAllDrafts()[0].ProductId;

            var dto = new Step2Dto
            {
                ProductId = productId,
                Answers = new Dictionary<string, bool>
                {
                    { "Fødevarekontaktmaterialer - FKM", true },
                    { "REACH", true }
                }
            };

            _service.SaveStep2Answers(dto);

            Console.WriteLine($"✅ Trin 2 besvaret for produkt ID: {productId}");
            foreach (var kvp in dto.Answers)
            {
                Console.WriteLine($" - {kvp.Key}: {(kvp.Value ? "✅ Ja" : "❌ Nej")}");
            }

            var triggered = _repo.GetTriggeredFields("Fødevarekontaktmaterialer - FKM", "true", 2);
            Console.WriteLine("🎯 Afhængige felter udløst:");
            foreach (var f in triggered)
                Console.WriteLine($" - {f.FieldName}");
        }

        private void TestStep3()
        {
            var fields = _repo.GetFieldDefinitionsByStep(3);
            if (fields.Count == 0)
                throw new Exception("Trin 3 fejlede: Ingen felter returneret");

            Console.WriteLine($"📦 Trin 3 har {fields.Count} felter:");
            foreach (var f in fields)
            {
                Console.WriteLine($" - {f.FieldName} ({f.Datatype}){(f.IsRequired ? " [Påkrævet]" : "")}");
            }
        }

        private void TestStep4()
        {
            var productId = _repo.GetAllDrafts()[0].ProductId;

            var dto = new Step4Dto
            {
                ProductId = productId,
                DgaColorGroupName = "Grøn",
                PantonePantone = "Pantone 123",
                DgaSalCatGroup = "1010 Notesblokke",
                DgaVendItemCodeCode = "5",
                Assorted = true,
                AdditionalInformation = "Ekstra info",
                GsmWeight = 180,
                BurningTimeHours = 12,
                AntidopingRegulation = false,
                Subcategory = "Testkategori",
                OtherInformation2 = "Flere detaljer",
                GsmWeight2 = 200
            };

            _service.SaveStep4Answers(dto);

            var updatedProduct = _repo.GetProductById(productId);
            Console.WriteLine("🎯 Step 4 værdier gemt:");
            Console.WriteLine($" - Farve: {updatedProduct.DgaColorGroupName}");
            Console.WriteLine($" - Pantone: {updatedProduct.PantonePantone}");
            Console.WriteLine($" - GsmWeight: {updatedProduct.GsmWeight}, Brændetid: {updatedProduct.BurningTimeHours}");
            Console.WriteLine($" - Subcategory: {updatedProduct.Subcategory}");

            if (updatedProduct.DgaColorGroupName != dto.DgaColorGroupName)
                throw new Exception("Step 4 fejlede: Farve ikke gemt korrekt");
        }

        private void TestStep5()
        {
            var productId = _repo.GetAllDrafts()[0].ProductId;

            var dto = new Step5Dto
            {
                ProductId = productId,
                IsFinalApproved = true,
                ApprovedBy = "QA-Manager",
                ApprovedDate = DateTime.UtcNow
            };

            _service.SaveStep5Answers(dto);

            Console.WriteLine("🎯 Step 5: Godkendelse gennemført");
        }
    }
}

