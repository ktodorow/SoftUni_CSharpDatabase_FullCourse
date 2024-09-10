namespace Invoices.Data.Models
{
    using Invoices.Data.Models.Enums;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstraints;
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        public int Number { get; set; }

        //DATETIME2 REQUIRED BY DEFAULT
        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public decimal Amount { get; set; }

        public CurrencyType CurrencyType { get; set; }

        [Required]
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        [Required]
        public Client Client { get; set; } = null!;
    }
}