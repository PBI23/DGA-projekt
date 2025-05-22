using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktFlow2.Core.Models
{
    /// <summary>
    /// Represents a configurable input field used in one of the product creation steps.
    /// Contains metadata such as field type, step association, and conditional dependencies.
    /// 
    /// Responsibilities:
    /// - Acts as a model/data contract for dynamic form rendering and validation logic.
    /// - Used to describe what input is needed from the user and under what conditions.
    /// 
    /// Dependencies:
    /// - This class is purely a data container with no service or database dependencies.
    /// - Used by the service layer (e.g., ProductService) to generate forms dynamically.
    /// </summary>
    public class FieldDefinition
    {
        /// <summary>
        /// Unique identifier for the field definition.
        /// </summary>
        public int FieldDefinitionID { get; set; }

        /// <summary>
        /// The internal name of the field, used to bind data and identify inputs.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// The step number in which this field appears (e.g., Step 2, Step 3).
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// Indicates whether the field is required for form submission.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// (Optional) The name of another field this one depends on (used for dynamic triggering).
        /// </summary>
        public string DependsOn { get; set; }

        /// <summary>
        /// The expected data type of the input, e.g., "Text", "Boolean", or "Dropdown".
        /// This is used to determine the UI component and validation.
        /// </summary>
        public string Datatype { get; set; }

        /// <summary>
        /// A tag used to group related fields together in the UI (e.g., "Basic", "Certification").
        /// Helps organize complex forms into logical sections.
        /// </summary>
        public string GroupTag { get; set; }
    }
}
