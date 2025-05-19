using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProduktOprettelse.Views;

namespace ProduktOprettelse
{
    /// <summary>
    /// Hovedvindue for produkt-oprettelses applikationen.
    /// Styrer navigation mellem trin og indsamling af data fra alle views.
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentStep = 1;
        private const int totalSteps = 5;
        private Button[] stepButtons;
        private UserControl[] stepViews;
        private ProduktViewModel viewModel;

        /// <summary>
        /// Initialiserer en ny instans af <see cref="MainWindow"/> klassen.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Initialiserer view-model og sætter den som DataContext
            viewModel = new ProduktViewModel();
            this.DataContext = viewModel;

            // Start at indlæse data til dropdown-lister
            LoadDataAsync();

            // Initialiserer arrays med trin-knapper og views for nemmere håndtering
            stepButtons = new Button[] { btnTrin1, btnTrin2, btnTrin3, btnTrin4, btnTrin5 };
            stepViews = new UserControl[] { ucTrin1Basisoplysninger, ucTrin2ProduktInfo, ucTrin3Compliance, ucTrin4ObligatoriskeFelter, ucTrin5ValgfriFelter };

            UpdateStepUI();
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
        /// Håndterer klik på en trin-knap og skifter til det valgte trin.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
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

        /// <summary>
        /// Konverterer et trin-tag til et numerisk trin-nummer.
        /// </summary>
        /// <param name="stepTag">Tag-streng for trinnet.</param>
        /// <returns>Numerisk trin-nummer.</returns>
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
        /// Håndterer klik på "Næste" knappen.
        /// Validerer det nuværende trin og går videre til næste.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void btnNaeste_Click(object sender, RoutedEventArgs e)
        {
            if (!ValiderNuværendeTrin())
            {
                MessageBox.Show("Udfyld venligst alle påkrævede felter korrekt, før du går videre.",
                                "Validering fejlede",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            if (currentStep < totalSteps)
            {
                currentStep++;
                UpdateStepUI();
            }
            else
            {
                // Sidste trin: Opret produktet
                if (OpretProdukt())
                {
                    MessageBox.Show("Produkt oprettet succesfuldt!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                    RydFormular();
                }
            }
        }

        /// <summary>
        /// Håndterer klik på "Tilbage" knappen.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
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
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void btnGemKladde_Click(object sender, RoutedEventArgs e)
        {
            // Implementer logik for at gemme kladde
            GemKladde();
            MessageBox.Show("Kladde gemt succesfuldt!", "Gem kladde", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Opdaterer UI baseret på det aktuelle trin.
        /// </summary>
        private void UpdateStepUI()
        {
            for (int i = 0; i < totalSteps; i++)
            {
                if (stepViews[i] != null) // Sikrer at UserControl er initialiseret
                {
                    stepViews[i].Visibility = (i == currentStep - 1) ? Visibility.Visible : Visibility.Collapsed;
                }
                if (stepButtons[i] != null)
                {
                    // Aktiv trin får grøn (#4CAF50) farve, inaktive får grå (#E0E0E0)
                    stepButtons[i].Background = (i == currentStep - 1)
                        ? new SolidColorBrush(Color.FromRgb(76, 175, 80))
                        : new SolidColorBrush(Color.FromRgb(224, 224, 224));

                    stepButtons[i].Foreground = (i == currentStep - 1) ? Brushes.White : Brushes.Black;
                }
            }

            // Opdater knapper baseret på det aktuelle trin
            btnTilbage.IsEnabled = currentStep > 1;
            btnNaeste.Content = (currentStep == totalSteps) ? "Færdig" : "Næste";
            btnGemKladde.IsEnabled = true; // Eller baser på logik
        }

        /// <summary>
        /// Validerer det aktuelle trin.
        /// </summary>
        /// <returns>Sand hvis trinnet er valideret, falsk ellers.</returns>
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
        /// Indsamler data fra alle trin og opretter produktet.
        /// </summary>
        /// <returns>Sand hvis produktet blev oprettet, falsk ellers.</returns>
        private bool OpretProdukt()
        {
            try
            {
                // Indsaml data fra alle views
                var basisOplysninger = (Trin1BasisoplysningerView)ucTrin1Basisoplysninger;
                var produktInfo = (Trin2ProduktInfoView)ucTrin2ProduktInfo;
                var compliance = (Trin3ComplianceView)ucTrin3Compliance;
                var obligatoriskeFelter = (Trin4ObligatoriskeFelterView)ucTrin4ObligatoriskeFelter;
                var valgfriFelter = (Trin5ValgfriFelterView)ucTrin5ValgfriFelter;

                // Samler data fra alle trin
                var produkt = new
                {
                    // Trin 1 - Basisoplysninger
                    ProduktNavn = basisOplysninger.ProduktNavn,
                    Varegruppe = basisOplysninger.SelectedVaregruppe,
                    Saeson = basisOplysninger.SelectedSaeson,
                    VareNummer = basisOplysninger.VareNummer,
                    Oprindelsesland = basisOplysninger.SelectedOprindelsesland,
                    Leverandoer = basisOplysninger.SelectedLeverandoer,
                    Designer = basisOplysninger.SelectedDesigner,
                    Beskrivelse = basisOplysninger.SelectedBeskrivelse,
                    ColiStoerrelseAntal = basisOplysninger.ColiStoerrelseAntal,

                    // Trin 2 - Produktinfo
                    Kategori = produktInfo.SelectedKategori,
                    Farve = produktInfo.SelectedFarve,
                    Materiale = produktInfo.SelectedMateriale,
                    Hoejde = produktInfo.Hoejde,
                    Bredde = produktInfo.Bredde,
                    Dybde = produktInfo.Dybde,
                    Vaegt = produktInfo.Vaegt,
                    Diameter = produktInfo.Diameter,
                    Enhed = produktInfo.SelectedEnhed,

                    // Trin 3 - Compliance
                    IsFKM = compliance.IsFKM,
                    FKMSpoergsmaal1 = compliance.FKMSpoergsmaal1Svar,
                    FKMSpoergsmaal2 = compliance.FKMSpoergsmaal2Svar,
                    FKMSpoergsmaal3 = compliance.FKMSpoergsmaal3Svar,
                    // Her kan du tilføje flere compliance-fields efter behov

                    // Trin 4 - Obligatoriske felter
                    Kostpris = obligatoriskeFelter.Kostpris,
                    Salgspris = obligatoriskeFelter.Salgspris,
                    Momspligtig = obligatoriskeFelter.Momspligtig,
                    Startdato = obligatoriskeFelter.Startdato,
                    Slutdato = obligatoriskeFelter.Slutdato,
                    PaaLager = obligatoriskeFelter.PaaLager,

                    // Trin 5 - Valgfri felter
                    Barcode = valgfriFelter.Barcode,
                    LeverandoerVarenummer = valgfriFelter.LeverandoerVarenummer,
                    Rabatkode = valgfriFelter.Rabatkode,
                    Tags = valgfriFelter.Tags,
                    Bemaerkninger = valgfriFelter.Bemaerkninger
                };

                // TODO: Implementér produktoprettelse (gem til database, ERP-system osv.)
                

                // Simulerer produktoprettelse
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved oprettelse af produkt: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Gemmer det aktuelle produkt som kladde.
        /// </summary>
        private void GemKladde()
        {
            try
            {
                // Indsaml data fra alle trin
                var alleData = IndsamlAlleData();

                // TODO: Implementér kladdefunktionalitet
                // 1. Gem data til database eller fil
                // 2. Tilføj tidsstempel eller unikt ID til kladden

                // Simulerer gemning af kladde
                Console.WriteLine("Kladde gemt: " + DateTime.Now);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved gemning af kladde: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Rydder alle formular-felter efter vellykket produktoprettelse.
        /// </summary>
        private void RydFormular()
        {
            try
            {
                // Nulstil hvert trin - her må man implementere en RydFelter-metode i hver UserControl
                // Eksempel:
                // (ucTrin1Basisoplysninger as Trin1BasisoplysningerView)?.RydFelter();
                // (ucTrin2ProduktInfo as Trin2ProduktInfoView)?.RydFelter();
                // osv.

                // Nulstil dropdown-valg til første trin
                currentStep = 1;
                UpdateStepUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved nulstilling af formular: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Indsamler alle data fra alle trin til brug ved oprettelse eller kladde.
        /// </summary>
        /// <returns>Et objekt der indeholder alle produkt data.</returns>
        private object IndsamlAlleData()
        {
            // Indsaml data fra alle views - samme som i OpretProdukt-metoden
            var basisOplysninger = (Trin1BasisoplysningerView)ucTrin1Basisoplysninger;
            var produktInfo = (Trin2ProduktInfoView)ucTrin2ProduktInfo;
            var compliance = (Trin3ComplianceView)ucTrin3Compliance;
            var obligatoriskeFelter = (Trin4ObligatoriskeFelterView)ucTrin4ObligatoriskeFelter;
            var valgfriFelter = (Trin5ValgfriFelterView)ucTrin5ValgfriFelter;

            // Samler data fra alle trin
            var produkt = new
            {
                // Trin 1 - Basisoplysninger
                ProduktNavn = basisOplysninger.ProduktNavn,
                Varegruppe = basisOplysninger.SelectedVaregruppe,
                Saeson = basisOplysninger.SelectedSaeson,
                VareNummer = basisOplysninger.VareNummer,
                Oprindelsesland = basisOplysninger.SelectedOprindelsesland,
                Leverandoer = basisOplysninger.SelectedLeverandoer,
                Designer = basisOplysninger.SelectedDesigner,
                Beskrivelse = basisOplysninger.SelectedBeskrivelse,
                ColiStoerrelseAntal = basisOplysninger.ColiStoerrelseAntal,

                // Trin 2 - Produktinfo
                Kategori = produktInfo.SelectedKategori,
                Farve = produktInfo.SelectedFarve,
                Materiale = produktInfo.SelectedMateriale,
                Hoejde = produktInfo.Hoejde,
                Bredde = produktInfo.Bredde,
                Dybde = produktInfo.Dybde,
                Vaegt = produktInfo.Vaegt,
                Diameter = produktInfo.Diameter,
                Enhed = produktInfo.SelectedEnhed,

                // Trin 3 - Compliance
                IsFKM = compliance.IsFKM,
                FKMSpoergsmaal1 = compliance.FKMSpoergsmaal1Svar,
                FKMSpoergsmaal2 = compliance.FKMSpoergsmaal2Svar,
                FKMSpoergsmaal3 = compliance.FKMSpoergsmaal3Svar,
                // Her kan du tilføje flere compliance-fields efter behov

                // Trin 4 - Obligatoriske felter
                Kostpris = obligatoriskeFelter.Kostpris,
                Salgspris = obligatoriskeFelter.Salgspris,
                Momspligtig = obligatoriskeFelter.Momspligtig,
                Startdato = obligatoriskeFelter.Startdato,
                Slutdato = obligatoriskeFelter.Slutdato,
                PaaLager = obligatoriskeFelter.PaaLager,

                // Trin 5 - Valgfri felter
                Barcode = valgfriFelter.Barcode,
                LeverandoerVarenummer = valgfriFelter.LeverandoerVarenummer,
                Rabatkode = valgfriFelter.Rabatkode,
                Tags = valgfriFelter.Tags,
                Bemaerkninger = valgfriFelter.Bemaerkninger
            };

            return produkt;
        }
    }
}