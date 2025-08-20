namespace WebApp.Application.dtos.productDtos
{
    public class ProductDto
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
