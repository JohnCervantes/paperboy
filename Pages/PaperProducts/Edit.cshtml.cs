using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaperBoyMain0.Data;

namespace PaperBoyMain0.Pages.PaperProducts
{
    public class EditModel : PageModel
    {
        private readonly PaperBoyMain0.Data.ApplicationDbContext _context;

        public EditModel(PaperBoyMain0.Data.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PaperProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaperProductExists(PaperProduct.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PaperProductExists(int id)
        {
            return _context.paperProduct.Any(e => e.ID == id);
        }
    }
}
