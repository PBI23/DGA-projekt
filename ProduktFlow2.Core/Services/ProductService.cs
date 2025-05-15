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
        /// <param name="parentField">The field name that has a



        /*
        int id = _repo.AddProduct(product);

        List<FieldDefinition> fields = _repo.GetFieldDefinitionsByStep(2);
        foreach (FieldDefinition f in fields)
        {
            ProductCertification cert = new ProductCertification();
            cert.ProductId = id;
            cert.Type = f.FieldName;
            cert.Status = false;
            _repo.AddCertification(cert);
        }
        return id;
    }

    public List<FieldDefinition> GetFieldsForStep(int step)
    {
        return _repo.GetFieldDefinitionsByStep(step);
    }

    public void SaveCertifications(int productId, Dictionary<string, bool> answers)
    {
        foreach (KeyValuePair<string, bool> entry in answers)
        {
            _repo.UpdateCertification(productId, entry.Key, entry.Value);
        }
    }
    }*/
    }

