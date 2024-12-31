namespace ProductManagementSystem.Application.Products.Dtos
{
    internal class ProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public string CreationDate { get; set; }
    }
}
