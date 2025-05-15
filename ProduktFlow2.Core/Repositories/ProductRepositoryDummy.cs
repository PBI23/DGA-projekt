using ProduktFlow2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktFlow2.Core.Repositories
{
    /// <summary>
    /// Dummy implementation of the IProductRepository interface for testing and prototyping purposes.
    /// Stores all data in-memory using local collections instead of a database.
    /// 
    /// Responsibilities:
    /// - Simulates CRUD operations for products, certifications, and dynamic field definitions.
    /// - Serves as a mock repository layer for local testing without external dependencies.
    /// 
    /// Dependencies:
    /// - None (pure in-memory logic).
    /// - Used in early development and unit testing when real database access is not required.
    /// </summary>
    public class ProductRepositoryDummy : IProductRepository
    {
        // In-memory list of products
        private List<Product> _products = new List<Product>();

        // In-memory list of certifications linked to products
        private List<ProductCertification> _certifications = new List<ProductCertification>();

        // Static list of predefined fields used in each step of the product process
        private List<FieldDefinition> _fields = new List<FieldDefinition>
        {
           // Trin 1 felter (hvis ikke allerede tilføjet)
    new FieldDefinition { FieldName = "Name", Step = 1, IsRequired = true, Datatype = "Text" },
    new FieldDefinition { FieldName = "Season", Step = 1, IsRequired = false, Datatype = "Text" },
    new FieldDefinition { FieldName = "DgaItemNo", Step = 1, IsRequired = true, Datatype = "Text" },
    new FieldDefinition { FieldName = "CountryOfOrigin", Step = 1, IsRequired = true, Datatype = "Text" },
    new FieldDefinition { FieldName = "Supplier", Step = 1, IsRequired = true, Datatype = "Text" },
    new FieldDefinition { FieldName = "Designer", Step = 1, IsRequired = true, Datatype = "Text" },
    new FieldDefinition { FieldName = "Description", Step = 1, IsRequired = false, Datatype = "Text" },
    new FieldDefinition { FieldName = "ColiSize", Step = 1, IsRequired = false, Datatype = "Text" },
    new FieldDefinition { FieldName = "ProductGroup", Step = 1, IsRequired = true, Datatype = "Dropdown" },

    // Trin 2 Certification
    new FieldDefinition { FieldName = "General Product Safety Regulation (EU) 2023/988", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "Certification" },
    new FieldDefinition { FieldName = "Ecodesign for Sustainable Products Regulation (EU) 2024/1781", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "Certification" },
    new FieldDefinition { FieldName = "Waste Framework Directive 2008/98/EC", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "Certification" },
    new FieldDefinition { FieldName = "CBAM Carbon Border Adjustment Mechanism Regulation (EU) 2023/956", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "Certification" },
    new FieldDefinition { FieldName = "Fødevarekontaktmaterialer - FKM", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "Certification" },
    new FieldDefinition { FieldName = "Børn", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "Certification" },
    new FieldDefinition { FieldName = "REACH", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "Certification" },
    new FieldDefinition { FieldName = "Tekstiler", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "Certification" },
    new FieldDefinition { FieldName = "Elektronik", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "Certification" },
    new FieldDefinition { FieldName = "Træ", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "Certification" },
    new FieldDefinition { FieldName = "Kosmetik", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "Certification" },

    // Trin 2 – FKM-afhængige
    new FieldDefinition { FieldName = "Food Contact Materials Framework Regulation (EC) 1935/2004", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "FoodContactMaterial", DependsOn = "Fødevarekontaktmaterialer - FKM" },
    new FieldDefinition { FieldName = "Good Manufacturing Practice for FCM Regulation (EC) 2023/2006", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "FoodContactMaterial", DependsOn = "Fødevarekontaktmaterialer - FKM" },
    new FieldDefinition { FieldName = "Plastic FCM Regulation (EU) 10/2011", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "FoodContactMaterial", DependsOn = "Fødevarekontaktmaterialer - FKM" },
    new FieldDefinition { FieldName = "Restriction of Use of Certain Epoxy Derivatives in FCM Regulation (EC) 1895/2005", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "FoodContactMaterial", DependsOn = "Fødevarekontaktmaterialer - FKM" },
    new FieldDefinition { FieldName = "Recycled Plastic FCM Regulation (EU) 2022/1616", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "FoodContactMaterial", DependsOn = "Fødevarekontaktmaterialer - FKM" },
    new FieldDefinition { FieldName = "Ceramic FCM Directive 84/500/EEC", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "FoodContactMaterial", DependsOn = "Fødevarekontaktmaterialer - FKM" },
    new FieldDefinition { FieldName = "Active and Intelligent FCM Regulation (EC) 450/2009", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "FoodContactMaterial", DependsOn = "Fødevarekontaktmaterialer - FKM" },
    new FieldDefinition { FieldName = "Regenerated Cellulose Film FCM Directive 2007/42/EC", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "FoodContactMaterial", DependsOn = "Fødevarekontaktmaterialer - FKM" },
    new FieldDefinition { FieldName = "Use of Bisphenol A in FCM Regulation (EU) 2024/3190", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "FoodContactMaterial", DependsOn = "Fødevarekontaktmaterialer - FKM" },
    new FieldDefinition { FieldName = "Use of Bisphenol A in Varnishes and Coatings Regulation (EU) 2018/213", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "FoodContactMaterial", DependsOn = "Fødevarekontaktmaterialer - FKM" },
    new FieldDefinition { FieldName = "Polyamide and Melamine Plastic Kitchenware from China or Hong Kong SAR (China) Regulation (EU) 284/2011", Step = 2, IsRequired = false, Datatype = "Boolean", GroupTag = "FoodContactMaterial", DependsOn = "Fødevarekontaktmaterialer - FKM" },
    // Trin 3 - Grunddata og mål
    new FieldDefinition { FieldName = "Supplier product no.", Step = 3, IsRequired = true, Datatype = "Number", GroupTag = "General" },
    new FieldDefinition { FieldName = "Customer clearance no.", Step = 3, IsRequired = true, Datatype = "Number", GroupTag = "General" },
    new FieldDefinition { FieldName = "Customer clearance %", Step = 3, IsRequired = true, Datatype = "Number", GroupTag = "General" },
    new FieldDefinition { FieldName = "Cost price", Step = 3, IsRequired = true, Datatype = "Number", GroupTag = "General" },
    new FieldDefinition { FieldName = "Inner carton", Step = 3, IsRequired = false, Datatype = "Number", GroupTag = "General" },
    new FieldDefinition { FieldName = "Outer carton", Step = 3, IsRequired = false, Datatype = "Barcode", GroupTag = "General" },

    new FieldDefinition { FieldName = "Gross weight", Step = 3, IsRequired = false, Datatype = "Number", GroupTag = "Dimensions" },
    new FieldDefinition { FieldName = "Packing height (cm)", Step = 3, IsRequired = false, Datatype = "Number", GroupTag = "Dimensions" },
    new FieldDefinition { FieldName = "Packing width/length (cm)", Step = 3, IsRequired = false, Datatype = "Number", GroupTag = "Dimensions" },
    new FieldDefinition { FieldName = "Packing depth (cm)", Step = 3, IsRequired = false, Datatype = "Number", GroupTag = "Dimensions" },
    new FieldDefinition { FieldName = "KI height (cm)", Step = 3, IsRequired = false, Datatype = "Number", GroupTag = "Dimensions" },
    new FieldDefinition { FieldName = "KY height (cm)", Step = 3, IsRequired = false, Datatype = "Number", GroupTag = "Dimensions" },
    new FieldDefinition { FieldName = "KY width/length (cm)", Step = 3, IsRequired = false, Datatype = "Number", GroupTag = "Dimensions" },

    new FieldDefinition { FieldName = "Dishwasher", Step = 3, IsRequired = false, Datatype = "Boolean", GroupTag = "Usage" },
    new FieldDefinition { FieldName = "Microwave", Step = 3, IsRequired = false, Datatype = "Boolean", GroupTag = "Usage" },
    new FieldDefinition { FieldName = "Svanemærket", Step = 3, IsRequired = false, Datatype = "Boolean", GroupTag = "Sustainability" },
    new FieldDefinition { FieldName = "Grüner Punkt", Step = 3, IsRequired = false, Datatype = "Boolean", GroupTag = "Sustainability" },
    new FieldDefinition { FieldName = "CE", Step = 3, IsRequired = false, Datatype = "Boolean", GroupTag = "Certification" },
    new FieldDefinition { FieldName = "FSC100", Step = 3, IsRequired = false, Datatype = "Boolean", GroupTag = "Sustainability" },
    new FieldDefinition { FieldName = "FSCMIX70", Step = 3, IsRequired = false, Datatype = "Boolean", GroupTag = "Sustainability" },

    new FieldDefinition { FieldName = "ABC", Step = 3, IsRequired = true, Datatype = "Dropdown", GroupTag = "Classification" },
    new FieldDefinition { FieldName = "Product logo", Step = 3, IsRequired = true, Datatype = "Dropdown", GroupTag = "Branding" },
    new FieldDefinition { FieldName = "Hangtags & Stickers", Step = 3, IsRequired = true, Datatype = "Dropdown", GroupTag = "Packaging" },
    new FieldDefinition { FieldName = "Series", Step = 3, IsRequired = true, Datatype = "Dropdown", GroupTag = "Series" },

    // Trin 4  Optional questions
    new FieldDefinition { FieldName = "DGA_Color_GroupName", Step = 4, IsRequired = false, Datatype = "Dropdown", GroupTag = "Optional" },
    new FieldDefinition { FieldName = "DGA_SalCat_Group", Step = 4, IsRequired = false, Datatype = "Dropdown", GroupTag = "Optional" },
    new FieldDefinition { FieldName = "Pantone_Pantone", Step = 4, IsRequired = false, Datatype = "Dropdown", GroupTag = "Optional" },
    new FieldDefinition { FieldName = "DGA_VendItemCode_Code", Step = 4, IsRequired = false, Datatype = "Dropdown", GroupTag = "Optional" },

    new FieldDefinition { FieldName = "Assorted", Step = 4, IsRequired = false, Datatype = "Boolean", GroupTag = "Optional" },
    new FieldDefinition { FieldName = "AdditionalInformation", Step = 4, IsRequired = false, Datatype = "Text", GroupTag = "Optional" },
    new FieldDefinition { FieldName = "GsmWeight", Step = 4, IsRequired = false, Datatype = "Number", GroupTag = "Optional" },
    new FieldDefinition { FieldName = "BurningTimeHours", Step = 4, IsRequired = false, Datatype = "Number", GroupTag = "Optional" },
    new FieldDefinition { FieldName = "AntidopingRegulation", Step = 4, IsRequired = false, Datatype = "Boolean", GroupTag = "Optional" },
    new FieldDefinition { FieldName = "Subcategory", Step = 4, IsRequired = false, Datatype = "Text", GroupTag = "Optional" },
    new FieldDefinition { FieldName = "OtherInformation2", Step = 4, IsRequired = false, Datatype = "Text", GroupTag = "Optional" },
    new FieldDefinition { FieldName = "GsmWeight2", Step = 4, IsRequired = false, Datatype = "Number", GroupTag = "Optional" }


};
        /// <summary>
        /// Returns predefined dropdown options for specific field names.
        /// </summary>
        public List<string> GetDropdownOptions(string fieldName)
        {
            return fieldName switch
            {
                "ProductGroup" => new List<string> { "Køkken", "Jul", "Tekstil", "Outdoor" },
                "ABC" => new List<string> { "A", "B", "C" },
                "Product logo" => new List<string> { "C2G", "Customer", "DGA", "JULIUS" },
                "Hangtags & Stickers" => new List<string> { "Backcard", "Belt 100mm", "Blistercard", "Craftbox" },
                "Series" => new List<string> { "100% christmas", "A christmas story", "Acorn" },
                "DGA_Color_GroupName" => new List<string> { "Amber", "Beige", "Blå", "Bordeaux", "Bronze", "Brun", "Champagne", "Creme", "Flerfarvet", "Frosted", "Gammel rosa", "Grøn", "Grå", "Gul", "Guld", "Hvid", "Kobber", "Lilla", "Messing", "Natur", "Orange", "Pink", "Rosa", "Rust", "Rød", "Sort", "Sølv", "Transparent", "Zink" },
                "DGA_SalCat_Group" => new List<string> { "1002 Sjovt tilbehør", "1010 Notesblokke", "1011 Gaveposer", "1015 Gavemærker" /* osv. forkortet */ },
                "Pantone_Pantone" => new List<string> { "Black", "Rød Norsk flag", "Champagne", "Creme", "White", "Yellow", "Melon", "Old rose", "Dusty Green", "Grey", "Green", "Orange" },
                "DGA_VendItemCode_Code" => Enumerable.Range(2, 44).Select(i => i.ToString()).ToList(),
                _ => new List<string>()
            };
        }

        /// <summary>
        /// Returns a static list of product group options.
        /// </summary>
        public List<string> GetProductGroupOptions()
        {
            return new List<string> { "Køkken", "Jul", "Tekstil", "Outdoor" };
        }
        // Defines dependencies between fields, used to trigger additional questions in forms
        private List<FieldDependency> _dependencies = new List<FieldDependency>
        {
            new FieldDependency
    {
        ParentField = "Fødevarekontaktmaterialer - FKM",
        TriggerValue = "true",
        Step = 2,
        ChildField = "Food Contact Materials Framework Regulation (EC) 1935/2004"
    },
    // ... du kan fortsætte med én linje for hver af de 11 FKM-felter ...
    new FieldDependency { ParentField = "Fødevarekontaktmaterialer - FKM", TriggerValue = "true", Step = 2, ChildField = "Good Manufacturing Practice for FCM Regulation (EC) 2023/2006" },
    new FieldDependency { ParentField = "Fødevarekontaktmaterialer - FKM", TriggerValue = "true", Step = 2, ChildField = "Plastic FCM Regulation (EU) 10/2011" },
    new FieldDependency { ParentField = "Fødevarekontaktmaterialer - FKM", TriggerValue = "true", Step = 2, ChildField = "Restriction of Use of Certain Epoxy Derivatives in FCM Regulation (EC) 1895/2005" },
    new FieldDependency { ParentField = "Fødevarekontaktmaterialer - FKM", TriggerValue = "true", Step = 2, ChildField = "Recycled Plastic FCM Regulation (EU) 2022/1616" },
    new FieldDependency { ParentField = "Fødevarekontaktmaterialer - FKM", TriggerValue = "true", Step = 2, ChildField = "Ceramic FCM Directive 84/500/EEC" },
    new FieldDependency { ParentField = "Fødevarekontaktmaterialer - FKM", TriggerValue = "true", Step = 2, ChildField = "Active and Intelligent FCM Regulation (EC) 450/2009" },
    new FieldDependency { ParentField = "Fødevarekontaktmaterialer - FKM", TriggerValue = "true", Step = 2, ChildField = "Regenerated Cellulose Film FCM Directive 2007/42/EC" },
    new FieldDependency { ParentField = "Fødevarekontaktmaterialer - FKM", TriggerValue = "true", Step = 2, ChildField = "Use of Bisphenol A in FCM Regulation (EU) 2024/3190" },
    new FieldDependency { ParentField = "Fødevarekontaktmaterialer - FKM", TriggerValue = "true", Step = 2, ChildField = "Use of Bisphenol A in Varnishes and Coatings Regulation (EU) 2018/213" },
    new FieldDependency { ParentField = "Fødevarekontaktmaterialer - FKM", TriggerValue = "true", Step = 2, ChildField = "Polyamide and Melamine Plastic Kitchenware from China or Hong Kong SAR (China) Regulation (EU) 284/2011" }
};

        /// <summary>
        /// Adds a new product to the in-memory list and returns its generated ID.
        /// </summary>
        public int AddProduct(Product product)
        {
            product.ProductId = _products.Count + 1;
            product.Status = "Draft";
            _products.Add(product);
            return product.ProductId;
        }

        /// <summary>
        /// Retrieves a product by its ID. Throws exception if not found.
        /// </summary>
        public Product GetProductById(int id)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
                throw new KeyNotFoundException($"Produkt med ID {id} blev ikke fundet.");
            return product;
        }

        /// <summary>
        /// Returns all products currently marked as "Draft".
        /// </summary>
        public List<Product> GetAllDrafts()
        {
            return _products.Where(p => p.Status == "Draft").ToList();
        }


        /// <summary>
        /// Adds a new certification record to the in-memory list.
        /// </summary>
        public void AddCertification(ProductCertification cert)
        {
            _certifications.Add(cert);
        }

        /// <summary>
        /// Updates an existing certification for a product, or adds it if it doesn't exist.
        /// </summary>
        public void UpdateCertification(int productId, string type, bool status)
        {
            var existing = _certifications
                .FirstOrDefault(c => c.ProductId == productId && c.Type == type);

            if (existing != null)
            {
                existing.Status = status;
            }
            else
            {
                _certifications.Add(new ProductCertification
                {
                    ProductId = productId,
                    Type = type,
                    Status = status
                });
            }
        }

        /// <summary>
        /// Returns a list of field definitions for a given step number.
        /// </summary>
        public List<FieldDefinition> GetFieldDefinitionsByStep(int step)
        {
            return _fields.Where(f => f.Step == step).ToList();
        }

        /// <summary>
        /// Returns fields that should be triggered based on a parent field's value.
        /// Supports dynamic form behavior in step 2.
        /// </summary>
        public List<FieldDefinition> GetTriggeredFields(string parentField, string triggerValue, int step)
        {
            var triggered = _dependencies
                .Where(d => d.ParentField == parentField && d.TriggerValue == triggerValue && d.Step == step)
                .Select(d => d.ChildField)
                .ToList();

            return _fields.Where(f => triggered.Contains(f.FieldName)).ToList();
        }
    }
}
