using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaperBoyMain0.Data;

namespace PaperBoyMain0.Pages.PaperProducts
{
    public class CreateModel : PageModel
    {
        private readonly PaperBoyMain0.Data.ApplicationDbContext _context;

        public CreateModel(PaperBoyMain0.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PaperProduct PaperProduct { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.paperProduct.Add(PaperProduct);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}