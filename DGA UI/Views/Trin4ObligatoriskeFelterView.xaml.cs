using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using ProduktOprettelse.Services;

namespace ProduktOprettelse.Views
{
    /// <summary>
    /// View for obligatoriske felter i trin 4 af produktoprettelsesprocessen.
    /// Dette trin indeholder felter der er påkrævet for at produktet kan oprettes,
    /// samt yderligere produktoplysninger.
    /// </summary>
    public partial class Trin4ObligatoriskeFelterView : UserControl
    {
        /// <summary>
        /// Initialiserer en ny instans af <see cref="Trin4ObligatoriskeFelterView"/> klassen.
        /// </summary>
        public Trin4ObligatoriskeFelterView()
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

                // Tillad kun tal og ét decimaltegn
                Regex regex = new Regex(@"^[0-9]*(?:[\.\,][0-9]*)?$");

                if (!regex.IsMatch(newText))
                {
                    e.Handled = true;
                }
                else if ((e.Text == "," || e.Text == ".") && (currentText.Contains(",") || currentText.Contains(".")))
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Validerer tekstinput så kun numeriske værdier tillades.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter der indeholder tekstinputtet.</param>
        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        #region Oprindelige Properties

        /// <summary>
        /// Får kostpris fra tekstfeltet.
        /// </summary>
        public string Kostpris => txtKostpris?.Text ?? string.Empty;

        /// <summary>
        /// Får salgspris fra tekstfeltet.
        /// </summary>
        public string Salgspris => txtSalgspris?.Text ?? string.Empty;

        /// <summary>
        /// Får om produktet er momspligtigt.
        /// </summary>
        public bool Momspligtig => chkMomspligtig?.IsChecked ?? false;

        /// <summary>
        /// Får startdatoen for produktet.
        /// </summary>
        public System.DateTime? Startdato => dpStartdato?.SelectedDate;

        /// <summary>
        /// Får slutdatoen for produktet.
        /// </summary>
        public System.DateTime? Slutdato => dpSlutdato?.SelectedDate;

        /// <summary>
        /// Får om produktet er på lager.
        /// </summary>
        public bool PaaLager => chkPaaLager?.IsChecked ?? false;

        #endregion

        #region Nye Properties

        /// <summary>
        /// Får leverandørens produktnummer.
        /// </summary>
        public string LeverandorProduktNr => txtLeverandorProduktNr?.Text ?? string.Empty;

        /// <summary>
        /// Får customer clearance nummer.
        /// </summary>
        public string CustomerClearanceNo => txtCustomerClearanceNo?.Text ?? string.Empty;

        /// <summary>
        /// Får customer clearance procent.
        /// </summary>
        public string CustomerClearancePct => txtCustomerClearancePct?.Text ?? string.Empty;

        /// <summary>
        /// Får inner carton værdi.
        /// </summary>
        public string InnerCarton => txtInnerCarton?.Text ?? string.Empty;

        /// <summary>
        /// Får outer carton værdi.
        /// </summary>
        public string OuterCarton => txtOuterCarton?.Text ?? string.Empty;

        /// <summary>
        /// Får EAN koder.
        /// </summary>
        public string EANCodes => txtEANCodes?.Text ?? string.Empty;

        /// <summary>
        /// Får hovedgruppe.
        /// </summary>
        public string Maingroup => txtMaingroup?.Text ?? string.Empty;

        /// <summary>
        /// Får hovedkategori.
        /// </summary>
        public string Maincategory => txtMaincategory?.Text ?? string.Empty;

        /// <summary>
        /// Får materiale.
        /// </summary>
        public string Material => txtMaterial?.Text ?? string.Empty;

        /// <summary>
        /// Får bruttovægt.
        /// </summary>
        public string GrossWeight => txtGrossWeight?.Text ?? string.Empty;

        /// <summary>
        /// Får emballage højde.
        /// </summary>
        public string PackingHeight => txtPackingHeight?.Text ?? string.Empty;

        /// <summary>
        /// Får emballage bredde/længde.
        /// </summary>
        public string PackingWidth => txtPackingWidth?.Text ?? string.Empty;

        /// <summary>
        /// Får emballage dybde.
        /// </summary>
        public string PackingDepth => txtPackingDepth?.Text ?? string.Empty;

        /// <summary>
        /// Får KI højde.
        /// </summary>
        public string KIHeight => txtKIHeight?.Text ?? string.Empty;

        /// <summary>
        /// Får KY højde.
        /// </summary>
        public string KYHeight => txtKYHeight?.Text ?? string.Empty;

        /// <summary>
        /// Får KY bredde/længde.
        /// </summary>
        public string KYWidth => txtKYWidth?.Text ?? string.Empty;

        /// <summary>
        /// Får om produktet er opvaskemaskine-egnet.
        /// </summary>
        public bool Dishwasher => chkDishwasher?.IsChecked ?? false;

        /// <summary>
        /// Får om produktet er mikrobølgeovn-egnet.
        /// </summary>
        public bool Microwave => chkMicrowave?.IsChecked ?? false;

        /// <summary>
        /// Får BSCI værdi.
        /// </summary>
        public string BSCI => txtBSCI?.Text ?? string.Empty;

        /// <summary>
        /// Får Recycled værdi.
        /// </summary>
        public string Recycled => txtRecycled?.Text ?? string.Empty;

        /// <summary>
        /// Får LED El-retur værdi.
        /// </summary>
        public string LEDElRetur => txtLEDElRetur?.Text ?? string.Empty;

        /// <summary>
        /// Får om produktet er Svanemærket.
        /// </summary>
        public bool Svanemaerket => chkSvanemaerket?.IsChecked ?? false;

        /// <summary>
        /// Får om produktet har Grüner Punkt.
        /// </summary>
        public bool GrunerPunkt => chkGrunerPunkt?.IsChecked ?? false;

        /// <summary>
        /// Får om produktet har CE-mærkning.
        /// </summary>
        public bool CE => chkCE?.IsChecked ?? false;

        /// <summary>
        /// Får energimærkning.
        /// </summary>
        public string ENERGIMAERKNING => txtENERGIMAERKNING?.Text ?? string.Empty;

        /// <summary>
        /// Får andre oplysninger.
        /// </summary>
        public string Other => txtOther?.Text ?? string.Empty;

        /// <summary>
        /// Får om produktet er FSC100 mærket.
        /// </summary>
        public bool FSC100 => chkFSC100?.IsChecked ?? false;

        /// <summary>
        /// Får om produktet er FSCMIX70 mærket.
        /// </summary>
        public bool FSCMIX70 => chkFSCMIX70?.IsChecked ?? false;

        /// <summary>
        /// Giv produktet et hangtag eller sticker
        /// </summary>
        public string HangtagsStickers => cmbHangtagsStickers?.SelectedItem?.ToString() ?? string.Empty;


        /// <summary>
        /// Får gruppe.
        /// </summary>
        public string Group => txtGroup?.Text ?? string.Empty;

        /// <summary>
        /// Produktserie
        /// </summary>

        public string Produktserie => cmbProduktserie?.SelectedItem?.ToString() ?? string.Empty;

        #endregion

        /// <summary>
        /// Validerer om alle påkrævede felter er udfyldt korrekt.
        /// </summary>
        /// <returns>Sand hvis alle felter er gyldige, falsk ellers.</returns>
        public bool ValiderFelter()
        {
            // Implementer validering af alle nødvendige felter
            bool kostprisValid = !string.IsNullOrWhiteSpace(Kostpris);
            bool salgsprisValid = !string.IsNullOrWhiteSpace(Salgspris);
            bool startdatoValid = Startdato.HasValue;

            // Vi kunne også validere at priserne er gyldige tal
            bool kostprisTalValid = decimal.TryParse(Kostpris.Replace(',', '.'), out _);
            bool salgsprisTalValid = decimal.TryParse(Salgspris.Replace(',', '.'), out _);

            // Og at slutdato er efter startdato hvis den er angivet
            bool datoerValide = !Slutdato.HasValue || (Startdato.HasValue && Slutdato.Value >= Startdato.Value);
            

            // Tilføj validering af vægtfelter og målefelter
            bool grossWeightValid = string.IsNullOrWhiteSpace(GrossWeight) || decimal.TryParse(GrossWeight.Replace(',', '.'), out _);
            bool packingHeightValid = string.IsNullOrWhiteSpace(PackingHeight) || decimal.TryParse(PackingHeight.Replace(',', '.'), out _);
            bool packingWidthValid = string.IsNullOrWhiteSpace(PackingWidth) || decimal.TryParse(PackingWidth.Replace(',', '.'), out _);
            bool packingDepthValid = string.IsNullOrWhiteSpace(PackingDepth) || decimal.TryParse(PackingDepth.Replace(',', '.'), out _);
            bool kiHeightValid = string.IsNullOrWhiteSpace(KIHeight) || decimal.TryParse(KIHeight.Replace(',', '.'), out _);
            bool kyHeightValid = string.IsNullOrWhiteSpace(KYHeight) || decimal.TryParse(KYHeight.Replace(',', '.'), out _);
            bool kyWidthValid = string.IsNullOrWhiteSpace(KYWidth) || decimal.TryParse(KYWidth.Replace(',', '.'), out _);

            // Valider numeriske felter
            bool customerClearanceNoValid = string.IsNullOrWhiteSpace(CustomerClearanceNo) || int.TryParse(CustomerClearanceNo, out _);
            bool customerClearancePctValid = string.IsNullOrWhiteSpace(CustomerClearancePct) || decimal.TryParse(CustomerClearancePct.Replace(',', '.'), out _);
            bool innerCartonValid = string.IsNullOrWhiteSpace(InnerCarton) || int.TryParse(InnerCarton, out _);

            // Valider at der er valgt en produktserie, handtags eller stickers og at der er indtastet en gruppe

            bool produktserieValid = cmbProduktserie?.SelectedItem != null;
            bool hangtagsStickersValid = cmbHangtagsStickers?.SelectedItem != null;
            bool groupValid = !string.IsNullOrWhiteSpace(Group);
            bool produktLogoValid = cmbProduktLogo?.SelectedItem != null;

            // Samlet validering
            return kostprisValid && salgsprisValid && startdatoValid &&
                   kostprisTalValid && salgsprisTalValid && datoerValide &&
                   grossWeightValid && packingHeightValid &&
                   packingWidthValid && packingDepthValid && kiHeightValid &&
                   kyHeightValid && kyWidthValid && customerClearanceNoValid &&
                   customerClearancePctValid && innerCartonValid && produktserieValid &&
                   hangtagsStickersValid && groupValid && produktLogoValid;
        }

        /// <summary>
        /// Validerer og konverterer et tekstfelt til en decimal værdi.
        /// </summary>
        /// <param name="value">Tekstværdien der skal konverteres.</param>
        /// <param name="result">Den konverterede decimal værdi.</param>
        /// <returns>Sand hvis konverteringen lykkedes, falsk ellers.</returns>
        private bool TryParseDecimal(string value, out decimal result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = 0;
                return false;
            }

            return decimal.TryParse(value.Replace(',', '.'), out result);
        }

        /// <summary>
        /// Rydder alle felter i viewet.
        /// </summary>
        public void RydFelter()
        {
            txtKostpris.Text = string.Empty;
            txtSalgspris.Text = string.Empty;
            chkMomspligtig.IsChecked = true;
            dpStartdato.SelectedDate = null;
            dpSlutdato.SelectedDate = null;
            chkPaaLager.IsChecked = true;
            txtLeverandorProduktNr.Text = string.Empty;
            txtCustomerClearanceNo.Text = string.Empty;
            txtCustomerClearancePct.Text = string.Empty;
            txtInnerCarton.Text = string.Empty;
            txtOuterCarton.Text = string.Empty;
            txtEANCodes.Text = string.Empty;
            txtMaingroup.Text = string.Empty;
            txtMaincategory.Text = string.Empty;
            txtMaterial.Text = string.Empty;
            txtGrossWeight.Text = string.Empty;
            txtPackingHeight.Text = string.Empty;
            txtPackingWidth.Text = string.Empty;
            txtPackingDepth.Text = string.Empty;
            txtKIHeight.Text = string.Empty;
            txtKYHeight.Text = string.Empty;
            txtKYWidth.Text = string.Empty;
            chkDishwasher.IsChecked = false;
            chkMicrowave.IsChecked = false;
            txtBSCI.Text = string.Empty;
            txtRecycled.Text = string.Empty;
            txtLEDElRetur.Text = string.Empty;
            chkSvanemaerket.IsChecked = false;
            chkGrunerPunkt.IsChecked = false;
            chkCE.IsChecked = false;
            txtENERGIMAERKNING.Text = string.Empty;
            txtOther.Text = string.Empty;
            chkFSC100.IsChecked = false;
            chkFSCMIX70.IsChecked = false;
            cmbHangtagsStickers.SelectedIndex = -1;
            txtGroup.Text = string.Empty;
            cmbProduktserie.SelectedIndex = -1;
            cmbProduktLogo.SelectedIndex = -1;
        }
    }
}