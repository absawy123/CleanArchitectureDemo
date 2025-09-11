using System.ComponentModel.DataAnnotations;

namespace WebApp.Application.dtos.productDtos
{
    public class ProductDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cant be more than 50 characters.")]
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
    }
}
