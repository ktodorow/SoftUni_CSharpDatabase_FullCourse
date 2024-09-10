namespace Invoices.Data.Models
{
    using Invoices.Data.Models.Enums;

    using System.ComponentModel.DataAnnotations;

    using static DataConstraints;

    public class Product
    {
        //READABILITY TIP: MAKE DATA CONSTRAINT CLASS. 
        public Product()
        {
            ProductsClients = new HashSet<ProductClient>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        //[MaxLength(30)] <-- NOT A GOOD CODE.
        [MaxLength(ProductNameMaxLength)] //<-- CODE READABILIY
        public string Name { get; set; }

        //[Required] <-- DECIMAL IS REQUIRED BY DEFAULT.
        //[MinLength(5)] <-- THIS DOES NOT WORK WITH EF! ONLY DTO.
        //[MaxLength(1000)] <-- THIS WORKS WITH EF BUT WE WILL DEFINE THE RANGE 5-1000 IN THE DTO.
        public decimal Price { get; set; }

        //[Required] <-- ENUMS REQUIRED BY DEFAULT
        public CategoryType CategoryType { get; set; }
    
        public ICollection<ProductClient> ProductsClients { get; set; }
    }
}
