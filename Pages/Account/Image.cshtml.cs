using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using PaperBoyMain0.Data;

namespace PaperBoyMain0.Pages.Account
{
    [Authorize(Policy = "Authenticated")]
    public class ImageModel : PageModel
    {
        private SqlConnection con;
        private SqlCommand com;
        
        [BindProperty]
        public string id { get; set; }
        
        public string imgPth { get; set; }

        private readonly IHostingEnvironment fe;
        private readonly IConfiguration config;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> SignedManager;


        public ImageModel(IHostingEnvironment e, ApplicationDbContext ee, IConfiguration eee, UserManager<ApplicationUser> eeee)
        {
            UserManager = eeee;
            fe = e;
            config = eee;
        }


        public async Task OnGetAsync()
        {
            imgPth = UserManager.Users.Where(p => p.Id.Equals(UserManager.GetUserId(User))).Select(p => p.ImgPth).FirstOrDefault();
        }
            public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
            {

            if (file != null)
            {
                if (file.Length > 0 && !file.FileName.Contains(".gif") && file.ContentType.Contains("image"))
                {
                    string root = fe.WebRootPath;
                    string path = Path.Combine(root + "\\Temp\\" + Path.GetFileName(file.FileName));
                    string path1 = Path.Combine("\\Temp\\" + Path.GetFileName(file.FileName));
                    // full path to file in temp location

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        con = new SqlConnection(config.GetConnectionString("DefaultConnection"));

                        con.Open();
                        com = new SqlCommand("UPDATE dbo.AspNetUsers set ImgPth=@a1 where Id=@a2", con);
                        com.Parameters.Add("@a1", SqlDbType.VarChar);
                        com.Parameters.Add("@a2", SqlDbType.VarChar);
                        com.Parameters["@a1"].Value = path1;
                        com.Parameters["@a2"].Value = UserManager.GetUserId(User);
                        com.ExecuteNonQuery();
                        con.Close();
                        imgPth = path1;
                        await file.CopyToAsync(stream);
                    }
                    return RedirectToPage();
                }
            }
            return RedirectToPage();
        }

    }

        
    //    public async Task<IActionResult> OnPostSaveAsync(IFormFile file)
    //    {
            

    //        string root = fe.WebRootPath;
    //        string path = Path.Combine(root + "\\User_Files\\Images\\" + Path.GetFileName(file.FileName));
    //        string path1 = Path.Combine("\\User_Files\\Images\\" + Path.GetFileName(file.FileName));


    //        if (file.Length > 0 && !file.FileName.Contains(".gif") && file.ContentType.Contains("image"))
    //        {
    //            con = new SqlConnection(config.GetConnectionString("DefaultConnection"));

    //            con.Open();
    //            com = new SqlCommand("UPDATE dbo.AspNetUsers set ImgPth=@a1 where Id=@a2", con);
    //            com.Parameters.Add("@a1", SqlDbType.VarChar);
    //            com.Parameters.Add("@a2", SqlDbType.VarChar);
    //            com.Parameters["@a1"].Value = imgPth;
    //            com.Parameters["@a2"].Value = UserManager.GetUserId(User);
    //            com.ExecuteNonQuery();
    //            con.Close();
    //        }

    //        return RedirectToPage("/index");
    //    }
    //}

    /*using (var stream = new FileStream(path, FileMode.Create))
               {
               "\\User_Files\\Images\\"
                   imgPth = path1;
                  await file.CopyToAsync(stream);
               }*/


}
