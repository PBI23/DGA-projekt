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

        // STEP SAVE METHODS
        int SaveStep1(Step1Dto dto);
        void SaveStep2(Step2Dto dto);
        void SaveStep3(Step3Dto dto);
        void SaveStep4(Step4Dto dto);
        void SaveStep5(Step5Dto dto);

        // PRODUCT METHODS
        // Field definitions and dependencies (used in UI)
        List<FieldDefinition> GetFieldDefinitionsByStep(int step);
        List<FieldDefinition> GetTriggeredFields(string parentField, string triggerValue, int step);
        void UpdateProductStatus(int productId, string status);


        // For UI: listing and accessing existing products
        List<Product> GetAllDrafts();
        Product GetProductById(int id);
    }
}
