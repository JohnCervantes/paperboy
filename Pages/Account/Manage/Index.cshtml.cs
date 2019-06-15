using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using PaperBoyMain0.Data;
using PaperBoyMain0.Services;

namespace PaperBoyMain0.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment fe;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IConfiguration config,
            IHostingEnvironment e)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _config = config;
            fe = e;
        }

        private SqlConnection con;
        private SqlCommand com;
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string imgPth { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }
        }
       
        
        public async Task<IActionResult> OnGetAsync()
        {
            imgPth = _userManager.Users.Where(p => p.Id.Equals(_userManager.GetUserId(User))).Select(p => p.ImgPth).FirstOrDefault();

            var users = _userManager.Users.Where(p => p.Id.Equals(_userManager.GetUserId(User)));
            if (users == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            foreach (var user in users) {
                Username = user.UserName;
                Input = new InputModel
                {
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName

                };
               
             }

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(await _userManager.GetUserAsync(User));


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

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
                        con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

                        con.Open();
                        com = new SqlCommand("UPDATE dbo.AspNetUsers set ImgPth=@a1 where Id=@a2", con);
                        com.Parameters.Add("@a1", SqlDbType.VarChar);
                        com.Parameters.Add("@a2", SqlDbType.VarChar);
                        com.Parameters["@a1"].Value = path1;
                        com.Parameters["@a2"].Value = _userManager.GetUserId(User);
                        com.ExecuteNonQuery();
                        con.Close();
                        imgPth = path1;
                        await file.CopyToAsync(stream);
                    }
                }
            }
           
                var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Input.FirstName != user.FirstName)
            {
                var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

                try
                {
                  
                    con.Open();
                    com = new SqlCommand("UPDATE dbo.AspNetUsers set FirstName=@a1 where Id=@a2", con);
                    com.Parameters.Add("@a1", SqlDbType.VarChar);
                    com.Parameters.Add("@a2", SqlDbType.VarChar);
                    com.Parameters["@a1"].Value = Input.FirstName;
                    com.Parameters["@a2"].Value = _userManager.GetUserId(User);
                    com.ExecuteNonQuery();
                    con.Close();
                    StatusMessage = "Your profile has been updated";
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }

            if (Input.LastName != user.LastName)
            {
                var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

                try
                {

                    con.Open();
                    com = new SqlCommand("UPDATE dbo.AspNetUsers set LastName=@a1 where Id=@a2", con);
                    com.Parameters.Add("@a1", SqlDbType.VarChar);
                    com.Parameters.Add("@a2", SqlDbType.VarChar);
                    com.Parameters["@a1"].Value = Input.LastName;
                    com.Parameters["@a2"].Value = _userManager.GetUserId(User);
                    com.ExecuteNonQuery();
                    con.Close();
                    StatusMessage = "Your profile has been updated";
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (Input.Email != user.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
                else
                {
                    StatusMessage = "Your profile has been updated";
                }
            }

            if (Input.PhoneNumber != user.PhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
                else
                {
                    StatusMessage = "Your profile has been updated";
                }
            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            await _emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
