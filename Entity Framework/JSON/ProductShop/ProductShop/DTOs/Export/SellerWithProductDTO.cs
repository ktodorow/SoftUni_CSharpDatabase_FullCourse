namespace ProductShop.DTOs.Export
{
    public class SellerWithProductDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<SoldProductsDTO> SoldProducts { get; set; }
    }
}
