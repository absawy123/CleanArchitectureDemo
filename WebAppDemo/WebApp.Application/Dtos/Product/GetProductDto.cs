namespace WebApp.Application.dtos.productDtos
{
    public class GetProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
