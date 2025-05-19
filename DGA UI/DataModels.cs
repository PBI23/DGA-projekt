using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProduktOprettelse
{
    /// <summary>
    /// Model class representing an item in a dropdown list
    /// </summary>
    public class ListeItem
    {
        /// <summary>
        /// Unique identifier for the item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Display name for the item
        /// </summary>
        public string Navn { get; set; }
    }

    /// <summary>
    /// ViewModel for handling data binding between the UI and data sources
    /// </summary>
    public class ProduktViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Collection of product groups
        /// </summary>
        private ObservableCollection<ListeItem> _varegrupper;
        public ObservableCollection<ListeItem> Varegrupper
        {
            get { return _varegrupper; }
            set
            {
                _varegrupper = value;
                OnPropertyChanged(nameof(Varegrupper));
            }
        }

        /// <summary>
        /// Collection of seasons
        /// </summary>
        private ObservableCollection<ListeItem> _saesoner;
        public ObservableCollection<ListeItem> Saesoner
        {
            get { return _saesoner; }
            set
            {
                _saesoner = value;
                OnPropertyChanged(nameof(Saesoner));
            }
        }

        /// <summary>
        /// Collection of countries of origin
        /// </summary>
        private ObservableCollection<ListeItem> _oprindelseslande;
        public ObservableCollection<ListeItem> Oprindelseslande
        {
            get { return _oprindelseslande; }
            set
            {
                _oprindelseslande = value;
                OnPropertyChanged(nameof(Oprindelseslande));
            }
        }

        /// <summary>
        /// Collection of suppliers
        /// </summary>
        private ObservableCollection<ListeItem> _leverandoerer;
        public ObservableCollection<ListeItem> Leverandoerer
        {
            get { return _leverandoerer; }
            set
            {
                _leverandoerer = value;
                OnPropertyChanged(nameof(Leverandoerer));
            }
        }
        /// <summary>
        /// Collection of designers
        /// </summary>
        private ObservableCollection<ListeItem> _designere;
        public ObservableCollection<ListeItem> Designere
        {
            get { return _designere; }
            set
            {
                _designere = value;
                OnPropertyChanged(nameof(Designere));
            }
        }
        /// <summary>
        /// Collection of product descriptions
        /// </summary>
        private ObservableCollection<ListeItem> _beskrivelse;
        public ObservableCollection<ListeItem> Beskrivelse
        {
            get { return _beskrivelse; }
            set
            {
                _beskrivelse = value;
                OnPropertyChanged(nameof(Beskrivelse));
            }
        }

        /// <summary>
        /// Collection of product categories
        /// </summary>
        private ObservableCollection<ListeItem> _kategorier;
        public ObservableCollection<ListeItem> Kategorier
        {
            get { return _kategorier; }
            set
            {
                _kategorier = value;
                OnPropertyChanged(nameof(Kategorier));
            }
        }

        /// <summary>
        /// Collection of colors
        /// </summary>
        private ObservableCollection<ListeItem> _farver;
        public ObservableCollection<ListeItem> Farver
        {
            get { return _farver; }
            set
            {
                _farver = value;
                OnPropertyChanged(nameof(Farver));
            }
        }

        /// <summary>
        /// Collection of materials
        /// </summary>
        private ObservableCollection<ListeItem> _materialer;
        public ObservableCollection<ListeItem> Materialer
        {
            get { return _materialer; }
            set
            {
                _materialer = value;
                OnPropertyChanged(nameof(Materialer));
            }
        }

        /// <summary>
        /// Collection of units
        /// </summary>
        private ObservableCollection<ListeItem> _enheder;
        public ObservableCollection<ListeItem> Enheder
        {
            get { return _enheder; }
            set
            {
                _enheder = value;
                OnPropertyChanged(nameof(Enheder));
            }
        }

        /// <summary>
        /// Notifies subscribers about property changes
        /// </summary>
        /// <param name="propertyName">Name of the property that changed</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Loads data from simulated backend services
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        public async Task LoadDataAsync()
        {
            // Simulates an asynchronous database query
            await Task.Delay(100);

            // Product groups
            Varegrupper = new ObservableCollection<ListeItem>
            {
                new ListeItem { Id = 1, Navn = "Julepynt" },
                new ListeItem { Id = 2, Navn = "Påskepynt" },
                new ListeItem { Id = 3, Navn = "Boligindretning" },
                new ListeItem { Id = 4, Navn = "Køkkenudstyr" },
                new ListeItem { Id = 5, Navn = "Dekoration" }
            };

            // Seasons
            Saesoner = new ObservableCollection<ListeItem>
            {
                new ListeItem { Id = 1, Navn = "Forår" },
                new ListeItem { Id = 2, Navn = "Sommer" },
                new ListeItem { Id = 3, Navn = "Efterår" },
                new ListeItem { Id = 4, Navn = "Vinter" },
                new ListeItem { Id = 5, Navn = "Jul" },
                new ListeItem { Id = 6, Navn = "Påske" },
                new ListeItem { Id = 7, Navn = "Hele året" }
            };

            // Countries of origin
            Oprindelseslande = new ObservableCollection<ListeItem>
            {
                new ListeItem { Id = 1, Navn = "Danmark" },
                new ListeItem { Id = 2, Navn = "Sverige" },
                new ListeItem { Id = 3, Navn = "Norge" },
                new ListeItem { Id = 4, Navn = "Tyskland" },
                new ListeItem { Id = 5, Navn = "Kina" },
                new ListeItem { Id = 6, Navn = "Indien" },
                new ListeItem { Id = 7, Navn = "Andet" }
            };

            // Suppliers (would typically come from a database)
            Leverandoerer = new ObservableCollection<ListeItem>
            {
                new ListeItem { Id = 1, Navn = "Jensen Import A/S" },
                new ListeItem { Id = 2, Navn = "Scandinavian Design" },
                new ListeItem { Id = 3, Navn = "Nordic Home" },
                new ListeItem { Id = 4, Navn = "Global Trading Co." },
                new ListeItem { Id = 5, Navn = "Dansk Boliginteriør" }
            };

            // Designers (would typically come from a database)

            Designere = new ObservableCollection<ListeItem>
            {
                new ListeItem { Id = 1, Navn = "Hans Hansen" },
                new ListeItem { Id = 2, Navn = "Marie Madsen" },
                new ListeItem { Id = 3, Navn = "Lars Larsen" },
                new ListeItem { Id = 4, Navn = "Sofie Sørensen" },
                new ListeItem { Id = 5, Navn = "Peter Petersen" }
            };

            // Product descriptions

            Beskrivelse = new ObservableCollection<ListeItem>
            {
                new ListeItem { Id = 1, Navn = "Elegant og stilren" },
                new ListeItem { Id = 2, Navn = "Farverig og sjov" },
                new ListeItem { Id = 3, Navn = "Praktisk og funktionel" },
                new ListeItem { Id = 4, Navn = "Traditionel og klassisk" },
                new ListeItem { Id = 5, Navn = "Moderne og innovativ" }
            };

            // Categories
            Kategorier = new ObservableCollection<ListeItem>
            {
                new ListeItem { Id = 1, Navn = "Dekoration" },
                new ListeItem { Id = 2, Navn = "Boligtilbehør" },
                new ListeItem { Id = 3, Navn = "Køkken" },
                new ListeItem { Id = 4, Navn = "Julepynt" },
                new ListeItem { Id = 5, Navn = "Påskepynt" }
            };

            // Colors
            Farver = new ObservableCollection<ListeItem>
            {
                new ListeItem { Id = 1, Navn = "Rød" },
                new ListeItem { Id = 2, Navn = "Grøn" },
                new ListeItem { Id = 3, Navn = "Blå" },
                new ListeItem { Id = 4, Navn = "Gul" },
                new ListeItem { Id = 5, Navn = "Sort" },
                new ListeItem { Id = 6, Navn = "Hvid" },
                new ListeItem { Id = 7, Navn = "Multifarvet" }
            };

            // Materials
            Materialer = new ObservableCollection<ListeItem>
            {
                new ListeItem { Id = 1, Navn = "Træ" },
                new ListeItem { Id = 2, Navn = "Metal" },
                new ListeItem { Id = 3, Navn = "Plastik" },
                new ListeItem { Id = 4, Navn = "Glas" },
                new ListeItem { Id = 5, Navn = "Stof" },
                new ListeItem { Id = 6, Navn = "Keramik" }
            };

            // Units
            Enheder = new ObservableCollection<ListeItem>
            {
                new ListeItem { Id = 1, Navn = "kg" },
                new ListeItem { Id = 2, Navn = "g" },
                new ListeItem { Id = 3, Navn = "lb" },
                new ListeItem { Id = 4, Navn = "oz" },
                new ListeItem { Id = 5, Navn = "stk" }
            };
        }
    }

    /// <summary>
    /// Service class that simulates backend API calls
    /// This would be replaced with actual API calls in a real implementation
    /// </summary>
    public static class DataService
    {
        /// <summary>
        /// Simulates fetching data from a backend service
        /// </summary>
        /// <param name="dataType">Type of data to retrieve</param>
        /// <returns>A list of items matching the requested type</returns>
        public static async Task<List<ListeItem>> HentDataFraBackendAsync(string dataType)
        {
            // This method would normally call a REST API, GraphQL, or other backend service
            await Task.Delay(100); // Simulates network delay

            // Return mock data based on type
            switch (dataType)
            {
                case "varegrupper":
                    return new List<ListeItem>
                    {
                        new ListeItem { Id = 1, Navn = "Julepynt" },
                        new ListeItem { Id = 2, Navn = "Påskepynt" },
                        // etc.
                    };
                // Other cases for different data types
                default:
                    return new List<ListeItem>();
            }
        }

        /// <summary>
        /// Simulates saving a product to the backend
        /// </summary>
        /// <param name="produktData">The product data to save</param>
        /// <returns>Boolean indicating success</returns>
        public static async Task<bool> GemProduktAsync(object produktData)
        {
            // Simulates an API call to save product data
            await Task.Delay(300);

            // Return true to indicate success (in reality would be based on API response)
            return true;
        }
    }
}
