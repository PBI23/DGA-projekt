using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktFlow2.Core.Models
{
    /// <summary>
    /// Represents the core business entity for a product being created or managed in the system.
    /// Holds all general information about a product, such as identifiers, origin, and descriptive fields.
    /// 
    /// Responsibilities:
    /// - Acts as the primary data structure for storing and transferring product-level data across the system.
    /// - Used throughout the flow, from input forms to database persistence and validation logic.
    /// 
    /// Dependencies:
    /// - This is a plain data model and does not rely on any external services or logic.
    /// - Typically populated by DTOs or from database entities.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique identifier for the product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The seasonal collection or release associated with the product.
        /// </summary>
        public string Season { get; set; }

        /// <summary>
        /// Internal or external item number (DGA system reference).
        /// </summary>
        public string DgaItemNo { get; set; }

        /// <summary>
        /// Country where the product was manufactured or originated.
        /// </summary>
        public string CountryOfOrigin { get; set; }

        /// <summary>
        /// Supplier responsible for providing the product.
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// Name of the designer who created the product.
        /// </summary>
        public string Designer { get; set; }

        /// <summary>
        /// A descriptive text or summary of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Packaging or coli size (e.g., how the product is shipped or boxed).
        /// </summary>
        public string ColiSize { get; set; }

        /// <summary>
        /// Logical grouping or category the product belongs to (e.g., "Apparel", "Accessories").
        /// </summary>
        public string ProductGroup { get; set; }

        /// <summary>
        /// Current status of the product (e.g., "Draft", "Submitted", "Approved").
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Initializes a new product instance with all string properties set to empty.
        /// This prevents null reference issues when handling form data.
        /// </summary>
        /// 
        // Step 4 optional fields
        public string DgaColorGroupName { get; set; }
        public string DgaSalCatGroup { get; set; }
        public string PantonePantone { get; set; }
        public string DgaVendItemCodeCode { get; set; }
        public bool? Assorted { get; set; }
        public string AdditionalInformation { get; set; }
        public string Subcategory { get; set; }
        public int? GsmWeight { get; set; }
        public int? GsmWeight2 { get; set; }
        public int? BurningTimeHours { get; set; }
        public bool? AntidopingRegulation { get; set; }
        public string OtherInformation2 { get; set; }
        public Product()
        {
            Name = string.Empty;
            Season = string.Empty;
            DgaItemNo = string.Empty;
            CountryOfOrigin = string.Empty;
            Supplier = string.Empty;
            Designer = string.Empty;
            Description = string.Empty;
            ColiSize = string.Empty;
            ProductGroup = string.Empty;
            Status = string.Empty;
        }
    }
}
