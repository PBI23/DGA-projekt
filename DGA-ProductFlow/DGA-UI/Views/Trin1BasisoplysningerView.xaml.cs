using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using ProduktOprettelse.Services;

namespace ProduktOprettelse.Views
{
    /// <summary>
    /// View for basale produktoplysninger i trin 1 af produktoprettelsen.
    /// Dette trin indeholder grundlæggende information som produktnavn og varegruppe.
    /// </summary>
    public partial class Trin1BasisoplysningerView : UserControl
    {
        /// <summary>
        /// Initialiserer en ny instans af <see cref="Trin1BasisoplysningerView"/> klassen.
        /// </summary>
        public Trin1BasisoplysningerView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Validerer tekstinput så kun numeriske værdier tillades.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter der indeholder tekstinputtet.</param>
        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            // Matcher alt der ikke er et tal
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Får produktets navn fra tekstfeltet.
        /// </summary>
        public string ProduktNavn => txtProduktNavn.Text;

        /// <summary>
        /// Får DGA varenummer fra tekstfeltet.
        /// </summary>
        public string VareNummer => txtVareNummer.Text;

        /// <summary>
        /// Får coli-størrelse-antal fra tekstfeltet.
        /// </summary>
        public string ColiStoerrelseAntal => txtColiStoerrelseAntal.Text;

        /// <summary>
        /// Får den valgte varegruppe fra dropdownlisten.
        /// </summary>
        public object SelectedVaregruppe => cmbVaregruppe.SelectedItem;

        /// <summary>
        /// Får den valgte sæson fra dropdownlisten.
        /// </summary>
        public object SelectedSaeson => cmbSaeson.SelectedItem;

        /// <summary>
        /// Får det valgte oprindelsesland fra dropdownlisten.
        /// </summary>
        public object SelectedOprindelsesland => cmbOprindelsesland.SelectedItem;

        /// <summary>
        /// Får den valgte leverandør fra dropdownlisten.
        /// </summary>
        public object SelectedLeverandoer => cmbLeverandoer.SelectedItem;

        /// <summary>
        /// Får den valgte designer fra dropdownlisten.
        /// </summary>
        public object SelectedDesigner => cmbDesigner.SelectedItem;

        /// <summary>
        /// Får den valgte beskrivelse fra dropdownlisten.
        /// </summary>
        public object SelectedBeskrivelse => cmbBeskrivelse.SelectedItem;

        /// <summary>
        /// Validerer om alle påkrævede felter er udfyldt korrekt.
        /// </summary>
        /// <returns>Sand hvis alle felter er gyldige, falsk ellers.</returns>
        /// <summary>
        /// Validerer om alle påkrævede felter er udfyldt korrekt.
        /// </summary>
        /// <returns>Sand hvis alle felter er gyldige, falsk ellers.</returns>
        public bool ValiderFelter()
        {
            var rules = new List<ValidationService.ValidationRule>
            {
                new ValidationService.ValidationRule
                {
                    FieldName = "ProduktNavn",
                    Condition = () => !string.IsNullOrWhiteSpace(ProduktNavn),
                    ErrorMessage = "Produktnavn skal udfyldes.",
                    Control = txtProduktNavn
                },

                new ValidationService.ValidationRule
                {
                    FieldName = "VareNummer",
                    Condition = () => !string.IsNullOrWhiteSpace(VareNummer),
                    ErrorMessage = "DGA varenummer skal udfyldes.",
                    Control = txtVareNummer
                },
                new ValidationService.ValidationRule
                {
                    FieldName = "Varegruppe",
                    Condition = () => SelectedVaregruppe != null,
                    ErrorMessage = "Vælg venligst en varegruppe.",
                    Control = cmbVaregruppe
                },
                new ValidationService.ValidationRule
                {
                    FieldName = "Sæson",
                    Condition = () => SelectedSaeson != null,
                    ErrorMessage = "Vælg venligst en sæson.",
                    Control = cmbSaeson
                },
                new ValidationService.ValidationRule
                {
                    FieldName = "Oprindelsesland",
                    Condition = () => SelectedOprindelsesland != null,
                    ErrorMessage = "Vælg venligst et oprindelsesland.",
                    Control = cmbOprindelsesland
                },

                new ValidationService.ValidationRule
                {
                    FieldName = "Leverandør",
                    Condition = () => SelectedLeverandoer != null,
                    ErrorMessage = "Vælg venligst en leverandør.",
                    Control = cmbLeverandoer
                },

                new ValidationService.ValidationRule
                {
                    FieldName = "Designer",
                    Condition = () => SelectedDesigner != null,
                    ErrorMessage = "Vælg venligst en designer.",
                    Control = cmbDesigner
                },

                new ValidationService.ValidationRule
                {
                    FieldName = "Beskrivelse",
                    Condition = () => SelectedBeskrivelse != null,
                    ErrorMessage = "Vælg venligst en beskrivelse.",
                    Control = cmbBeskrivelse
                },

                new ValidationService.ValidationRule
                {
                    FieldName = "ColiStørrelseAntal",
                    Condition = () => !string.IsNullOrWhiteSpace(ColiStoerrelseAntal),
                    ErrorMessage = "Coli-størrelse-antal skal udfyldes.",
                    Control = txtColiStoerrelseAntal
                }
                // Add more rules if needed here, and use the same pattern
            };

            return ValidationService.ValidateRules(rules);
        }
    }
}