using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PaperBoyMain0.Data;

namespace PaperBoyMain0.Pages.PaperProducts
{
    public class IndexModel : PageModel
    {
        private readonly PaperBoyMain0.Data.ApplicationDbContext _context;

        public IndexModel(PaperBoyMain0.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PaperProduct> PaperProduct { get;set; }
        public string html { get; set; }

        public async Task OnGetAsync(string searchString)
        {

            var paperProduct = from m in _context.paperProduct
                               select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                paperProduct = paperProduct.Where(s => s.title.Contains(searchString));
            }

            PaperProduct = await paperProduct.ToListAsync();
        }
    }
}
