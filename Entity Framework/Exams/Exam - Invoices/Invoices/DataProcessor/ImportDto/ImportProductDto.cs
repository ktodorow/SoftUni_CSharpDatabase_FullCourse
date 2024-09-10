namespace Invoices.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstraints;
    
    public class ImportProductDto
    {
        [Required]
        [MinLength(ProductNameMinLength)]
        [MaxLength(ProductNameMaxLength)]
        public string Name { get; set; }

        [Range(typeof(decimal), ProductPriceMinLength, ProductPriceMaxLength) ]
        public decimal Price { get; set; }

        [Range(ProductCategoryTypeMinLength, ProductCategoryTypeMaxLength)]
        public int CategoryType { get; set; }
        
        [Required]
        public int[] Clients { get; set; }
    }
}
