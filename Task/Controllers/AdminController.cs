using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task.Models;


namespace Task.Controllers
{
   
    public class AdminController : Controller
    {
        private readonly TaskManagementDBContext db = new TaskManagementDBContext();

        // GET: Admin/UserList
       // [Authorize(Roles = "Admin")]
        public ActionResult UserList()
        {
            var users = db.Users.ToList();
            return View(users);
        }

        public ActionResult Index()
        {
            return View();
        }
        // GET: Admin/CreateUser
        public ActionResult CreateUser()
        {
            return View();
        }

        // POST: Admin/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                user.SignupDate = DateTime.Now;
                user.PasswordHash = HashPassword(user.PasswordHash); // Add password hashing logic
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("UserList");
            }
            return View(user);
        }

        // GET: Admin/EditUser/5
        public ActionResult EditUser(int? id)
        {
            if (id == null)
           {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/EditUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user) // Changed from User to Users
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.Find(user.UserID);
                if (existingUser == null)
                {
                    return HttpNotFound();
                }
                // Update the fields you want to change
                existingUser.Username = user.Username;
                existingUser.Role = user.Role;
                existingUser.IsBlocked = user.IsBlocked;
                existingUser.IsDeleted = user.IsDeleted;

                db.Entry(existingUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserList");
            }
            return View(user);
        }



        // GET: Admin/DeleteUser/5
        public ActionResult DeleteUser(int? id)
        
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/DeleteUser/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            user.IsDeleted = true;
            
            db.SaveChanges();
            return RedirectToAction("UserList");
        }

        private string HashPassword(string password)
        {
            // Implement password hashing logic here
            return password; // Replace with hashed password
        }
    }
}
