using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BulkyWeb.Domain.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string ISBN { get; set; }
		[Required]
		public string Author { get; set; }
		[Required]
		public string Description { get; set; }

		[Required]
		[Display(Name="List Price")]
		[Range(1, 1000)]
		public double ListPrice { get; set; }

		// these are going to be the prices for when people are buying in bulk
        [Required]
        [Display(Name = "List Price 1-50")]
        [Range(1, 1000)]
        public double Price { get; set; }

        [Required]
        [Display(Name = "List Price 50+")]
        [Range(1, 1000)]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "List Price 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

		// adding foreign key to category table
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
        // we do not want to validate this property as it will invalidate the model state and not post correctly on create
        [ValidateNever]
		public Category Category { get; set; } // navigation property to the category table
        [ValidateNever]
        public string? ImageUrl { get; set; }

	}
}

