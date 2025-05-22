using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using ProduktOprettelse.Services;

namespace ProduktOprettelse.Views
{
    /// <summary>
    /// View for produktinformation i trin 2 af produktoprettelsesprocessen.
    /// Dette trin indeholder detaljeret information om produktet som pris og mængde.
    /// </summary>
    public partial class Trin2ProduktInfoView : UserControl
    {
        /// <summary>
        /// Initialiserer en ny instans af <see cref="Trin2ProduktInfoView"/> klassen.
        /// </summary>
        public Trin2ProduktInfoView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Validerer tekstinput så kun numeriske værdier med ét decimaltegn tillades.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter der indeholder tekstinputtet.</param>
        private void NumericWithDecimal(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string currentText = textBox.Text;
                string newText = currentText.Insert(textBox.CaretIndex, e.Text);

                // Tillad kun tal og ét decimaltegn (komma eller punktum afhængig af kultur)
                // Denne regex tillader tal, og ét enkelt decimaltegn.
                // For dansk kultur (,) brug: ^[0-9]*(,[0-9]*)?$
                // For engelsk kultur (.) brug: ^[0-9]*(\.[0-9]*)?$
                // Her bruges en mere generel, der tillader begge, men kun én:
                Regex regex = new Regex(@"^[0-9]*(?:[\.\,][0-9]*)?$");

                if (!regex.IsMatch(newText))
                {
                    e.Handled = true;
                }
                // Forhindre mere end ét decimaltegn
                else if ((e.Text == "," || e.Text == ".") && (currentText.Contains(",") || currentText.Contains(".")))
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Får den valgte kategori fra dropdownlisten.
        /// </summary>
        public object SelectedKategori => cmbKategori.SelectedItem;

        /// <summary>
        /// Får den valgte farve fra dropdownlisten.
        /// </summary>
        public object SelectedFarve => cmbFarve.SelectedItem;

        /// <summary>
        /// Får det valgte materiale fra dropdownlisten.
        /// </summary>
        public object SelectedMateriale => cmbMateriale.SelectedItem;

        /// <summary>
        /// Får den valgte enhed fra dropdownlisten.
        /// </summary>
        public object SelectedEnhed => cmbEnhed.SelectedItem;

        /// <summary>
        /// Får produktets højde fra tekstfeltet.
        /// </summary>
        public string Hoejde => txtHoejde?.Text ?? string.Empty;

        /// <summary>
        /// Får produktets bredde fra tekstfeltet.
        /// </summary>
        public string Bredde => txtBredde?.Text ?? string.Empty;

        /// <summary>
        /// Får produktets dybde fra tekstfeltet.
        /// </summary>
        public string Dybde => txtDybde?.Text ?? string.Empty;

        /// <summary>
        /// Får produktets vægt fra tekstfeltet.
        /// </summary>
        public string Vaegt => txtVaegt?.Text ?? string.Empty;

        /// <summary>
        /// Får produktets diameter fra tekstfeltet.
        /// </summary>
        public string Diameter => txtDiameter?.Text ?? string.Empty;

        /// <summary>
        /// Validerer om alle påkrævede felter er udfyldt korrekt.
        /// </summary>
        /// <returns>Sand hvis alle felter er gyldige, falsk ellers.</returns>
        public bool ValiderFelter()
        {
            // Da dette er trin 2, er der måske ikke så mange obligatoriske felter
            // Tilføj mere validering efter behov
            bool kategoriValid = SelectedKategori != null;
            bool materialeValid = SelectedMateriale != null;

            // Man kunne også validere at numeriske felter enten er tomme eller gyldige tal
            bool vaegValid = string.IsNullOrEmpty(Vaegt) || decimal.TryParse(Vaegt.Replace(',', '.'), out _);

            return kategoriValid && materialeValid && vaegValid;
        }
    }
}