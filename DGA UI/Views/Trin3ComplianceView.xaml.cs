using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProduktOprettelse.Services;

namespace ProduktOprettelse.Views
{
    /// <summary>
    /// View for compliance-relateret information i trin 3 af produktoprettelsesprocessen.
    /// Dette trin indeholder information om produktets overholdelse af forskellige regler og standarder.
    /// </summary>
    public partial class Trin3ComplianceView : UserControl
    {
        /// <summary>
        /// Initialiserer en ny instans af <see cref="Trin3ComplianceView"/> klassen.
        /// </summary>
        public Trin3ComplianceView()
        {
            InitializeComponent();

            
        }

        /// <summary>
        /// Håndterer mouse wheel events og sender dem til hovedscrollviewet
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        

        #region FKM Handlers

        /// <summary>
        /// Håndterer event når FKM "Ja" radio button er valgt.
        /// Viser detaljesektionen for FKM compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbFKMJa_Checked(object sender, RoutedEventArgs e)
        {
            if (spFKMQuestions != null)
                spFKMQuestions.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Håndterer event når FKM "Nej" radio button er valgt.
        /// Skjuler detaljesektionen for FKM compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbFKMNej_Checked(object sender, RoutedEventArgs e)
        {
            if (spFKMQuestions != null)
                spFKMQuestions.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Elektronik Handlers

        /// <summary>
        /// Håndterer event når Elektronik "Ja" radio button er valgt.
        /// Viser detaljesektionen for Elektronik compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbElektronikJa_Checked(object sender, RoutedEventArgs e)
        {
            if (spElektronikQuestions != null)
                spElektronikQuestions.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Håndterer event når Elektronik "Nej" radio button er valgt.
        /// Skjuler detaljesektionen for Elektronik compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbElektronikNej_Checked(object sender, RoutedEventArgs e)
        {
            if (spElektronikQuestions != null)
                spElektronikQuestions.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Træ Handlers

        /// <summary>
        /// Håndterer event når Træ "Ja" radio button er valgt.
        /// Viser detaljesektionen for Træ compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbTraeJa_Checked(object sender, RoutedEventArgs e)
        {
            if (spTraeQuestions != null)
                spTraeQuestions.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Håndterer event når Træ "Nej" radio button er valgt.
        /// Skjuler detaljesektionen for Træ compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbTraeNej_Checked(object sender, RoutedEventArgs e)
        {
            if (spTraeQuestions != null)
                spTraeQuestions.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Kosmetik Handlers

        /// <summary>
        /// Håndterer event når Kosmetik "Ja" radio button er valgt.
        /// Viser detaljesektionen for Kosmetik compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbKosmetikJa_Checked(object sender, RoutedEventArgs e)
        {
            if (spKosmetikQuestions != null)
                spKosmetikQuestions.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Håndterer event når Kosmetik "Nej" radio button er valgt.
        /// Skjuler detaljesektionen for Kosmetik compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbKosmetikNej_Checked(object sender, RoutedEventArgs e)
        {
            if (spKosmetikQuestions != null)
                spKosmetikQuestions.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region REACH Handlers

        /// <summary>
        /// Håndterer event når REACH "Ja" radio button er valgt.
        /// Viser detaljesektionen for REACH compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbREACHJa_Checked(object sender, RoutedEventArgs e)
        {
            if (spREACHQuestions != null)
                spREACHQuestions.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Håndterer event når REACH "Nej" radio button er valgt.
        /// Skjuler detaljesektionen for REACH compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbREACHNej_Checked(object sender, RoutedEventArgs e)
        {
            if (spREACHQuestions != null)
                spREACHQuestions.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Børn Handlers

        /// <summary>
        /// Håndterer event når Børn "Ja" radio button er valgt.
        /// Viser detaljesektionen for Børn compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbBoernJa_Checked(object sender, RoutedEventArgs e)
        {
            if (spBoernQuestions != null)
                spBoernQuestions.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Håndterer event når Børn "Nej" radio button er valgt.
        /// Skjuler detaljesektionen for Børn compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbBoernNej_Checked(object sender, RoutedEventArgs e)
        {
            if (spBoernQuestions != null)
                spBoernQuestions.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Tekstiler Handlers

        /// <summary>
        /// Håndterer event når Tekstiler "Ja" radio button er valgt.
        /// Viser detaljesektionen for Tekstiler compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbTekstilerJa_Checked(object sender, RoutedEventArgs e)
        {
            if (spTekstilerQuestions != null)
                spTekstilerQuestions.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Håndterer event når Tekstiler "Nej" radio button er valgt.
        /// Skjuler detaljesektionen for Tekstiler compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbTekstilerNej_Checked(object sender, RoutedEventArgs e)
        {
            if (spTekstilerQuestions != null)
                spTekstilerQuestions.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Generelle Forordninger Handlers

        /// <summary>
        /// Håndterer event når Generelle "Ja" radio button er valgt.
        /// Viser detaljesektionen for Generelle compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbGenerelleJa_Checked(object sender, RoutedEventArgs e)
        {
            if (spGenerelleQuestions != null)
                spGenerelleQuestions.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Håndterer event når Generelle "Nej" radio button er valgt.
        /// Skjuler detaljesektionen for Generelle compliance.
        /// </summary>
        /// <param name="sender">Objektet der udløste eventet.</param>
        /// <param name="e">Event argumenter.</param>
        private void rbGenerelleNej_Checked(object sender, RoutedEventArgs e)
        {
            if (spGenerelleQuestions != null)
                spGenerelleQuestions.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Får svaret på om produktet er beregnet til kontakt med fødevarer.
        /// </summary>
        public bool? IsFKM
        {
            get
            {
                if (rbFKMJa.IsChecked == true) return true;
                if (rbFKMNej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        /// <summary>
        /// Får svaret på om produktet indeholder elektroniske komponenter.
        /// </summary>
        public bool? IsElektronik
        {
            get
            {
                if (rbElektronikJa.IsChecked == true) return true;
                if (rbElektronikNej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        /// <summary>
        /// Får svaret på om produktet indeholder trækomponenter.
        /// </summary>
        public bool? IsTrae
        {
            get
            {
                if (rbTraeJa.IsChecked == true) return true;
                if (rbTraeNej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        /// <summary>
        /// Får svaret på om produktet er et kosmetisk produkt.
        /// </summary>
        public bool? IsKosmetik
        {
            get
            {
                if (rbKosmetikJa.IsChecked == true) return true;
                if (rbKosmetikNej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        /// <summary>
        /// Får svaret på om produktet er omfattet af REACH-forordningen.
        /// </summary>
        public bool? IsREACH
        {
            get
            {
                if (rbREACHJa.IsChecked == true) return true;
                if (rbREACHNej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        /// <summary>
        /// Får svaret på om produktet er beregnet til børn.
        /// </summary>
        public bool? IsBoern
        {
            get
            {
                if (rbBoernJa.IsChecked == true) return true;
                if (rbBoernNej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        /// <summary>
        /// Får svaret på om produktet indeholder tekstiler.
        /// </summary>
        public bool? IsTekstiler
        {
            get
            {
                if (rbTekstilerJa.IsChecked == true) return true;
                if (rbTekstilerNej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        /// <summary>
        /// Får svaret på om produktet er omfattet af generelle produktforordninger.
        /// </summary>
        public bool? IsGenerelle
        {
            get
            {
                if (rbGenerelleJa.IsChecked == true) return true;
                if (rbGenerelleNej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        /// <summary>
        /// Får svaret på FKM spørgsmål 1.
        /// </summary>
        public bool? FKMSpoergsmaal1Svar
        {
            get
            {
                if (rbFKM_Q1Ja.IsChecked == true) return true;
                if (rbFKM_Q1Nej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        /// <summary>
        /// Får svaret på FKM spørgsmål 2.
        /// </summary>
        public bool? FKMSpoergsmaal2Svar
        {
            get
            {
                if (rbFKM_Q2Ja.IsChecked == true) return true;
                if (rbFKM_Q2Nej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        /// <summary>
        /// Får svaret på FKM spørgsmål 3.
        /// </summary>
        public bool? FKMSpoergsmaal3Svar
        {
            get
            {
                if (rbFKM_Q3Ja.IsChecked == true) return true;
                if (rbFKM_Q3Nej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        /// <summary>
        /// Får svaret på elektronik spørgsmål 1.
        /// </summary>
        public bool? ElektronikSpoergsmaal1Svar
        {
            get
            {
                if (rbElektronik_Q1Ja.IsChecked == true) return true;
                if (rbElektronik_Q1Nej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        /// <summary>
        /// Får svaret på træ spørgsmål 1.
        /// </summary>
        public bool? TraeSpoergsmaal1Svar
        {
            get
            {
                if (rbTrae_Q1Ja.IsChecked == true) return true;
                if (rbTrae_Q1Nej.IsChecked == true) return false;
                return null; // Ikke valgt
            }
        }

        #endregion

        #region Validering

        /// <summary>
        /// Validerer om alle påkrævede compliance felter er besvaret.
        /// </summary>
        /// <returns>Sand hvis alle obligatoriske spørgsmål er besvaret, falsk ellers.</returns>
        public bool ValiderFelter()
        {
            // Først kontroller om der er svaret på alle hovedkategorier
            if (!IsFKM.HasValue || !IsElektronik.HasValue || !IsTrae.HasValue ||
                !IsKosmetik.HasValue || !IsREACH.HasValue || !IsBoern.HasValue ||
                !IsTekstiler.HasValue || !IsGenerelle.HasValue)
                return false;

            // Derefter kontroller underspørgsmål for kategorier markeret som "Ja"

            // FKM spørgsmål
            if (IsFKM == true)
            {
                // Valider at alle FKM spørgsmål er besvaret
                for (int i = 1; i <= 11; i++)
                {
                    var rbJa = FindName($"rbFKM_Q{i}Ja") as RadioButton;
                    var rbNej = FindName($"rbFKM_Q{i}Nej") as RadioButton;

                    if (rbJa == null || rbNej == null)
                        continue; // Spring over hvis controls ikke eksisterer

                    if (!(rbJa.IsChecked == true || rbNej.IsChecked == true))
                        return false; // Hvis hverken Ja eller Nej er valgt
                }
            }

            // Elektronik spørgsmål
            if (IsElektronik == true)
            {
                // Valider at alle Elektronik spørgsmål er besvaret
                for (int i = 1; i <= 11; i++)
                {
                    var rbJa = FindName($"rbElektronik_Q{i}Ja") as RadioButton;
                    var rbNej = FindName($"rbElektronik_Q{i}Nej") as RadioButton;

                    if (rbJa == null || rbNej == null)
                        continue; // Spring over hvis controls ikke eksisterer

                    if (!(rbJa.IsChecked == true || rbNej.IsChecked == true))
                        return false; // Hvis hverken Ja eller Nej er valgt
                }
            }

            // Træ spørgsmål
            if (IsTrae == true)
            {
                // Valider at alle Træ spørgsmål er besvaret
                for (int i = 1; i <= 3; i++)
                {
                    var rbJa = FindName($"rbTrae_Q{i}Ja") as RadioButton;
                    var rbNej = FindName($"rbTrae_Q{i}Nej") as RadioButton;

                    if (rbJa == null || rbNej == null)
                        continue; // Spring over hvis controls ikke eksisterer

                    if (!(rbJa.IsChecked == true || rbNej.IsChecked == true))
                        return false; // Hvis hverken Ja eller Nej er valgt
                }
            }

            // Kosmetik spørgsmål
            if (IsKosmetik == true)
            {
                // Valider at alle Kosmetik spørgsmål er besvaret
                for (int i = 1; i <= 1; i++) // Kun ét spørgsmål i eksemplet
                {
                    var rbJa = FindName($"rbKosmetik_Q{i}Ja") as RadioButton;
                    var rbNej = FindName($"rbKosmetik_Q{i}Nej") as RadioButton;

                    if (rbJa == null || rbNej == null)
                        continue; // Spring over hvis controls ikke eksisterer

                    if (!(rbJa.IsChecked == true || rbNej.IsChecked == true))
                        return false; // Hvis hverken Ja eller Nej er valgt
                }
            }

            // REACH spørgsmål
            if (IsREACH == true)
            {
                // Valider at alle REACH spørgsmål er besvaret
                for (int i = 1; i <= 1; i++) // Kun ét spørgsmål i eksemplet
                {
                    var rbJa = FindName($"rbREACH_Q{i}Ja") as RadioButton;
                    var rbNej = FindName($"rbREACH_Q{i}Nej") as RadioButton;

                    if (rbJa == null || rbNej == null)
                        continue; // Spring over hvis controls ikke eksisterer

                    if (!(rbJa.IsChecked == true || rbNej.IsChecked == true))
                        return false; // Hvis hverken Ja eller Nej er valgt
                }
            }

            // Børn spørgsmål
            if (IsBoern == true)
            {
                // Valider at alle Børn spørgsmål er besvaret
                for (int i = 1; i <= 2; i++)
                {
                    var rbJa = FindName($"rbBoern_Q{i}Ja") as RadioButton;
                    var rbNej = FindName($"rbBoern_Q{i}Nej") as RadioButton;

                    if (rbJa == null || rbNej == null)
                        continue; // Spring over hvis controls ikke eksisterer

                    if (!(rbJa.IsChecked == true || rbNej.IsChecked == true))
                        return false; // Hvis hverken Ja eller Nej er valgt
                }
            }

            // Tekstiler spørgsmål
            if (IsTekstiler == true)
            {
                // Valider at alle Tekstiler spørgsmål er besvaret
                for (int i = 1; i <= 1; i++) // Kun ét spørgsmål i eksemplet
                {
                    var rbJa = FindName($"rbTekstiler_Q{i}Ja") as RadioButton;
                    var rbNej = FindName($"rbTekstiler_Q{i}Nej") as RadioButton;

                    if (rbJa == null || rbNej == null)
                        continue; // Spring over hvis controls ikke eksisterer

                    if (!(rbJa.IsChecked == true || rbNej.IsChecked == true))
                        return false; // Hvis hverken Ja eller Nej er valgt
                }
            }

            // Generelle spørgsmål
            if (IsGenerelle == true)
            {
                // Valider at alle Generelle spørgsmål er besvaret
                for (int i = 1; i <= 4; i++)
                {
                    var rbJa = FindName($"rbGenerelle_Q{i}Ja") as RadioButton;
                    var rbNej = FindName($"rbGenerelle_Q{i}Nej") as RadioButton;

                    if (rbJa == null || rbNej == null)
                        continue; // Spring over hvis controls ikke eksisterer

                    if (!(rbJa.IsChecked == true || rbNej.IsChecked == true))
                        return false; // Hvis hverken Ja eller Nej er valgt
                }
            }

            // Alle kontroller gennemført uden fejl
            return true;
        }

        #endregion
    }
}