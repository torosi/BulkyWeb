using BulkWebRazor_Temp.Data;
using BulkWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Category Category { get; set; }

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _context.Categories.Add(Category);
            _context.SaveChanges();
            TempData["success"] = "Category created successfully";
            return RedirectToPage("Index");

        }
    }
}
