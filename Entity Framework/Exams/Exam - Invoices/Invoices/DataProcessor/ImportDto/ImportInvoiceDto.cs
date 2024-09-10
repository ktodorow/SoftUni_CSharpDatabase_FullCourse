namespace Invoices.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstraints;

    public class ImportInvoiceDto
    {
        [Range(InvoiceNumberMinLength, InvoiceNumberMaxLength)]
        public int Number { get; set; }

        [Required]
        public string IssueDate { get; set; }

        [Required]
        public string DueDate { get; set; }

        public decimal Amount { get; set; }

        [Range(InvoiceCurrencyTypeMinLength, InvoiceCurrencyTypeMaxLength)]
        public int CurrencyType { get; set; } 

        public int ClientId { get; set; }
    }
}
