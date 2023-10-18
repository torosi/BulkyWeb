using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyWeb.Domain.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace BulkyWeb.Domain.ViewModels
{
	public class ProductVM
	{
		[ValidateNever] // we do not want to validate this property as it will invalidate the model state and not post correctly on create
		public IEnumerable<SelectListItem> CategoryList { get; set; }

        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public double ListPrice { get; set; }

        [Display(Name = "List Price 1-50")]
        [Range(1, 1000)]
        public double Price { get; set; }

        [Display(Name = "List Price 50+")]
        [Range(1, 1000)]
        public double Price50 { get; set; }

        [Display(Name = "List Price 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

    }
}

