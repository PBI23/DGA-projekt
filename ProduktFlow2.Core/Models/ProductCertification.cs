using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktFlow2.Core.Models
{
    /// <summary>
    /// Represents a certification or compliance record associated with a product.
    /// This could reflect sustainability, safety, or quality marks relevant to the product.
    /// 
    /// Responsibilities:
    /// - Stores metadata about a specific certification type linked to a product.
    /// - Used for validation, filtering, reporting, or compliance documentation.
    /// 
    /// Dependencies:
    /// - Standalone model that references Product via ProductId.
    /// - Typically used in conjunction with ProductService or during step handling.
    /// </summary>
    public class ProductCertification
    {
        /// <summary>
        /// Unique identifier for the certification record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key linking this certification to a specific product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// The type or name of the certification (e.g., "FSC", "OEKO-TEX").
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The current status of the certification (true = valid/applicable, false = not certified).
        /// </summary>
        public bool Status { get; set; }
    }
}
