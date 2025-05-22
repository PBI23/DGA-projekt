using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ProduktFlow2.Core.Models;
using ProduktFlow2.Core.Helpers;
using System.Collections.Generic;

namespace ProduktFlow2.Core.Repositories
{
    public class ProductRepositoryDb : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepositoryDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int SaveStep1(Step1Dto dto)
        {
            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spSaveStep1", conn);

            SqlHelper.AddParameter(cmd, "@Name", dto.Name);
            SqlHelper.AddParameter(cmd, "@Season", dto.Season);
            SqlHelper.AddParameter(cmd, "@DgaItemNo", dto.DgaItemNo);
            SqlHelper.AddParameter(cmd, "@CountryOfOrigin", dto.CountryOfOrigin);
            SqlHelper.AddParameter(cmd, "@Supplier", dto.Supplier);
            SqlHelper.AddParameter(cmd, "@Designer", dto.Designer);
            SqlHelper.AddParameter(cmd, "@Description", dto.Description);
            SqlHelper.AddParameter(cmd, "@ColiSize", dto.ColiSize);
            SqlHelper.AddParameter(cmd, "@ProductGroup", dto.ProductGroup);

            var outputId = SqlHelper.AddOutputParameter(cmd, "@ProductId", SqlDbType.Int);

            cmd.ExecuteNonQuery();

            return (int)outputId.Value;
        }

        public void SaveStep2(Step2Dto dto)
        {
            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spSaveStep2", conn);

            SqlHelper.AddParameter(cmd, "@ProductId", dto.ProductId);
            SqlHelper.AddParameter(cmd, "@JsonData", System.Text.Json.JsonSerializer.Serialize(dto.Answers));

            cmd.ExecuteNonQuery();
        }

        public void SaveStep3(Step3Dto dto)
        {
            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spSaveStep3", conn);

            SqlHelper.AddSimpleParameter(cmd, "@ProductId", dto.ProductId);

            // General data
            SqlHelper.AddSimpleParameter(cmd, "@SupplierProductNo", dto.SupplierProductNo);
            SqlHelper.AddSimpleParameter(cmd, "@CustomerClearanceNo", dto.CustomerClearanceNo);
            SqlHelper.AddSimpleParameter(cmd, "@CustomerClearancePercent", dto.CustomerClearancePercent);
            SqlHelper.AddSimpleParameter(cmd, "@CostPrice", dto.CostPrice);

            // Packaging
            SqlHelper.AddSimpleParameter(cmd, "@InnerCarton", dto.InnerCarton);
            SqlHelper.AddSimpleParameter(cmd, "@OuterCarton", dto.OuterCarton);

            // Dimensions
            SqlHelper.AddSimpleParameter(cmd, "@GrossWeight", dto.GrossWeight);
            SqlHelper.AddSimpleParameter(cmd, "@PackingHeight", dto.PackingHeight);
            SqlHelper.AddSimpleParameter(cmd, "@PackingWidthLength", dto.PackingWidthLength);
            SqlHelper.AddSimpleParameter(cmd, "@PackingDepth", dto.PackingDepth);

            // Usage
            SqlHelper.AddSimpleParameter(cmd, "@DishwasherSafe", dto.DishwasherSafe);
            SqlHelper.AddSimpleParameter(cmd, "@MicrowaveSafe", dto.MicrowaveSafe);

            // Sustainability
            SqlHelper.AddSimpleParameter(cmd, "@Svanemaerket", dto.Svanemaerket);
            SqlHelper.AddSimpleParameter(cmd, "@GrunerPunkt", dto.GrunerPunkt);
            SqlHelper.AddSimpleParameter(cmd, "@FSC100", dto.FSC100);
            SqlHelper.AddSimpleParameter(cmd, "@FSCMix70", dto.FSCMix70);

            // Classification / Branding
            SqlHelper.AddSimpleParameter(cmd, "@ABC", dto.ABC);
            SqlHelper.AddSimpleParameter(cmd, "@ProductLogo", dto.ProductLogo);
            SqlHelper.AddSimpleParameter(cmd, "@HangtagsAndStickers", dto.HangtagsAndStickers);
            SqlHelper.AddSimpleParameter(cmd, "@Series", dto.Series);

            cmd.ExecuteNonQuery();
        }

        public void SaveStep4(Step4Dto dto)
        {
            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spSaveStep4", conn);

            SqlHelper.AddParameter(cmd, "@ProductId", dto.ProductId);
            SqlHelper.AddParameter(cmd, "@DgaColorGroupName", dto.DgaColorGroupName);
            SqlHelper.AddParameter(cmd, "@PantonePantone", dto.PantonePantone);
            SqlHelper.AddParameter(cmd, "@DgaSalCatGroup", dto.DgaSalCatGroup);
            SqlHelper.AddParameter(cmd, "@DgaVendItemCodeCode", dto.DgaVendItemCodeCode);
            SqlHelper.AddParameter(cmd, "@Assorted", dto.Assorted);
            SqlHelper.AddParameter(cmd, "@AdditionalInformation", dto.AdditionalInformation);
            SqlHelper.AddParameter(cmd, "@GsmWeight", dto.GsmWeight);
            SqlHelper.AddParameter(cmd, "@BurningTimeHours", dto.BurningTimeHours);
            SqlHelper.AddParameter(cmd, "@AntidopingRegulation", dto.AntidopingRegulation);
            SqlHelper.AddParameter(cmd, "@Subcategory", dto.Subcategory);
            SqlHelper.AddParameter(cmd, "@OtherInformation2", dto.OtherInformation2);
            SqlHelper.AddParameter(cmd, "@GsmWeight2", dto.GsmWeight2);

            cmd.ExecuteNonQuery();
        }

        public void SaveStep5(Step5Dto dto)
        {
            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spApproveProduct", conn);

            SqlHelper.AddSimpleParameter(cmd, "@ProductId", dto.ProductId);
            SqlHelper.AddSimpleParameter(cmd, "@IsApproved", dto.IsApproved);
            SqlHelper.AddSimpleParameter(cmd, "@ApprovedBy", dto.ApprovedBy ?? (object)DBNull.Value);
            SqlHelper.AddSimpleParameter(cmd, "@ApprovedAt", dto.ApprovedAt);

            cmd.ExecuteNonQuery();
        }

        public List<FieldDefinition> GetFieldDefinitionsByStep(int step)
        {
            var fields = new List<FieldDefinition>();

            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spGetFieldsForStep", conn);
            SqlHelper.AddSimpleParameter(cmd, "@Step", step);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var field = new FieldDefinition
                {
                    FieldDefinitionID = reader.GetInt32(reader.GetOrdinal("FieldDefinitionId")),
                    FieldName = reader.GetString(reader.GetOrdinal("FieldName")),
                    Step = reader.GetInt32(reader.GetOrdinal("Step")),
                    IsRequired = reader.GetBoolean(reader.GetOrdinal("IsRequired")),
                    DependsOn = reader.IsDBNull(reader.GetOrdinal("DependsOn")) ? null : reader.GetString(reader.GetOrdinal("DependsOn")),
                    Datatype = reader.GetString(reader.GetOrdinal("Datatype")),
                    GroupTag = reader.IsDBNull(reader.GetOrdinal("GroupTag")) ? null : reader.GetString(reader.GetOrdinal("GroupTag"))
                };

                fields.Add(field);
            }

            return fields;
        }

        public List<FieldDefinition> GetTriggeredFields(string parentField, string triggerValue, int step)
        {
            var triggeredFields = new List<FieldDefinition>();

            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spGetDependentFields", conn);

            SqlHelper.AddSimpleParameter(cmd, "@ParentField", parentField);
            SqlHelper.AddSimpleParameter(cmd, "@TriggerValue", triggerValue);
            SqlHelper.AddSimpleParameter(cmd, "@Step", step);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var field = new FieldDefinition
                {
                    FieldDefinitionID = reader.GetInt32(reader.GetOrdinal("FieldDefinitionId")),
                    FieldName = reader.GetString(reader.GetOrdinal("FieldName")),
                    Step = reader.GetInt32(reader.GetOrdinal("Step")),
                    IsRequired = reader.GetBoolean(reader.GetOrdinal("IsRequired")),
                    DependsOn = reader.IsDBNull(reader.GetOrdinal("DependsOn")) ? null : reader.GetString(reader.GetOrdinal("DependsOn")),
                    Datatype = reader.GetString(reader.GetOrdinal("Datatype")),
                    GroupTag = reader.IsDBNull(reader.GetOrdinal("GroupTag")) ? null : reader.GetString(reader.GetOrdinal("GroupTag"))
                };

                triggeredFields.Add(field);
            }

            return triggeredFields;
        }

        public List<Product> GetAllDrafts()
        {
            var drafts = new List<Product>();

            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spGetAllDrafts", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var product = new Product
                {
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? "" : reader.GetString(reader.GetOrdinal("Name")),
                    Season = reader.IsDBNull(reader.GetOrdinal("Season")) ? "" : reader.GetString(reader.GetOrdinal("Season")),
                    DgaItemNo = reader.IsDBNull(reader.GetOrdinal("DgaItemNo")) ? "" : reader.GetString(reader.GetOrdinal("DgaItemNo")),
                    CountryOfOrigin = reader.IsDBNull(reader.GetOrdinal("CountryOfOrigin")) ? "" : reader.GetString(reader.GetOrdinal("CountryOfOrigin")),
                    Supplier = reader.IsDBNull(reader.GetOrdinal("Supplier")) ? "" : reader.GetString(reader.GetOrdinal("Supplier")),
                    Designer = reader.IsDBNull(reader.GetOrdinal("Designer")) ? "" : reader.GetString(reader.GetOrdinal("Designer")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString(reader.GetOrdinal("Description")),
                    ColiSize = reader.IsDBNull(reader.GetOrdinal("ColiSize")) ? "" : reader.GetInt32(reader.GetOrdinal("ColiSize")).ToString(),
                    ProductGroup = reader.IsDBNull(reader.GetOrdinal("ProductGroup")) ? "" : reader.GetString(reader.GetOrdinal("ProductGroup")),
                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "" : reader.GetString(reader.GetOrdinal("Status"))
                };

                drafts.Add(product);
            }

            return drafts;
        }

        public Product GetProductById(int id)
        {
            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spGetProductFullInfo", conn);

            SqlHelper.AddSimpleParameter(cmd, "@ProductId", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                throw new KeyNotFoundException($"Produkt med ID {id} blev ikke fundet.");

            return new Product
            {
                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Season = reader.IsDBNull(reader.GetOrdinal("Season")) ? "" : reader.GetString(reader.GetOrdinal("Season")),
                DgaItemNo = reader.GetString(reader.GetOrdinal("DgaItemNo")),
                CountryOfOrigin = reader.GetString(reader.GetOrdinal("CountryOfOrigin")),
                Supplier = reader.GetString(reader.GetOrdinal("Supplier")),
                Designer = reader.GetString(reader.GetOrdinal("Designer")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString(reader.GetOrdinal("Description")),
                ColiSize = reader.IsDBNull(reader.GetOrdinal("ColiSize")) ? "" : reader.GetInt32(reader.GetOrdinal("ColiSize")).ToString(),
                ProductGroup = reader.GetString(reader.GetOrdinal("ProductGroup")),
                Status = reader.GetString(reader.GetOrdinal("Status"))
            };
        }

        public void UpdateProductStatus(int productId, string status)
        {
            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spUpdateProductStatus", conn);

            SqlHelper.AddSimpleParameter(cmd, "@ProductId", productId);
            SqlHelper.AddSimpleParameter(cmd, "@Status", status);

            cmd.ExecuteNonQuery();
        }

        // NEW DROPDOWN METHODS - Reading from your database tables

        public List<DropdownItem> GetCountries()
        {
            var countries = new List<DropdownItem>();

            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT CountryId, CountryName, CountryCode FROM Country ORDER BY CountryName", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                countries.Add(new DropdownItem
                {
                    Id = reader.GetInt32("CountryId"),
                    Name = reader.GetString("CountryName"),
                    Code = reader.GetString("CountryCode")
                });
            }

            return countries;
        }

        public List<DropdownItem> GetDesigners()
        {
            var designers = new List<DropdownItem>();

            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT DesignerId, DesignerName FROM Designer ORDER BY DesignerName", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                designers.Add(new DropdownItem
                {
                    Id = reader.GetInt32("DesignerId"),
                    Name = reader.GetString("DesignerName")
                });
            }

            return designers;
        }

        public List<DropdownItem> GetSuppliers()
        {
            var suppliers = new List<DropdownItem>();

            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT SupplierId, Name, SupplierNo FROM Supplier ORDER BY Name", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                suppliers.Add(new DropdownItem
                {
                    Id = reader.GetInt32("SupplierId"),
                    Name = reader.GetString("Name"),
                    Code = reader.IsDBNull("SupplierNo") ? "" : reader.GetInt32("SupplierNo").ToString()
                });
            }

            return suppliers;
        }

        public List<DropdownItem> GetColorGroups()
        {
            var colorGroups = new List<DropdownItem>();

            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT ColorGroupId, ColorGroupName FROM ColorGroup ORDER BY ColorGroupName", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                colorGroups.Add(new DropdownItem
                {
                    Id = reader.GetInt32("ColorGroupId"),
                    Name = reader.IsDBNull("ColorGroupName") ? "" : reader.GetString("ColorGroupName")
                });
            }

            return colorGroups;
        }

        public List<DropdownItem> GetCertifications()
        {
            var certifications = new List<DropdownItem>();

            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT CertificationId, CertificationType FROM Certification ORDER BY CertificationType", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                certifications.Add(new DropdownItem
                {
                    Id = reader.GetInt32("CertificationId"),
                    Name = reader.GetString("CertificationType")
                });
            }

            return certifications;
        }

        public List<DropdownItem> GetPantoneColors()
        {
            var pantoneColors = new List<DropdownItem>();

            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT PantoneId, ColorCode, ColorName FROM Pantone ORDER BY ColorCode", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pantoneColors.Add(new DropdownItem
                {
                    Id = reader.GetInt32("PantoneId"),
                    Name = reader.IsDBNull("ColorName") ? "" : reader.GetString("ColorName"),
                    Code = reader.GetString("ColorCode")
                });
            }

            return pantoneColors;
        }

        public List<DropdownItem> GetProductLogos()
        {
            var productLogos = new List<DropdownItem>();

            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spGetProductLogos", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                productLogos.Add(new DropdownItem
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name")
                });
            }

            return productLogos;
        }

        public List<DropdownItem> GetHangtagsStickers()
        {
            var hangtagsStickers = new List<DropdownItem>();

            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spGetHangtagsStickers", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                hangtagsStickers.Add(new DropdownItem
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name")
                });
            }

            return hangtagsStickers;
        }

        public List<DropdownItem> GetProductSeries()
        {
            var productSeries = new List<DropdownItem>();

            using var conn = SqlHelper.CreateConnection(_connectionString);
            using var cmd = SqlHelper.CreateStoredProcedureCommand("spGetProductSeries", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                productSeries.Add(new DropdownItem
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name")
                });
            }

            return productSeries;
        }
    }
}