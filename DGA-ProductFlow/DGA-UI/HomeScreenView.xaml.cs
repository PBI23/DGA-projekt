using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ProduktFlow2.Core.Models;

namespace ProduktOprettelse.Views
{
    /// <summary>
    /// Homescreen view that displays all products and allows navigation to create/edit products
    /// </summary>
    public partial class HomeScreenView : UserControl
    {
        private ProduktViewModel _viewModel;
        private List<Product> _allProducts;
        private ICollectionView _productsView;

        public HomeScreenView()
        {
            InitializeComponent();
            Loaded += HomeScreenView_Loaded;
        }

        private async void HomeScreenView_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as ProduktViewModel;
            if (_viewModel != null)
            {
                await LoadProducts();
                SetupFilters();
            }
        }

        /// <summary>
        /// Load all products from the database
        /// </summary>
        private async System.Threading.Tasks.Task LoadProducts()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                // Get all products (both drafts and approved)
                _allProducts = new List<Product>();

                // Get drafts
                var drafts = _viewModel.GetDraftProducts();
                _allProducts.AddRange(drafts);

                // TODO: Add method to get approved products when available in repository
                // For now, we'll just show drafts

                // Set up the collection view for filtering
                _productsView = CollectionViewSource.GetDefaultView(_allProducts);
                dgProducts.ItemsSource = _productsView;

                // Update last updated time
                txtLastUpdated.Text = DateTime.Now.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved indlæsning af produkter: {ex.Message}", "Fejl",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        /// <summary>
        /// Setup filter functionality
        /// </summary>
        private void SetupFilters()
        {
            if (_productsView != null)
            {
                _productsView.Filter = ProductFilter;
            }

            // Add "All" option to designer filter
            if (cmbDesignerFilter.Items.Count == 0 || !(cmbDesignerFilter.Items[0] is ComboBoxItem))
            {
                if (cmbDesignerFilter.ItemsSource is null)
                {
                    cmbDesignerFilter.Items.Insert(0, new ComboBoxItem { Content = "Alle", IsSelected = true });
                }
                else
                {
                    // Modify the bound collection instead
                    var designerFilterSource = cmbDesignerFilter.ItemsSource as IList<object>;
                    designerFilterSource?.Insert(0, "Alle");
                }
            }
        }

        /// <summary>
        /// Filter logic for products
        /// </summary>
        private bool ProductFilter(object item)
        {
            var product = item as Product;
            if (product == null) return false;

            // Search filter
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var searchText = txtSearch.Text.ToLower();
                if (!product.Name.ToLower().Contains(searchText) &&
                    !product.DgaItemNo.ToLower().Contains(searchText))
                {
                    return false;
                }
            }

            // Status filter
            if (cmbStatusFilter.SelectedItem is ComboBoxItem statusItem &&
                statusItem.Content.ToString() != "Alle")
            {
                var selectedStatus = statusItem.Content.ToString();
                var productStatus = GetDisplayStatus(product.Status);
                if (productStatus != selectedStatus)
                {
                    return false;
                }
            }

            // Designer filter
            if (cmbDesignerFilter.SelectedItem is ListeItem designerItem)
            {
                if (product.Designer != designerItem.Navn)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Convert internal status to display status
        /// </summary>
        private string GetDisplayStatus(string status)
        {
            switch (status)
            {
                case "Draft":
                    return "Kladde";
                case "Approved":
                    return "Godkendt";
                case "Step2Completed":
                case "Step3Completed":
                case "Step4Completed":
                    return "Under behandling";
                default:
                    return status;
            }
        }

        /// <summary>
        /// Handle create new product button click
        /// </summary>
        private void btnCreateNewProduct_Click(object sender, RoutedEventArgs e)
        {
            // Find the main window and trigger product creation
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.StartNewProductCreation();
            }
        }

        /// <summary>
        /// Handle refresh button click
        /// </summary>
        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadProducts();
        }

        /// <summary>
        /// Handle search text changed
        /// </summary>
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _productsView?.Refresh();
        }

        /// <summary>
        /// Handle status filter changed
        /// </summary>
        private void cmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _productsView?.Refresh();
        }

        /// <summary>
        /// Handle designer filter changed
        /// </summary>
        private void cmbDesignerFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _productsView?.Refresh();
        }

        /// <summary>
        /// Handle double-click on product row
        /// </summary>
        private void dgProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgProducts.SelectedItem is Product product)
            {
                EditProduct(product.ProductId);
            }
        }

        /// <summary>
        /// Handle edit button click
        /// </summary>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int productId)
            {
                EditProduct(productId);
            }
        }

        /// <summary>
        /// Handle view button click
        /// </summary>
        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int productId)
            {
                ViewProduct(productId);
            }
        }

        /// <summary>
        /// Open product for editing
        /// </summary>
        private void EditProduct(int productId)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.EditExistingProduct(productId);
            }
        }

        /// <summary>
        /// Open product for viewing (read-only)
        /// </summary>
        private void ViewProduct(int productId)
        {
            // Get product details
            var product = _viewModel.GetProductById(productId);

            // Show product details in a dialog
            var message = $"Produkt ID: {product.ProductId}\n" +
                         $"Navn: {product.Name}\n" +
                         $"DGA Nr: {product.DgaItemNo}\n" +
                         $"Designer: {product.Designer}\n" +
                         $"Leverandør: {product.Supplier}\n" +
                         $"Land: {product.CountryOfOrigin}\n" +
                         $"Status: {product.Status}\n\n" +
                         $"Beskrivelse: {product.Description}";

            MessageBox.Show(message, "Produkt Detaljer", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Public method to refresh the product list (called from MainWindow after product creation)
        /// </summary>
        public async System.Threading.Tasks.Task RefreshProductList()
        {
            await LoadProducts();
        }
    }
}