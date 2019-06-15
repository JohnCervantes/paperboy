using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using PaperBoyMain0.Data;

namespace PaperBoyMain0.Pages
{
    [Authorize(Policy = "Authenticated")]
    public class RatingModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly UserManager<ApplicationUser> UserManager;
        private SqlConnection con;
        private SqlCommand com;


        public RatingModel(IConfiguration eee, UserManager<ApplicationUser> eeee)
        {
            UserManager = eeee;
            config = eee;
        }

        [BindProperty]
        public InputModel input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }


        public class InputModel
        {
            [Required]
            [Display(Name = "Rating")]
            public int Rating { get; set; }

            [Required]
            [Display(Name = "Comment")]
            public string Comment { get; set; }
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = new InputModel {Comment = input.Comment, Rating = input.Rating};

            con = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            try
            {
                con.Open();
                com = new SqlCommand("UPDATE dbo.AspNetUsers set Comment=@a1, Rating=@a3 where Id=@a2", con);
                com.Parameters.Add("@a1", SqlDbType.VarChar);
                com.Parameters.Add("@a2", SqlDbType.VarChar);
                com.Parameters.Add("@a3", SqlDbType.VarChar);
                com.Parameters["@a1"].Value = user.Comment;
                com.Parameters["@a2"].Value = UserManager.GetUserId(User);
                com.Parameters["@a3"].Value = user.Rating;
                com.ExecuteNonQuery();
                con.Close();
                StatusMessage = "Your feedback has been successfully submitted.";
            }
            catch (SqlException e)
            {
                return RedirectToPage();
            }
            return RedirectToPage();
        }
    }
}