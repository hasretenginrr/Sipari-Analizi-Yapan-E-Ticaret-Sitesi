namespace BitirmeProjesi.DTO
{
    public class UserHomeViewModel
    {
        public List<Products> Products { get; set; }
        public Users Users { get; set; }
        public List<ProductDto> Recommendations { get; set; }
    }
}
