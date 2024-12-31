using Microsoft.AspNetCore.Http;

namespace ProductManagementSystem.Application.Products.Dtos
{
    public class EditProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
    }
}
