using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce511.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Range(0, 100000)]
        public decimal Price { get; set; }
        [RegularExpression("^.*\\.(png|jpg)$")]
        public string? Img { get; set; }
        [Range(0, 5)]
        public double Rate { get; set; }
        [Range(0, 50)]
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public int? CompanyId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public Company Company { get; set; }
    }
}
