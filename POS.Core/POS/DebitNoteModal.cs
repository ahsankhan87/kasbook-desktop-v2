using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core.POS
{
    public class DebitNoteModal
    {
        public int DebitNoteId { get; set; }
        public string OriginalInvoiceId { get; set; }
        public string DebitNoteNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public string CustomerName { get; set; }
        public string ZatcaUuid { get; set; }

        public decimal VATAmount { get; set; }  
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; } // Optional, if you want to link to a customer ID

        public string InvoiceSubTypeCode { get; set; } = "02"; // Default value 02=simplified for ZATCA compliance

        public string InvoiceSubtype { get; set; }

        // Add other ZATCA-required fields as needed
    }
}
