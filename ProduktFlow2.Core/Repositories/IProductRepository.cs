using ProduktFlow2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktFlow2.Core.Repositories
{
    /// <summary>
    /// Defines the contract for data access operations related to product entities and their metadata.
    /// Acts as the abstraction layer between the business logic (services) and the underlying data store (e.g., SQL database).
    /// 
    /// Responsibilities:
    /// - Provides CRUD operations for products and product-related configurations (e.g., certifications, field definitions).
    /// - Supports dependency injection for testing and modularity.
    /// 
    /// Dependencies:
    /// - Implementations typically depend on Entity Framework, Dapper, or similar ORM/data access frameworks.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Persists a new product entity and returns the generated product ID.
        /// </summary>
        /// <param name="product">The product object to be stored.</param>
        /// <returns>Newly created product ID.</returns>
        int AddProduct(Product product);

        /// <summary>
        /// Retrieves a product by its unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The product object if found; otherwise, null or an empty instance.</returns>
        Product GetProductById(int id);

        /// <summary>
        /// Returns all products that are currently marked as "Draft" (not yet completed or submitted).
        /// </summary>
        /// <returns>List of draft product objects.</returns>
        List<Product> GetAllDrafts();

        /// <summary>
        /// Adds a certification record to a specific product.
        /// </summary>
        /// <param name="cert">The certification object containing product ID, type, and status.</param>
        void AddCertification(ProductCertification cert);

        /// <summary>
        /// Updates the status of a certification for a given product and certification type.
        /// </summary>
        /// <param name="productId">The product's ID to update the certification for.</param>
        /// <param name="type">The certification type (e.g., "FSC").</param>
        /// <param name="status">The new status (true = certified, false = not certified).</param>
        void UpdateCertification(int productId, string type, bool status);

        /// <summary>
        /// Retrieves a list of field definitions (questions/fields) associated with a specific step number.
        /// </summary>
        /// <param name="step">The step number (e.g., Step 2).</param>
        /// <returns>A list of FieldDefinition objects.</returns>
        List<FieldDefinition> GetFieldDefinitionsByStep(int step);
    }
}
