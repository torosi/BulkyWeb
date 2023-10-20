using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyWeb.Domain.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace BulkyWeb.Domain.ViewModels
{
	public class CompanyVM
	{
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostCode { get; set; }
        public string PhoneNumber { get; set; }

    }
}

