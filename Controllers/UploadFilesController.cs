using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PaperBoyMain0.Data;

namespace PaperBoyMain0.Controllers
{
    public class UploadFilesController : Controller
    {
        private readonly IHostingEnvironment fe;
        private readonly ApplicationDbContext dbContext;
     
        public UploadFilesController(IHostingEnvironment e , ApplicationDbContext ee)
        {
            fe = e;
            dbContext = ee;
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                return Content("File not selected");
            }

            if (file.Length > 0 && file.ContentType.Contains("image"))
            {
                string root = fe.WebRootPath;
                string path = Path.Combine(root + "\\User_Files\\Images\\" + Path.GetFileName(file.FileName));

                // full path to file in temp location

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return RedirectToPage("/Account/Image");
            }
            return RedirectToPage("/Account/Image");

        }
    }
}