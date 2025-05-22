using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktFlow2.Core.Models
{
    public class Step3Dto
    {
        public int ProductId { get; set; }

        // General data
        public int SupplierProductNo { get; set; }
        public int CustomerClearanceNo { get; set; }
        public decimal CustomerClearancePercent { get; set; }
        public decimal CostPrice { get; set; }

        // Packaging
        public int InnerCarton { get; set; }
        public string OuterCarton { get; set; }

        // Dimensions
        public decimal GrossWeight { get; set; }
        public decimal PackingHeight { get; set; }
        public decimal PackingWidthLength { get; set; }
        public decimal PackingDepth { get; set; }

        // Usage properties
        public bool DishwasherSafe { get; set; }
        public bool MicrowaveSafe { get; set; }

        // Sustainability
        public bool Svanemaerket { get; set; }
        public bool GrunerPunkt { get; set; }
        public bool FSC100 { get; set; }
        public bool FSCMix70 { get; set; }

        // Classification / Branding
        public string ABC { get; set; }
        public string ProductLogo { get; set; }
        public string HangtagsAndStickers { get; set; }
        public string Series { get; set; }
    }

}
