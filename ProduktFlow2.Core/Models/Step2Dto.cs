using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktFlow2.Core.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) for step 2 of the product creation process.
    /// Captures answers to dynamically generated field definitions (typically checkboxes or booleans).
    /// 
    /// Responsibilities:
    /// - Transports user responses from step 2 to the service layer.
    /// - Links the answers to a specific product via ProductId.
    /// 
    /// Dependencies:
    /// - Used by ProductProcessManager and ProductService to persist and process step 2 input.
    /// </summary>
    public class Step2Dto
    {
        /// <summary>
        /// The ID of the product these answers belong to.
        /// Ensures that the input is associated with the correct product instance in progress.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// A dictionary mapping field names to boolean values.
        /// Used for storing answers to yes/no fields, such as certifications or feature flags.
        /// Key = field name (string), Value = selected answer (true/false).
        /// </summary>
        public Dictionary<string, bool> Answers { get; set; }
    }
}
