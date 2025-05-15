using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktFlow2.Core.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) for capturing user input during step 1 of the product creation process.
    /// This object is passed from the UI or API layer into the business/service layer.
    /// 
    /// Responsibilities:
    /// - Encapsulates input data for product initialization.
    /// - Acts as a lightweight, serializable contract for step 1 data exchange.
    /// 
    /// Dependencies:
    /// - Used by ProductProcessManager and ProductService during step 1 handling.
    /// </summary>
    public class Step1Dto
    {
        /// <summary>
        /// Name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The seasonal collection or launch context of the product.
        /// </summary>
        public string Season { get; set; }

        /// <summary>
        /// Internal or external DGA item number for the product.
        /// </summary>
        public string DgaItemNo { get; set; }

        /// <summary>
        /// The country where the product originates or is manufactured.
        /// </summary>
        public string CountryOfOrigin { get; set; }

        /// <summary>
        /// Supplier name or identifier.
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// Name of the product designer.
        /// </summary>
        public string Designer { get; set; }

        /// <summary>
        /// Free-text description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Packaging unit size or coli size.
        /// </summary>
        public string ColiSize { get; set; }

        /// <summary>
        /// The product group or category the item belongs to.
        /// </summary>
        public string ProductGroup { get; set; }
    }
}
