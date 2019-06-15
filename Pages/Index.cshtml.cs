using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PaperBoyMain0.Data;

namespace PaperBoyMain0.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> SignedManager;
        private SqlConnection con;
        private SqlCommand com;

        public IndexModel(UserManager<ApplicationUser> UM)
        {
            UserManager = UM;
        }

        public IList<ApplicationUser> ratingList { get; set; }

        public async Task OnGetAsync()
        {
            Random rand = new Random();
            var ratings =  UserManager.Users.OrderBy(c => rand.Next()).Where(n => n.Comment != null).Select(n=>n);
            ratingList = await ratings.ToListAsync();
        }
    }
}
