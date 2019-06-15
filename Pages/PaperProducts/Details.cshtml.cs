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
    public class DetailsModel : PageModel
    {
        private readonly PaperBoyMain0.Data.ApplicationDbContext _context;

        public DetailsModel(PaperBoyMain0.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
