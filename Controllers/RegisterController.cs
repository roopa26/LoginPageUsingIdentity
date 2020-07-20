using LoginPage.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoginPage.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Helpers;

namespace LoginPage.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel rvm)
        {
            if(ModelState.IsValid)
            {
                var context = new ApplicationDBContext();
                var store = new ApplicationUserStore(context);
                var userManager = new ApplicationUserManager(store);
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                if (!roleManager.RoleExists("Customer"))
                {
                    var role = new IdentityRole();
                    role.Name = "Customer";
                    roleManager.Create(role);
                }
                if (userManager.FindByName(rvm.UserName) == null)
                {
                    var user = new ApplicationUser();
                    user.UserName = rvm.UserName;
                    user.Email = rvm.Email;
                    string password = Crypto.HashPassword(rvm.Password);
                    user.PasswordHash = password;
                    var chkUser = userManager.Create(user, user.PasswordHash);
                    if (chkUser.Succeeded)
                    {
                        userManager.AddToRole(user.Id, "Customer");
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}