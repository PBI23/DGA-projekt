using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProduktOprettelse
{
    public partial class MainWindow : Window
    {
        private int currentStep = 1;

        public MainWindow()
        {
            InitializeComponent();
            SetupInitialState();
        }

        private void SetupInitialState()
        {
            // Set default date values
            dpStartdato.SelectedDate = DateTime.Today;
            dpSlutdato.SelectedDate = DateTime.Today.AddYears(1);

            // Setup UI for first step
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Hide all grids
            gridTrin1.Visibility = Visibility.Collapsed;
            gridTrin2.Visibility = Visibility.Collapsed;
            gridTrin3.Visibility = Visibility.Collapsed;
            gridTrin4.Visibility = Visibility.Collapsed;

            // Reset button styles
            btnTrin1.Background = System.Windows.Media.Brushes.LightGray;
            btnTrin2.Background = System.Windows.Media.Brushes.LightGray;
            btnTrin3.Background = System.Windows.Media.Brushes.LightGray;
            btnTrin4.Background = System.Windows.Media.Brushes.LightGray;

            // Update UI based on current step
            switch (currentStep)
            {
                case 1:
                    gridTrin1.Visibility = Visibility.Visible;
                    btnTrin1.Background = System.Windows.Media.Brushes.ForestGreen;
                    btnTrin1.Foreground = System.Windows.Media.Brushes.White;
                    btnTilbage.IsEnabled = false;
                    btnNaeste.Content = "Næste";
                    break;
                case 2:
                    gridTrin2.Visibility = Visibility.Visible;
                    btnTrin2.Background = System.Windows.Media.Brushes.ForestGreen;
                    btnTrin2.Foreground = System.Windows.Media.Brushes.White;
                    btnTilbage.IsEnabled = true;
                    btnNaeste.Content = "Næste";
                    break;
                case 3:
                    gridTrin3.Visibility = Visibility.Visible;
                    btnTrin3.Background = System.Windows.Media.Brushes.ForestGreen;
                    btnTrin3.Foreground = System.Windows.Media.Brushes.White;
                    btnTilbage.IsEnabled = true;
                    btnNaeste.Content = "Næste";
                    break;
                case 4:
                    gridTrin4.Visibility = Visibility.Visible;
                    btnTrin4.Background = System.Windows.Media.Brushes.ForestGreen;
                    btnTrin4.Foreground = System.Windows.Media.Brushes.White;
                    btnTilbage.IsEnabled = true;
                    btnNaeste.Content = "Opret produkt";
                    break;
            }
        }

        #region Button Click Events
        private void btnTrin1_Click(object sender, RoutedEventArgs e)
        {
            currentStep = 1;
            btnNaeste.Content = "Næste";
            UpdateUI();
        }

        private void btnTrin2_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateTrin1())
            {
                currentStep = 2;
                btnNaeste.Content = "Næste";
                UpdateUI();
            }
        }

        private void btnTrin3_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateTrin1() && ValidateTrin2())
            {
                currentStep = 3;
                btnNaeste.Content = "Næste";
                UpdateUI();
            }
        }

        private void btnTrin4_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateTrin1() && ValidateTrin2() && ValidateTrin3())
            {
                currentStep = 4;
                btnNaeste.Content = "Afslut";
                UpdateUI();
            }
        }

        private void btnTilbage_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep > 1)
            {
                currentStep--;
                btnNaeste.Content = "Næste";
                UpdateUI();
            }
        }

        private void btnGemKladde_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Produktet er gemt som kladde.", "Gem som kladde", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnNaeste_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = false;

            switch (currentStep)
            {
                case 1:
                    isValid = ValidateTrin1();
                    break;
                case 2:
                    isValid = ValidateTrin2();
                    break;
                case 3:
                    isValid = ValidateTrin3();
                    break;
                case 4:
                    // No validation required for the last step
                    isValid = true;
                    MessageBox.Show("Produktet er oprettet succesfuldt!", "Produktoprettelse", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetForm();
                    return;
            }

            if (isValid)
            {
                currentStep++;
                UpdateUI();
            }
        }
        #endregion

        #region Validation Methods
        private bool ValidateTrin1()
        {
            if (string.IsNullOrWhiteSpace(txtProduktNavn.Text))
            {
                MessageBox.Show("Produktnavn skal udfyldes.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtVareNummer.Text))
            {
                MessageBox.Show("DGA varenummer skal udfyldes.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (cmbSaeson.SelectedItem == null)
            {
                MessageBox.Show("Vælg venligst en sæson.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (cmbOprindelsesland.SelectedItem == null)
            {
                MessageBox.Show("Vælg venligst et oprindelsesland.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool ValidateTrin2()
        {
            if (cmbKategori.SelectedItem == null)
            {
                MessageBox.Show("Vælg venligst en kategori.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (cmbMateriale.SelectedItem == null)
            {
                MessageBox.Show("Vælg venligst et materiale.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Dimensions are optional but if provided, must be valid numbers
            if (!string.IsNullOrWhiteSpace(txtHoejde.Text))
            {
                if (!double.TryParse(txtHoejde.Text, out _))
                {
                    MessageBox.Show("Højde skal være et tal.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtBredde.Text))
            {
                if (!double.TryParse(txtBredde.Text, out _))
                {
                    MessageBox.Show("Bredde skal være et tal.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtDybde.Text))
            {
                if (!double.TryParse(txtDybde.Text, out _))
                {
                    MessageBox.Show("Dybde skal være et tal.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            return true;
        }

        private bool ValidateTrin3()
        {
            if (string.IsNullOrWhiteSpace(txtKostpris.Text))
            {
                MessageBox.Show("Kostpris skal udfyldes.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!double.TryParse(txtKostpris.Text, out _))
            {
                MessageBox.Show("Kostpris skal være et tal.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSalgspris.Text))
            {
                MessageBox.Show("Salgspris skal udfyldes.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!double.TryParse(txtSalgspris.Text, out _))
            {
                MessageBox.Show("Salgspris skal være et tal.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (dpStartdato.SelectedDate == null)
            {
                MessageBox.Show("Startdato skal vælges.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (dpSlutdato.SelectedDate == null)
            {
                MessageBox.Show("Slutdato skal vælges.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (dpStartdato.SelectedDate > dpSlutdato.SelectedDate)
            {
                MessageBox.Show("Startdato kan ikke være efter slutdato.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
        #endregion

        #region Helper Methods
        // Only allow numbers to be entered
        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Allow numbers and decimal separator
        private void NumericWithDecimal(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,\\.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ResetForm()
        {
            // Reset form fields
            txtProduktNavn.Clear();
            cmbSaeson.SelectedIndex = -1;
            txtVareNummer.Clear();
            cmbOprindelsesland.SelectedIndex = -1;
            txtLeverandoer.Clear();
            txtDesigner.Clear();
            txtBeskrivelse.Clear();
            txtColiStoerrelseAntal.Clear();

            cmbKategori.SelectedIndex = -1;
            cmbFarve.SelectedIndex = -1;
            cmbMateriale.SelectedIndex = -1;
            txtHoejde.Clear();
            txtBredde.Clear();
            txtDybde.Clear();

            txtKostpris.Clear();
            txtSalgspris.Clear();
            chkMomspligtig.IsChecked = true;
            dpStartdato.SelectedDate = DateTime.Today;
            dpSlutdato.SelectedDate = DateTime.Today.AddYears(1);
            chkPaaLager.IsChecked = true;

            txtBarcode.Clear();
            txtLeverandoerVarenummer.Clear();
            txtRabatkode.Clear();
            txtTags.Clear();
            txtBemaerkninger.Clear();

            // Reset to first step
            currentStep = 1;
            btnNaeste.Content = "Næste";
            UpdateUI();
        }
        #endregion
    }
}