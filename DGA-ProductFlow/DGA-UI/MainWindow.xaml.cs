using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProduktOprettelse.Views;
using ProduktFlow2.Core.Models;
using System.Collections.Generic;

namespace ProduktOprettelse
{
    /// <summary>
    /// Hovedvindue for produkt-oprettelses applikationen.
    /// Nu med homescreen og navigation mellem produkt oversigt og oprettelse/redigering.
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentStep = 1;
        private const int totalSteps = 5;
        private Button[] stepButtons;
        private UserControl[] stepViews;
        private ProduktViewModel viewModel;
        private bool isEditMode = false;
        private int? editingProductId = null;

        /// <summary>
        /// Initialiserer en ny instans af MainWindow klassen med integreret ProductFlow2.Core.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Initialiserer view-model med integreret database-funktionalitet
            viewModel = new ProduktViewModel();
            this.DataContext = viewModel;

            // Start at indlæse data til dropdown-lister
            LoadDataAsync();

            // Initialiserer arrays med trin-knapper og views for nemmere håndtering
            stepButtons = new Button[] { btnTrin1, btnTrin2, btnTrin3, btnTrin4, btnTrin5 };
            stepViews = new UserControl[] { ucTrin1Basisoplysninger, ucTrin2ProduktInfo, ucTrin3Compliance, ucTrin4ObligatoriskeFelter, ucTrin5ValgfriFelter };

            // Start på homescreen
            ShowHomeScreen();
        }

        /// <summary>
        /// Indlæser data til view-model asynkront.
        /// </summary>
        private async void LoadDataAsync()
        {
            try
            {
                await viewModel.LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved indlæsning af data: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Viser homescreen og skjuler produkt oprettelse
        /// </summary>
        private void ShowHomeScreen()
        {
            ucHomeScreen.Visibility = Visibility.Visible;
            gridProductCreation.Visibility = Visibility.Collapsed;
            isEditMode = false;
            editingProductId = null;
        }

        /// <summary>
        /// Viser produkt oprettelse/redigering og skjuler homescreen
        /// </summary>
        private void ShowProductCreation()
        {
            ucHomeScreen.Visibility = Visibility.Collapsed;
            gridProductCreation.Visibility = Visibility.Visible;
            currentStep = 1;
            UpdateStepUI();
        }

        /// <summary>
        /// Start ny produkt oprettelse (kaldes fra HomeScreen)
        /// </summary>
        public void StartNewProductCreation()
        {
            isEditMode = false;
            editingProductId = null;
            txtEditMode.Visibility = Visibility.Collapsed;

            // Ryd alle felter
            RydFormular();

            // Vis produkt oprettelse
            ShowProductCreation();
        }

        /// <summary>
        /// Rediger eksisterende produkt (kaldes fra HomeScreen)
        /// </summary>
        public void EditExistingProduct(int productId)
        {
            try
            {
                isEditMode = true;
                editingProductId = productId;

                // Vis edit mode i header
                txtEditMode.Text = $"Redigering - Produkt ID: {productId}";
                txtEditMode.Visibility = Visibility.Visible;

                // Hent produkt data
                var product = viewModel.GetProductById(productId);

                // Udfyld felterne med eksisterende data
                LoadProductDataIntoForm(product);

                // Vis produkt oprettelse i edit mode
                ShowProductCreation();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved indlæsning af produkt: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                ShowHomeScreen();
            }
        }

        /// <summary>
        /// Indlæser produkt data ind i formular felterne
        /// </summary>
        private void LoadProductDataIntoForm(Product product)
        {
            // Step 1 - Basis oplysninger
            var trin1 = ucTrin1Basisoplysninger as Trin1BasisoplysningerView;
            if (trin1 != null)
            {
                trin1.txtProduktNavn.Text = product.Name;
                trin1.txtVareNummer.Text = product.DgaItemNo;
                trin1.txtColiStoerrelseAntal.Text = product.ColiSize;

                // Find og vælg dropdown værdier
                SelectDropdownItem(trin1.cmbVaregruppe, product.ProductGroup);
                SelectDropdownItem(trin1.cmbSaeson, product.Season);
                SelectDropdownItem(trin1.cmbOprindelsesland, product.CountryOfOrigin);
                SelectDropdownItem(trin1.cmbLeverandoer, product.Supplier);
                SelectDropdownItem(trin1.cmbDesigner, product.Designer);
                SelectDropdownItem(trin1.cmbBeskrivelse, product.Description);
            }

            // TODO: Load data for other steps when available
        }

        /// <summary>
        /// Hjælpemetode til at vælge item i dropdown
        /// </summary>
        private void SelectDropdownItem(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            foreach (var item in comboBox.Items)
            {
                if (item is ListeItem listeItem && listeItem.Navn == value)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Tilbage til homescreen knap
        /// </summary>
        private void btnBackToHome_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Er du sikker på at du vil gå tilbage? Ikke-gemte ændringer vil gå tabt.",
                "Bekræft", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ShowHomeScreen();
                ucHomeScreen.RefreshProductList();
            }
        }

        /// <summary>
        /// Annuller knap - går tilbage til homescreen
        /// </summary>
        private void btnAnnuller_Click(object sender, RoutedEventArgs e)
        {
            btnBackToHome_Click(sender, e);
        }

        /// <summary>
        /// Håndterer klik på en trin-knap og skifter til det valgte trin.
        /// </summary>
        private void btnTrin_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is string stepTag)
            {
                int targetStep = GetStepNumberFromTag(stepTag);

                // Tjek at det nuværende trin er valideret før du går videre
                if (targetStep > currentStep && !ValiderNuværendeTrin())
                {
                    MessageBox.Show("Udfyld venligst alle påkrævede felter i det nuværende trin, før du går videre.",
                                    "Validering fejlede",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                currentStep = targetStep;
                UpdateStepUI();
            }
        }

        private int GetStepNumberFromTag(string stepTag)
        {
            switch (stepTag)
            {
                case "Trin1": return 1;
                case "Trin2": return 2;
                case "Trin3": return 3;
                case "Trin4": return 4;
                case "Trin5": return 5;
                default: return 1;
            }
        }

        /// <summary>
        /// Håndterer klik på "Næste" knappen med integreret database-gemning.
        /// </summary>
        private async void btnNaeste_Click(object sender, RoutedEventArgs e)
        {
            if (!ValiderNuværendeTrin())
            {
                MessageBox.Show("Udfyld venligst alle påkrævede felter korrekt, før du går videre.",
                                "Validering fejlede",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            // Gem det aktuelle trin til databasen
            try
            {
                await SaveCurrentStepAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved gemning af trin {currentStep}: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (currentStep < totalSteps)
            {
                currentStep++;
                UpdateStepUI();
            }
            else
            {
                // Sidste trin: Produktet er nu fuldført
                string message = isEditMode ? "Produkt opdateret succesfuldt!" : "Produkt oprettet succesfuldt!";
                MessageBox.Show(message, "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                // Gå tilbage til homescreen og opdater listen
                ShowHomeScreen();
                await ucHomeScreen.RefreshProductList();
            }
        }

        /// <summary>
        /// Gemmer det aktuelle trin til databasen via ProductFlow2.Core
        /// </summary>
        private async System.Threading.Tasks.Task SaveCurrentStepAsync()
        {
            switch (currentStep)
            {
                case 1:
                    await SaveStep1();
                    break;
                case 2:
                    await SaveStep2();
                    break;
                case 3:
                    await SaveStep3();
                    break;
                case 4:
                    await SaveStep4();
                    break;
                case 5:
                    // Step 5 gemmes når brugeren klikker "Færdig"
                    break;
            }
        }

        /// <summary>
        /// Gemmer Step 1 data til databasen
        /// </summary>
        private async System.Threading.Tasks.Task SaveStep1()
        {
            var basisOplysninger = (Trin1BasisoplysningerView)ucTrin1Basisoplysninger;

            var step1Data = new Step1Dto
            {
                Name = basisOplysninger.ProduktNavn,
                Season = GetSelectedItemName(basisOplysninger.SelectedSaeson),
                DgaItemNo = basisOplysninger.VareNummer,
                CountryOfOrigin = GetSelectedItemName(basisOplysninger.SelectedOprindelsesland),
                Supplier = GetSelectedItemName(basisOplysninger.SelectedLeverandoer),
                Designer = GetSelectedItemName(basisOplysninger.SelectedDesigner),
                Description = GetSelectedItemName(basisOplysninger.SelectedBeskrivelse),
                ColiSize = basisOplysninger.ColiStoerrelseAntal,
                ProductGroup = GetSelectedItemName(basisOplysninger.SelectedVaregruppe)
            };

            if (isEditMode && editingProductId.HasValue)
            {
                // I edit mode skal vi opdatere eksisterende produkt
                // TODO: Implementer update metode i repository
                MessageBox.Show("Produkt opdatering er under udvikling", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // Opret nyt produkt
                int productId = await viewModel.SaveStep1DataAsync(step1Data);
                editingProductId = productId; // Gem ID for videre brug
                MessageBox.Show($"Trin 1 gemt. Produkt ID: {productId}", "Gemt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Gemmer Step 2 compliance data til databasen
        /// </summary>
        private async System.Threading.Tasks.Task SaveStep2()
        {
            var compliance = (Trin3ComplianceView)ucTrin3Compliance;

            var step2Data = new Step2Dto
            {
                ProductId = editingProductId ?? viewModel.CurrentProductId,
                Answers = new Dictionary<string, bool>()
            };

            // Map compliance answers to the expected format
            if (compliance.IsFKM.HasValue)
                step2Data.Answers["IsFKM"] = compliance.IsFKM.Value;
            if (compliance.IsElektronik.HasValue)
                step2Data.Answers["IsElektronik"] = compliance.IsElektronik.Value;
            if (compliance.IsTrae.HasValue)
                step2Data.Answers["IsTrae"] = compliance.IsTrae.Value;
            if (compliance.IsKosmetik.HasValue)
                step2Data.Answers["IsKosmetik"] = compliance.IsKosmetik.Value;
            if (compliance.IsREACH.HasValue)
                step2Data.Answers["IsREACH"] = compliance.IsREACH.Value;
            if (compliance.IsBoern.HasValue)
                step2Data.Answers["IsBoern"] = compliance.IsBoern.Value;
            if (compliance.IsTekstiler.HasValue)
                step2Data.Answers["IsTekstiler"] = compliance.IsTekstiler.Value;
            if (compliance.IsGenerelle.HasValue)
                step2Data.Answers["IsGenerelle"] = compliance.IsGenerelle.Value;

            // Add sub-question answers
            if (compliance.FKMSpoergsmaal1Svar.HasValue)
                step2Data.Answers["FKMSpoergsmaal1"] = compliance.FKMSpoergsmaal1Svar.Value;
            if (compliance.FKMSpoergsmaal2Svar.HasValue)
                step2Data.Answers["FKMSpoergsmaal2"] = compliance.FKMSpoergsmaal2Svar.Value;
            if (compliance.FKMSpoergsmaal3Svar.HasValue)
                step2Data.Answers["FKMSpoergsmaal3"] = compliance.FKMSpoergsmaal3Svar.Value;

            await viewModel.SaveStep2DataAsync(step2Data);

            MessageBox.Show("Trin 2 (Compliance) gemt", "Gemt", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Gemmer Step 3 data til databasen
        /// </summary>
        private async System.Threading.Tasks.Task SaveStep3()
        {
            var obligatoriskeFelter = (Trin4ObligatoriskeFelterView)ucTrin4ObligatoriskeFelter;

            var step3Data = new Step3Dto
            {
                ProductId = editingProductId ?? viewModel.CurrentProductId,
                SupplierProductNo = ParseIntSafely(obligatoriskeFelter.LeverandorProduktNr),
                CustomerClearanceNo = ParseIntSafely(obligatoriskeFelter.CustomerClearanceNo),
                CustomerClearancePercent = ParseDecimalSafely(obligatoriskeFelter.CustomerClearancePct),
                CostPrice = ParseDecimalSafely(obligatoriskeFelter.Kostpris),
                InnerCarton = ParseIntSafely(obligatoriskeFelter.InnerCarton),
                OuterCarton = obligatoriskeFelter.OuterCarton,
                GrossWeight = ParseDecimalSafely(obligatoriskeFelter.GrossWeight),
                PackingHeight = ParseDecimalSafely(obligatoriskeFelter.PackingHeight),
                PackingWidthLength = ParseDecimalSafely(obligatoriskeFelter.PackingWidth),
                PackingDepth = ParseDecimalSafely(obligatoriskeFelter.PackingDepth),
                DishwasherSafe = obligatoriskeFelter.Dishwasher,
                MicrowaveSafe = obligatoriskeFelter.Microwave,
                Svanemaerket = obligatoriskeFelter.Svanemaerket,
                GrunerPunkt = obligatoriskeFelter.GrunerPunkt,
                FSC100 = obligatoriskeFelter.FSC100,
                FSCMix70 = obligatoriskeFelter.FSCMIX70,
                ABC = obligatoriskeFelter.Group,
                ProductLogo = "Standard", // You might want to map this from a dropdown
                HangtagsAndStickers = obligatoriskeFelter.HangtagsStickers,
                Series = obligatoriskeFelter.Produktserie
            };

            await viewModel.SaveStep3DataAsync(step3Data);

            MessageBox.Show("Trin 3 gemt", "Gemt", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Gemmer Step 4 data til databasen
        /// </summary>
        private async System.Threading.Tasks.Task SaveStep4()
        {
            var valgfriFelter = (Trin5ValgfriFelterView)ucTrin5ValgfriFelter;

            var step4Data = new Step4Dto
            {
                ProductId = editingProductId ?? viewModel.CurrentProductId,
                DgaColorGroupName = "Standard", // Map from your UI if available
                DgaSalCatGroup = "General", // Map from your UI if available
                PantonePantone = "N/A", // Map from your UI if available
                DgaVendItemCodeCode = valgfriFelter.LeverandoerVarenummer,
                Assorted = null, // Map from your UI if available
                AdditionalInformation = valgfriFelter.Bemaerkninger,
                Subcategory = "General", // Map from your UI if available
                GsmWeight = null, // Map from your UI if available
                GsmWeight2 = null, // Map from your UI if available
                BurningTimeHours = null, // Map from your UI if available
                AntidopingRegulation = null, // Map from your UI if available
                OtherInformation2 = valgfriFelter.Tags
            };

            await viewModel.SaveStep4DataAsync(step4Data);

            MessageBox.Show("Trin 4 gemt", "Gemt", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Håndterer klik på "Tilbage" knappen.
        /// </summary>
        private void btnTilbage_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep > 1)
            {
                currentStep--;
                UpdateStepUI();
            }
        }

        /// <summary>
        /// Håndterer klik på "Gem Kladde" knappen.
        /// </summary>
        private async void btnGemKladde_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SaveCurrentStepAsync();
                MessageBox.Show("Kladde gemt succesfuldt!", "Gem kladde", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved gemning af kladde: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Opdaterer UI baseret på det aktuelle trin.
        /// </summary>
        private void UpdateStepUI()
        {
            for (int i = 0; i < totalSteps; i++)
            {
                if (stepViews[i] != null)
                {
                    stepViews[i].Visibility = (i == currentStep - 1) ? Visibility.Visible : Visibility.Collapsed;
                }
                if (stepButtons[i] != null)
                {
                    stepButtons[i].Background = (i == currentStep - 1)
                        ? new SolidColorBrush(Color.FromRgb(76, 175, 80))
                        : new SolidColorBrush(Color.FromRgb(224, 224, 224));

                    stepButtons[i].Foreground = (i == currentStep - 1) ? Brushes.White : Brushes.Black;
                }
            }

            btnTilbage.IsEnabled = currentStep > 1;
            btnNaeste.Content = (currentStep == totalSteps) ? "Færdig" : "Næste";
            btnGemKladde.IsEnabled = true;
        }

        /// <summary>
        /// Validerer det aktuelle trin.
        /// </summary>
        private bool ValiderNuværendeTrin()
        {
            switch (currentStep)
            {
                case 1:
                    return (ucTrin1Basisoplysninger as Trin1BasisoplysningerView)?.ValiderFelter() ?? false;
                case 2:
                    return (ucTrin2ProduktInfo as Trin2ProduktInfoView)?.ValiderFelter() ?? false;
                case 3:
                    return (ucTrin3Compliance as Trin3ComplianceView)?.ValiderFelter() ?? false;
                case 4:
                    return (ucTrin4ObligatoriskeFelter as Trin4ObligatoriskeFelterView)?.ValiderFelter() ?? false;
                case 5:
                    return (ucTrin5ValgfriFelter as Trin5ValgfriFelterView)?.ValiderFelter() ?? false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Rydder alle formular-felter efter vellykket produktoprettelse.
        /// </summary>
        private void RydFormular()
        {
            try
            {
                // Reset each step
                (ucTrin1Basisoplysninger as Trin1BasisoplysningerView)?.txtProduktNavn.Clear();
                (ucTrin1Basisoplysninger as Trin1BasisoplysningerView)?.txtVareNummer.Clear();
                (ucTrin1Basisoplysninger as Trin1BasisoplysningerView)?.txtColiStoerrelseAntal.Clear();
                if (ucTrin1Basisoplysninger is Trin1BasisoplysningerView trin1)
                {
                    trin1.cmbVaregruppe.SelectedIndex = -1;
                    trin1.cmbSaeson.SelectedIndex = -1;
                    trin1.cmbOprindelsesland.SelectedIndex = -1;
                    trin1.cmbLeverandoer.SelectedIndex = -1;
                    trin1.cmbDesigner.SelectedIndex = -1;
                    trin1.cmbBeskrivelse.SelectedIndex = -1;
                }

                (ucTrin4ObligatoriskeFelter as Trin4ObligatoriskeFelterView)?.RydFelter();

                // Reset to first step
                currentStep = 1;
                UpdateStepUI();

                // Reset edit mode
                isEditMode = false;
                editingProductId = null;
                txtEditMode.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved nulstilling af formular: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Helper Methods

        /// <summary>
        /// Hjælpemetode til at hente navnet fra et valgt ListeItem objekt
        /// </summary>
        private string GetSelectedItemName(object selectedItem)
        {
            if (selectedItem is ListeItem item)
                return item.Navn;
            return string.Empty;
        }

        /// <summary>
        /// Hjælpemetode til at parse tekst til int sikkert
        /// </summary>
        private int ParseIntSafely(string value)
        {
            if (int.TryParse(value, out int result))
                return result;
            return 0;
        }

        /// <summary>
        /// Hjælpemetode til at parse tekst til decimal sikkert
        /// </summary>
        private decimal ParseDecimalSafely(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0m;

            // Handle both comma and period as decimal separator
            value = value.Replace(',', '.');

            if (decimal.TryParse(value, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal result))
                return result;
            return 0m;
        }

        /// <summary>
        /// Viser en liste over kladder til brugeren
        /// </summary>
        private void ShowDraftProducts()
        {
            try
            {
                var drafts = viewModel.GetDraftProducts();
                if (drafts.Any())
                {
                    string draftList = "Fundne kladder:\n\n";
                    foreach (var draft in drafts)
                    {
                        draftList += $"ID: {draft.ProductId} - {draft.Name} ({draft.Status})\n";
                    }
                    MessageBox.Show(draftList, "Kladder", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ingen kladder fundet.", "Kladder", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved hentning af kladder: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        /// <summary>
        /// Menu eller button handler for at vise kladder (hvis du vil tilføje denne funktionalitet)
        /// </summary>
        private void btnVisKladder_Click(object sender, RoutedEventArgs e)
        {
            ShowDraftProducts();
        }
    }
}