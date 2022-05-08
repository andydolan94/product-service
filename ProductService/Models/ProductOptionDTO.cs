using System;

namespace ProductService.Models
{
    public class ProductOptionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}