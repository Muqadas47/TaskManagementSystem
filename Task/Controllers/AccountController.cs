using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

using Task.Models; // Adjust namespace as needed

namespace Task.Controllers
{
    public class AccountController : Controller
    {
        private TaskManagementDBContext db = new TaskManagementDBContext();

        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existingUser == null)
                {
                    var user = new User
                    {
                        
                        Username = model.Username,
                        PasswordHash = HashPassword(model.Password),
                        Role = "User", // Automatically assigning "User" role
                        SignupDate = DateTime.Now
                    };

                    db.Users.Add(user);
                    db.SaveChanges();

                    // Redirect to login page after successful registration
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Username already exists.");
                }
            }

            return View(model);
        }

        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == HashPassword(model.Password));
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe);

                    // Redirect based on role
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (user.Role == "Manager")
                    {
                        return RedirectToAction("Index", "Manager");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(model);
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            // Implement your hashing logic here
            return password; // Placeholder
        }
    }
}
