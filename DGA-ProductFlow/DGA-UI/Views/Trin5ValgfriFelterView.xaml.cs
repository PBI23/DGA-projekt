using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using ProduktOprettelse.Services;

namespace ProduktOprettelse.Views
{
    /// <summary>
    /// View for valgfrie felter i trin 5 af produktoprettelsesprocessen.
    /// Dette trin indeholder felter der ikke er obligatoriske men kan tilføje ekstra information til produktet.
    /// </summary>
    public partial class Trin5ValgfriFelterView : UserControl
    {
        /// <summary>
        /// Initialiserer en ny instans af <see cref="Trin5ValgfriFelterView"/> klassen.
        /// </summary>
        public Trin5ValgfriFelterView()
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
        /// Får barcode/EAN fra tekstfeltet.
        /// </summary>
        public string Barcode => txtBarcode?.Text ?? string.Empty;

        /// <summary>
        /// Får leverandørens varenummer fra tekstfeltet.
        /// </summary>
        public string LeverandoerVarenummer => txtLeverandoerVarenummer?.Text ?? string.Empty;

        /// <summary>
        /// Får rabatkode fra tekstfeltet.
        /// </summary>
        public string Rabatkode => txtRabatkode?.Text ?? string.Empty;

        /// <summary>
        /// Får tags/søgeord fra tekstfeltet.
        /// </summary>
        public string Tags => txtTags?.Text ?? string.Empty;

        /// <summary>
        /// Får bemærkninger fra tekstfeltet.
        /// </summary>
        public string Bemaerkninger => txtBemaerkninger?.Text ?? string.Empty;

        /// <summary>
        /// Validerer valgfrie felter for korrekthed (selv om de ikke er obligatoriske).
        /// </summary>
        /// <returns>Sand hvis alle udfyldte felter er gyldige, falsk ellers.</returns>
        public bool ValiderFelter()
        {
            // Da felterne er valgfrie, returnerer denne altid true
            // men man kunne validere om de udfyldte felter overholder bestemte formater

            // For eksempel kunne vi validere at barcode kun indeholder tal og har en bestemt længde hvis udfyldt
            //bool barcodeValid = string.IsNullOrEmpty(Barcode) ||
            //(Regex.IsMatch(Barcode, @"^[0-9]+$") && (Barcode.Length == 8 || Barcode.Length == 13));

            //return barcodeValid;
            return true;
        }
    }
}