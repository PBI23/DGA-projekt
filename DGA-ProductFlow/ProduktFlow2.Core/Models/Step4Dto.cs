using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProduktFlow2.Core.Models
{
    /// <summary>
    /// Data Transfer Object for optional Step 4 answers.
    /// Contains dropdown and other optional values tied to a product.
    /// </summary>
    public class Step4Dto
    {
        public int ProductId { get; set; }

        public string DgaColorGroupName { get; set; }
        public string DgaSalCatGroup { get; set; }
        public string PantonePantone { get; set; }
        public string DgaVendItemCodeCode { get; set; }

        public bool? Assorted { get; set; }
        public string AdditionalInformation { get; set; }
        public int? GsmWeight { get; set; }
        public int? BurningTimeHours { get; set; }
        public bool? AntidopingRegulation { get; set; }
        public string Subcategory { get; set; }
        public string OtherInformation2 { get; set; }
        public int? GsmWeight2 { get; set; }
    }
}
