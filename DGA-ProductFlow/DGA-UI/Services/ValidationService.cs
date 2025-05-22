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
    /// Updated for .NET 8.0 compatibility
    /// </summary>
    public class ValidationService
    {
        /// <summary>
        /// Valideringsregel der indeholder betingelse og fejlbesked
        /// </summary>
        public class ValidationRule
        {
            public string FieldName { get; set; } = string.Empty;
            public Func<bool> Condition { get; set; } = () => true;
            public string ErrorMessage { get; set; } = string.Empty;
            public Control? Control { get; set; }  // Reference til UI-kontrollen
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
                if (rule.Control != null)
                {
                    rule.Control.BorderBrush = SystemColors.ControlDarkBrush; // Use system brush for consistency
                }
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

        /// <summary>
        /// Validates that a text field contains only numeric values
        /// </summary>
        /// <param name="value">The text value to validate</param>
        /// <returns>True if numeric, false otherwise</returns>
        public static bool IsNumeric(string value)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                   value.All(char.IsDigit);
        }

        /// <summary>
        /// Validates that a text field contains a valid decimal value
        /// </summary>
        /// <param name="value">The text value to validate</param>
        /// <returns>True if valid decimal, false otherwise</returns>
        public static bool IsValidDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // Handle both comma and period as decimal separator
            var normalizedValue = value.Replace(',', '.');
            return decimal.TryParse(normalizedValue,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out _);
        }
    }
}