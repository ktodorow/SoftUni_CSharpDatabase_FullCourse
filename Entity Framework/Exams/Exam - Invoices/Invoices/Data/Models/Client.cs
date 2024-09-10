namespace Invoices.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstraints;
    public class Client
    {
        public Client()
        {
            Invoices = new HashSet<Invoice>();
            Addresses = new HashSet<Address>();
            ProductsClients = new HashSet<ProductClient>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ClientNameMaxLength)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(ClientNumberVatMaxLength)]
        public string NumberVat { get; set; } 

        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<ProductClient> ProductsClients { get; set; }
    }
}

