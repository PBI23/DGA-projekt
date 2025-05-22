using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ProduktFlow2.Core.Controllers;
using ProduktFlow2.Core.Models;
using ProduktFlow2.Core.Repositories;
using ProduktFlow2.Core.Services;
using System.Linq;

namespace ProduktOprettelse
{
    /// <summary>
    /// Model class representing an item in a dropdown list
    /// </summary>
    public class ListeItem
    {
        /// <summary>
        /// Unique identifier for the item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Display name for the item
        /// </summary>
        public string Navn { get; set; } = string.Empty;

        /// <summary>
        /// Optional code (for country codes, etc.)
        /// </summary>
        public string Code { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel for handling data binding between the UI and data sources
    /// Now fully integrated with ProductFlow2.Core and loads data from your online database
    /// </summary>
    public class ProduktViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly ProductProcessManager _productManager;
        private readonly ProductService _productService;
        private readonly IProductRepository _repository;
        private readonly string _connectionString;

        // Current product ID being processed
        public int CurrentProductId { get; private set; }

        /// <summary>
        /// Constructor that initializes the ProductFlow2.Core integration with your online database
        /// </summary>
        public ProduktViewModel()
        {
            // Initialize configuration for .NET 8.0
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Get connection string from configuration (your online database)
            _connectionString = configuration.GetConnectionString("DGADatabase")
                ?? "Server=tcp:dga-server.database.windows.net,1433;Initial Catalog=DGA_ProductDB;Persist Security Info=False;User ID=sqladmin;Password=Projekt4@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            // Set up dependency injection for .NET 8.0
            var services = new ServiceCollection();
            services.AddSingleton<IProductRepository>(provider => new ProductRepositoryDb(_connectionString));
            services.AddSingleton<ProductService>();
            services.AddSingleton<ProductProcessManager>();

            var serviceProvider = services.BuildServiceProvider();
            _productManager = serviceProvider.GetRequiredService<ProductProcessManager>();
            _productService = serviceProvider.GetRequiredService<ProductService>();
            _repository = serviceProvider.GetRequiredService<IProductRepository>();

            // Initialize collections (will be loaded asynchronously)
            InitializeEmptyCollections();
        }

        #region Observable Collections for Dropdowns

        private ObservableCollection<ListeItem>? _varegrupper;
        public ObservableCollection<ListeItem>? Varegrupper
        {
            get { return _varegrupper; }
            set
            {
                _varegrupper = value;
                OnPropertyChanged(nameof(Varegrupper));
            }
        }

        private ObservableCollection<ListeItem>? _saesoner;
        public ObservableCollection<ListeItem>? Saesoner
        {
            get { return _saesoner; }
            set
            {
                _saesoner = value;
                OnPropertyChanged(nameof(Saesoner));
            }
        }

        private ObservableCollection<ListeItem>? _oprindelseslande;
        public ObservableCollection<ListeItem>? Oprindelseslande
        {
            get { return _oprindelseslande; }
            set
            {
                _oprindelseslande = value;
                OnPropertyChanged(nameof(Oprindelseslande));
            }
        }

        private ObservableCollection<ListeItem>? _leverandoerer;
        public ObservableCollection<ListeItem>? Leverandoerer
        {
            get { return _leverandoerer; }
            set
            {
                _leverandoerer = value;
                OnPropertyChanged(nameof(Leverandoerer));
            }
        }

        private ObservableCollection<ListeItem>? _designere;
        public ObservableCollection<ListeItem>? Designere
        {
            get { return _designere; }
            set
            {
                _designere = value;
                OnPropertyChanged(nameof(Designere));
            }
        }

        private ObservableCollection<ListeItem>? _beskrivelse;
        public ObservableCollection<ListeItem>? Beskrivelse
        {
            get { return _beskrivelse; }
            set
            {
                _beskrivelse = value;
                OnPropertyChanged(nameof(Beskrivelse));
            }
        }

        private ObservableCollection<ListeItem>? _kategorier;
        public ObservableCollection<ListeItem>? Kategorier
        {
            get { return _kategorier; }
            set
            {
                _kategorier = value;
                OnPropertyChanged(nameof(Kategorier));
            }
        }

        private ObservableCollection<ListeItem>? _farver;
        public ObservableCollection<ListeItem>? Farver
        {
            get { return _farver; }
            set
            {
                _farver = value;
                OnPropertyChanged(nameof(Farver));
            }
        }

        private ObservableCollection<ListeItem>? _materialer;
        public ObservableCollection<ListeItem>? Materialer
        {
            get { return _materialer; }
            set
            {
                _materialer = value;
                OnPropertyChanged(nameof(Materialer));
            }
        }

        private ObservableCollection<ListeItem>? _enheder;
        public ObservableCollection<ListeItem>? Enheder
        {
            get { return _enheder; }
            set
            {
                _enheder = value;
                OnPropertyChanged(nameof(Enheder));
            }
        }

        private ObservableCollection<ListeItem>? _produktLogos;
        public ObservableCollection<ListeItem>? ProduktLogos
        {
            get { return _produktLogos; }
            set
            {
                _produktLogos = value;
                OnPropertyChanged(nameof(ProduktLogos));
            }
        }

        private ObservableCollection<ListeItem>? _hangtagsStickers;
        public ObservableCollection<ListeItem>? HangtagsStickers
        {
            get { return _hangtagsStickers; }
            set
            {
                _hangtagsStickers = value;
                OnPropertyChanged(nameof(HangtagsStickers));
            }
        }

        private ObservableCollection<ListeItem>? _produktSerier;
        public ObservableCollection<ListeItem>? ProduktSerier
        {
            get { return _produktSerier; }
            set
            {
                _produktSerier = value;
                OnPropertyChanged(nameof(ProduktSerier));
            }
        }

        #endregion

        /// <summary>
        /// Initialize empty collections first, then load from database
        /// </summary>
        private void InitializeEmptyCollections()
        {
            Varegrupper = new ObservableCollection<ListeItem>();
            Saesoner = new ObservableCollection<ListeItem>();
            Oprindelseslande = new ObservableCollection<ListeItem>();
            Leverandoerer = new ObservableCollection<ListeItem>();
            Designere = new ObservableCollection<ListeItem>();
            Beskrivelse = new ObservableCollection<ListeItem>();
            Kategorier = new ObservableCollection<ListeItem>();
            Farver = new ObservableCollection<ListeItem>();
            Materialer = new ObservableCollection<ListeItem>();
            Enheder = new ObservableCollection<ListeItem>();
            ProduktLogos = new ObservableCollection<ListeItem>();
            HangtagsStickers = new ObservableCollection<ListeItem>();
            ProduktSerier = new ObservableCollection<ListeItem>();
        }

        /// <summary>
        /// Load dropdown data from your database tables
        /// </summary>
        private async Task LoadDropdownDataFromDatabase()
        {
            try
            {
                // Load Countries from database
                var countries = await Task.Run(() => _repository.GetCountries());
                Oprindelseslande!.Clear();
                foreach (var country in countries)
                {
                    Oprindelseslande.Add(new ListeItem
                    {
                        Id = country.Id,
                        Navn = country.Name,
                        Code = country.Code
                    });
                }

                // Load Designers from database
                var designers = await Task.Run(() => _repository.GetDesigners());
                Designere!.Clear();
                foreach (var designer in designers)
                {
                    Designere.Add(new ListeItem
                    {
                        Id = designer.Id,
                        Navn = designer.Name
                    });
                }

                // Load Suppliers from database
                var suppliers = await Task.Run(() => _repository.GetSuppliers());
                Leverandoerer!.Clear();
                foreach (var supplier in suppliers)
                {
                    Leverandoerer.Add(new ListeItem
                    {
                        Id = supplier.Id,
                        Navn = supplier.Name,
                        Code = supplier.Code
                    });
                }

                // Load Color Groups from database
                var colorGroups = await Task.Run(() => _repository.GetColorGroups());
                Farver!.Clear();
                foreach (var color in colorGroups)
                {
                    Farver.Add(new ListeItem
                    {
                        Id = color.Id,
                        Navn = color.Name
                    });
                }

                // Load Certifications as categories
                var certifications = await Task.Run(() => _repository.GetCertifications());
                Kategorier!.Clear();
                foreach (var cert in certifications)
                {
                    Kategorier.Add(new ListeItem
                    {
                        Id = cert.Id,
                        Navn = cert.Name
                    });
                }

                // Load Product Logos from database
                var productLogos = await Task.Run(() => _repository.GetProductLogos());
                ProduktLogos!.Clear();
                foreach (var logo in productLogos)
                {
                    ProduktLogos.Add(new ListeItem
                    {
                        Id = logo.Id,
                        Navn = logo.Name
                    });
                }

                // Load Hangtags/Stickers from database
                var hangtagsStickers = await Task.Run(() => _repository.GetHangtagsStickers());
                HangtagsStickers!.Clear();
                foreach (var hangtag in hangtagsStickers)
                {
                    HangtagsStickers.Add(new ListeItem
                    {
                        Id = hangtag.Id,
                        Navn = hangtag.Name
                    });
                }

                // Load Product Series from database
                var productSeries = await Task.Run(() => _repository.GetProductSeries());
                ProduktSerier!.Clear();
                foreach (var series in productSeries)
                {
                    ProduktSerier.Add(new ListeItem
                    {
                        Id = series.Id,
                        Navn = series.Name
                    });
                }

                // For now, keep some hardcoded values for data not in your DB yet
                LoadHardcodedData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved indlæsning af dropdown data: {ex.Message}", "Database Fejl", MessageBoxButton.OK, MessageBoxImage.Error);

                // Fallback to hardcoded data if database fails
                LoadHardcodedData();
            }
        }

        /// <summary>
        /// Load hardcoded data for fields not yet in database
        /// </summary>
        private void LoadHardcodedData()
        {
            // Product groups - you should add these to your database later
            if (!Varegrupper!.Any())
            {
                var productGroups = new[]
                {
                    new ListeItem { Id = 1, Navn = "Julepynt" },
                    new ListeItem { Id = 2, Navn = "Påskepynt" },
                    new ListeItem { Id = 3, Navn = "Boligindretning" },
                    new ListeItem { Id = 4, Navn = "Køkkenudstyr" },
                    new ListeItem { Id = 5, Navn = "Dekoration" }
                };

                foreach (var item in productGroups)
                    Varegrupper.Add(item);
            }

            // Seasons - you should add these to your database later
            if (!Saesoner!.Any())
            {
                var seasons = new[]
                {
                    new ListeItem { Id = 1, Navn = "Forår" },
                    new ListeItem { Id = 2, Navn = "Sommer" },
                    new ListeItem { Id = 3, Navn = "Efterår" },
                    new ListeItem { Id = 4, Navn = "Vinter" },
                    new ListeItem { Id = 5, Navn = "Jul" },
                    new ListeItem { Id = 6, Navn = "Påske" },
                    new ListeItem { Id = 7, Navn = "Hele året" }
                };

                foreach (var item in seasons)
                    Saesoner.Add(item);
            }

            // Product descriptions - you should add these to your database later
            if (!Beskrivelse!.Any())
            {
                var descriptions = new[]
                {
                    new ListeItem { Id = 1, Navn = "Elegant og stilren" },
                    new ListeItem { Id = 2, Navn = "Farverig og sjov" },
                    new ListeItem { Id = 3, Navn = "Praktisk og funktionel" },
                    new ListeItem { Id = 4, Navn = "Traditionel og klassisk" },
                    new ListeItem { Id = 5, Navn = "Moderne og innovativ" }
                };

                foreach (var item in descriptions)
                    Beskrivelse.Add(item);
            }

            // Materials - you should add these to your database later
            if (!Materialer!.Any())
            {
                var materials = new[]
                {
                    new ListeItem { Id = 1, Navn = "Træ" },
                    new ListeItem { Id = 2, Navn = "Metal" },
                    new ListeItem { Id = 3, Navn = "Plastik" },
                    new ListeItem { Id = 4, Navn = "Glas" },
                    new ListeItem { Id = 5, Navn = "Stof" },
                    new ListeItem { Id = 6, Navn = "Keramik" }
                };

                foreach (var item in materials)
                    Materialer.Add(item);
            }

            // Units - you should add these to your database later
            if (!Enheder!.Any())
            {
                var units = new[]
                {
                    new ListeItem { Id = 1, Navn = "kg" },
                    new ListeItem { Id = 2, Navn = "g" },
                    new ListeItem { Id = 3, Navn = "lb" },
                    new ListeItem { Id = 4, Navn = "oz" },
                    new ListeItem { Id = 5, Navn = "stk" }
                };

                foreach (var item in units)
                    Enheder.Add(item);
            }
        }

        /// <summary>
        /// Saves Step 1 data to your online database and returns the new product ID
        /// </summary>
        public async Task<int> SaveStep1DataAsync(Step1Dto step1Data)
        {
            try
            {
                CurrentProductId = _productManager.StartStep1(step1Data);
                return CurrentProductId;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved gemning af trin 1: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        /// <summary>
        /// Gets field definitions for Step 2 (compliance questions) from your database
        /// </summary>
        public List<FieldDefinition> GetStep2Fields()
        {
            try
            {
                return _productManager.StartStep2();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved hentning af step 2 felter: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<FieldDefinition>();
            }
        }

        /// <summary>
        /// Saves Step 2 compliance answers to your online database
        /// </summary>
        public async Task SaveStep2DataAsync(Step2Dto step2Data)
        {
            try
            {
                _productManager.SaveStep2Answers(step2Data);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved gemning af trin 2: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        /// <summary>
        /// Saves Step 3 data to your online database
        /// </summary>
        public async Task SaveStep3DataAsync(Step3Dto step3Data)
        {
            try
            {
                _productManager.SaveStep3Answers(step3Data);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved gemning af trin 3: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        /// <summary>
        /// Saves Step 4 data to your online database
        /// </summary>
        public async Task SaveStep4DataAsync(Step4Dto step4Data)
        {
            try
            {
                _productManager.SaveStep4Answers(step4Data);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved gemning af trin 4: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        /// <summary>
        /// Gets all draft products from your online database
        /// </summary>
        public List<Product> GetDraftProducts()
        {
            try
            {
                return _productManager.GetDraftProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved hentning af kladder: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Product>();
            }
        }

        /// <summary>
        /// Gets a specific product by ID from your online database
        /// </summary>
        public Product GetProductById(int productId)
        {
            try
            {
                return _productService.GetProductById(productId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved hentning af produkt: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        /// <summary>
        /// Gets triggered fields based on user selection from your database
        /// </summary>
        public List<FieldDefinition> GetTriggeredFields(string parentField, string triggerValue)
        {
            try
            {
                return _productManager.GetTriggeredFields(parentField, triggerValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved hentning af afhængige felter: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<FieldDefinition>();
            }
        }

        /// <summary>
        /// Tests database connection and loads dropdown data
        /// </summary>
        public async Task<bool> TestDatabaseConnectionAsync()
        {
            try
            {
                var drafts = GetDraftProducts();
                await LoadDropdownDataFromDatabase();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database forbindelse fejlede: {ex.Message}", "Database Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Loads data asynchronously from your database
        /// </summary>
        public async Task LoadDataAsync()
        {
            try
            {
                // Test database connection and load dropdown data
                bool isConnected = await TestDatabaseConnectionAsync();
                if (isConnected)
                {
                    MessageBox.Show("Database forbindelse etableret og data indlæst!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // If database connection fails, use hardcoded data as fallback
                    LoadHardcodedData();
                    MessageBox.Show("Database forbindelse fejlede. Bruger lokale data.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved indlæsning af data: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                // Use hardcoded data as fallback
                LoadHardcodedData();
            }
        }

        /// <summary>
        /// Refreshes dropdown data from database
        /// </summary>
        public async Task RefreshDropdownDataAsync()
        {
            await LoadDropdownDataFromDatabase();
        }

        /// <summary>
        /// Add a new country to the database and refresh the dropdown
        /// </summary>
        public async Task AddCountryAsync(string countryName, string countryCode)
        {
            try
            {
                // You would need to add this method to your repository
                // For now, show what would happen
                MessageBox.Show($"Ville tilføje land: {countryName} ({countryCode})", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                // After adding to database, refresh dropdown
                await RefreshDropdownDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved tilføjelse af land: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Add a new designer to the database and refresh the dropdown
        /// </summary>
        public async Task AddDesignerAsync(string designerName)
        {
            try
            {
                // You would need to add this method to your repository
                MessageBox.Show($"Ville tilføje designer: {designerName}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                // After adding to database, refresh dropdown
                await RefreshDropdownDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved tilføjelse af designer: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Notifies subscribers about property changes
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Updated DataService that now uses the integrated ProductFlow2.Core
    /// </summary>
    public static class DataService
    {
        /// <summary>
        /// This method is kept for compatibility but now redirects to the integrated system
        /// </summary>
        public static async Task<List<ListeItem>> HentDataFraBackendAsync(string dataType)
        {
            await Task.Delay(100); // Simulate async operation
            return new List<ListeItem>(); // Return empty list as data is now handled by integrated system
        }

        /// <summary>
        /// This method is kept for compatibility but product saving is now handled through the integrated system
        /// </summary>
        public static async Task<bool> GemProduktAsync(object produktData)
        {
            await Task.Delay(100); // Simulate async operation
            return true; // Product saving is now handled through ProductProcessManager
        }
    }
}