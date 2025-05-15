using ProduktFlow2.Core.Models;
using ProduktFlow2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktFlow2.Core.Controllers
{
    /// <summary>
    /// Responsible for managing the product creation process across different steps.
    /// Acts as a controller or coordinator between the service layer and higher-level logic (e.g., UI or orchestration layer).
    /// 
    /// Dependencies:
    /// - Depends on ProductService to delegate all business logic and data-handling responsibilities.
    /// - This class does not implement any logic itself but facilitates flow and input/output coordination.
    /// </summary>
    public class ProductProcessManager
    {
        private readonly ProductService _service;

        /// <summary>
        /// Initializes a new instance of the ProductProcessManager class with an injected ProductService.
        /// </summary>
        /// <param name="service">The ProductService instance used to perform the actual step processing logic.</param>
        public ProductProcessManager(ProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Starts the first step of the product creation process by forwarding data to the service.
        /// </summary>
        /// <param name="dto">The input data for step 1 (e.g., product name, type, etc.).</param>
        /// <returns>Returns the newly created product ID or process instance ID.</returns>
        public int StartStep1(Step1Dto dto)
        {
            return _service.HandleStep1(dto);
        }

        /// <summary>
        /// Retrieves the field definitions/questions required for step 2 of the product creation.
        /// </summary>
        /// <returns>A list of field definitions that the user needs to fill in during step 2.</returns>
        public List<FieldDefinition> StartStep2()
        {
            return _service.GetFieldsForStep(2);
        }

        /// <summary>
        /// Saves the answers provided by the user in step 2.
        /// Delegates the data handling and validation to the service layer.
        /// </summary>
        /// <param name="dto">The user input values for step 2 fields.</param>
        public void SaveStep2Answers(Step2Dto dto)
        {
            _service.SaveStep2Answers(dto);
        }

        /// <summary>
        /// Retrieves additional fields that should be shown based on a user's previous input.
        /// This supports field dependencies (e.g., if selecting X triggers additional questions).
        /// </summary>
        /// <param name="parentField">The name of the parent field that triggered the dependency.</param>
        /// <param name="triggerValue">The specific value in the parent field that causes the dependency.</param>
        /// <returns>A list of triggered field definitions for step 2.</returns>
        public List<FieldDefinition> GetTriggeredFields(string parentField, string triggerValue)
        {
            return _service.GetTriggeredFields(parentField, triggerValue, 2);
        }
    }
}
