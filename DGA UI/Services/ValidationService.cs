using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProduktOprettelse.Services
{
    /// <summary>
    /// Service til at håndtere validering af felter på tværs af applikationen
    /// </summary>
    public class ValidationService
    {
        /// <summary>
        /// Valideringsregel der indeholder betingelse og fejlbesked
        /// </summary>
        public class ValidationRule
        {
            public string FieldName { get; set; }
            public Func<bool> Condition { get; set; }
            public string ErrorMessage { get; set; }
            public Control Control { get; set; }  // Reference til UI-kontrollen
        }

        /// <summary>
        /// Validerer en liste af regler og viser fejlbesked for den første fejl
        /// </summary>
        /// <param name="rules">Listen af valideringsregler</param>
        /// <param name="showErrorMessage">Om fejlbesked skal vises (default: true)</param>
        /// <returns>Sand hvis alle regler er opfyldt, ellers falsk</returns>
        public static bool ValidateRules(IEnumerable<ValidationRule> rules, bool showErrorMessage = true)
        {
            // Nulstil alle kontroller først (fjern eventuelle fejlmarkeringer)
            foreach (var rule in rules.Where(r => r.Control != null))
            {
                rule.Control.BorderBrush = Brushes.Gray; // Brug en passende standardfarve
            }

            foreach (var rule in rules)
            {
                if (!rule.Condition())
                {
                    if (showErrorMessage)
                    {
                        MessageBox.Show(rule.ErrorMessage, "Validering",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    // Fremhæv fejlfeltet
                    if (rule.Control != null)
                    {
                        rule.Control.BorderBrush = Brushes.Red;
                        rule.Control.Focus();
                    }

                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Validerer en liste af regler og returnerer alle fejl
        /// </summary>
        /// <param name="rules">Listen af valideringsregler</param>
        /// <returns>Liste af fejlbeskeder, tom hvis ingen fejl</returns>
        public static List<string> GetValidationErrors(IEnumerable<ValidationRule> rules)
        {
            return rules.Where(r => !r.Condition())
                        .Select(r => r.ErrorMessage)
                        .ToList();
        }
    }
}