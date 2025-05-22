using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Step5Dto.cs i Models
namespace ProduktFlow2.Core.Models
{
    public class Step5Dto
    {
        public int ProductId { get; set; }
        public bool IsApproved { get; set; } // eller hvad der svarer til 'godkendelse'
        public string ApprovedBy { get; set; } // evt. brugernavn eller initialer
        public DateTime ApprovedAt { get; set; } = DateTime.Now;

        public bool IsFinalApproved { get; set; }          // NYT
        public DateTime? ApprovedDate { get; set; }        // NYT – nullable hvis dato ikke altid er sat

    }
}
