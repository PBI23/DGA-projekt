using ProduktFlow2.Core.Models;
using ProduktFlow2.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktFlow2.Core.Services
{
    /// <summary>
    /// Handles the business logic related to product creation, field handling, and certifications.
    /// Acts as the core service layer between the controller and repository layers.
    /// 
    /// Responsibilities:
    /// - Processes incoming data from DTOs.
    /// - Coordinates logic and data updates by interacting with the repository.
    /// - Initializes default certification states and handles field dependencies.
    /// 
    /// Dependencies:
    /// - Depends on ProductRepositoryDummy (or an IProductRepository implementation in the future).
    /// </summary>
    public class ProductService
    {
        private readonly ProductRepositoryDummy _repo;

        /// <summary>
        /// Initializes the service with a repository implementation.
        /// </summary>
        /// <param name="repo">Injected repository to manage product and field data.</param>
        public ProductService(ProductRepositoryDummy repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Handles logic for step 1 of the product creation process.
        /// Creates a new product from user input and initializes all boolean certifications as false.
        /// </summary>
        /// <param name="dto">Step1Dto containing user-provided product data.</param>
        /// <returns>The ID of the newly created product.</returns>
        public int HandleStep1(Step1Dto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Season = dto.Season,
                DgaItemNo = dto.DgaItemNo,
                CountryOfOrigin = dto.CountryOfOrigin,
                Supplier = dto.Supplier,
                Designer = dto.Designer,
                Description = dto.Description,
                ColiSize = dto.ColiSize,
                ProductGroup = dto.ProductGroup,
                Status = "Draft"
            };

            // Save product to repository
            int productId = _repo.AddProduct(product);

            // Initialize all Boolean-type certifications as unchecked (false)
            var boolFields = _repo.GetFieldDefinitionsByStep(2)
                .Where(f => f.Datatype == "Boolean");

            foreach (var field in boolFields)
            {
                _repo.AddCertification(new ProductCertification
                {
                    ProductId = productId,
                    Type = field.FieldName,
                    Status = false
                });
            }

            return productId;
        }

        /// <summary>
        /// Retrieves dropdown options for a specific field name.
        /// Used for dynamic form population in the UI.
        /// </summary>
        /// <param name="fieldName">The name of the field needing dropdown values.</param>
        /// <returns>A list of string options or an empty list if not supported.</returns>
        public List<string> GetDropdownOptions(string fieldName)
        {
            if (fieldName == "ProductGroup")
                return _repo.GetProductGroupOptions();

            return new List<string>(); // Can be extended for other fields
        }

        /// <summary>
        /// Saves user responses to certification questions (Boolean fields) from step 2.
        /// Updates or creates certification records accordingly.
        /// </summary>
        /// <param name="dto">Step2Dto containing product ID and user responses.</param>
        public void SaveStep2Answers(Step2Dto dto)
        {
            foreach (var kvp in dto.Answers)
            {
                _repo.UpdateCertification(dto.ProductId, kvp.Key, kvp.Value);
            }
        }

        public void SaveStep3Answers(Step3Dto dto)
        {
            var product = _repo.GetProductById(dto.ProductId);

            // Her gemmer vi blot felterne direkte i produktet for nu
            //product.Status = "Step3Completed"; 
            // eller en anden logik

        }


        /// <summary>
        /// Saves data from optional Step 4 fields into the product object (simplified approach).
        /// </summary>
        /// <param name="dto">Step4Dto containing all optional step 4 answers.</param>
        public void SaveStep4Answers(Step4Dto dto)
        {
            var product = _repo.GetProductById(dto.ProductId);

            product.DgaColorGroupName = dto.DgaColorGroupName;
            product.DgaSalCatGroup = dto.DgaSalCatGroup;
            product.PantonePantone = dto.PantonePantone;
            product.DgaVendItemCodeCode = dto.DgaVendItemCodeCode;
            product.Assorted = dto.Assorted;
            product.AdditionalInformation = dto.AdditionalInformation;
            product.Subcategory = dto.Subcategory;
            product.GsmWeight = (int?)dto.GsmWeight;
            product.GsmWeight2 = (int?)dto.GsmWeight2;
            product.BurningTimeHours = (int?)dto.BurningTimeHours;
            product.AntidopingRegulation = dto.AntidopingRegulation;
            product.OtherInformation2 = dto.OtherInformation2;
        }




        /// <summary>
        /// Retrieves all field definitions for a given step number.
        /// </summary>
        /// <param name="step">The step number (e.g., 2 or 3).</param>
        /// <returns>A list of field definitions associated with the step.</returns>
        public List<FieldDefinition> GetFieldsForStep(int step)
        {
            return _repo.GetFieldDefinitionsByStep(step);
        }

        /// <summary>
        /// Retrieves fields that are conditionally triggered based on user input.
        /// Used to support dynamic forms (e.g., dependencies like FKM → subfields).
        /// </summary>
        /// <param name="parentField">The field name that has a dependency relationship.</param>
        /// <param name="triggerValue">The value that triggers one or more dependent fields.</param>
        /// <param name="step">The step number in which the dependency applies.</param>


        public List<FieldDefinition> GetTriggeredFields(string parentField, string triggerValue, int step)
        {
            return _repo.GetTriggeredFields(parentField, triggerValue, step);
        }

        public List<Product> GetAllDrafts()
        {
            return _repo.GetAllDrafts();
        }
        // For upstart to show all in draft or un-complete
        public Product GetProductById(int id)
        {
            return _repo.GetProductById(id);
        }


        public void UpdateStatus(int productId, string newStatus)
        {
            var product = _repo.GetProductById(productId);
            product.Status = newStatus;
        }



    }
}


