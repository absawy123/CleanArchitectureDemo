namespace WebApp.Application.dtos.productDtos
{
    public class AddProductDto
    {
        public string Name { get; set; } = default!;
        public string Price { get; set; } = default!;
        public int CategoryId { get; set; }
    }
}
