using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce511.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Scale { get; set; }

        [ValidateNever]
        public List<Product> Products { get; set; }
    }
}
