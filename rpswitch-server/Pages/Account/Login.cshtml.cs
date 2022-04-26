using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Microsoft.AspNetCore.Authorization;

namespace rpswitch.Pages.Account
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
   // [AllowAnonymous]
    public class LoginModel : PageModel
    {
        public string RequestId { get; set; }

        public LoginModel(){
            ;
        }
        public void OnGet()
        {
            Console.Write("Hello");
        }
        public IActionResult OnPost(string user,string pass)
        {
            Console.Write("Post");
            return  Page();
        }
    }
}
