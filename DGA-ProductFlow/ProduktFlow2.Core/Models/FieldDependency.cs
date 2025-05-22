using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktFlow2.Core.Models
{
    /// <summary>
    /// Represents a dependency between two fields in a specific step of the product creation process.
    /// Used to determine when a child field should be displayed based on a parent field's value.
    /// 
    /// Responsibilities:
    /// - Defines conditional logic for dynamic field visibility and activation.
    /// - Enables reactive form behavior (e.g., showing extra questions when a certain answer is selected).
    /// 
    /// Dependencies:
    /// - This model is used in combination with field definitions and logic in the service layer.
    /// - Data may originate from configuration, database, or be hardcoded during prototyping.
    /// </summary>
    public class FieldDependency
    {
        /// <summary>
        /// The name of the parent field that triggers the dependency.
        /// </summary>
        public string ParentField { get; set; }

        /// <summary>
        /// The name of the child field that becomes visible or enabled when the trigger condition is met.
        /// </summary>
        public string ChildField { get; set; }

        /// <summary>
        /// The specific value of the parent field that activates the child field.
        /// </summary>
        public string TriggerValue { get; set; }

        /// <summary>
        /// The step number in which this dependency applies.
        /// Used to scope dependencies to relevant stages of the product creation process.
        /// </summary>
        public int Step { get; set; }
    }
}

