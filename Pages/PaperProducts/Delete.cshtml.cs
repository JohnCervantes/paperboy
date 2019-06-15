using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PaperBoyMain0.Data;

namespace PaperBoyMain0.Pages.PaperProducts
{
    public class DeleteModel : PageModel
    {
        private readonly PaperBoyMain0.Data.ApplicationDbContext _context;

        public DeleteModel(PaperBoyMain0.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PaperProduct PaperProduct { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PaperProduct = await _context.paperProduct.SingleOrDefaultAsync(m => m.ID == id);

            if (PaperProduct == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PaperProduct = await _context.paperProduct.FindAsync(id);

            if (PaperProduct != null)
            {
                _context.paperProduct.Remove(PaperProduct);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
