using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        private ShopThoiTrangDbContext db = new ShopThoiTrangDbContext();
        // GET: Admin/Auth
        public ActionResult Login()
        {
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        public ActionResult DoLogin(FormCollection field)
        {
            string username = field["username"];
            string password = field["pw"];

            User user = db.Users
                .Where(m => m.Roles == "ad" && m.Status == 1 && (m.Username == username))
                .FirstOrDefault();

            if(user != null)
            {
                if (user.Password.Equals(password))
                {
                    Session["UserAdmin"] = username;
                    Session["UserId"] = user.Id.ToString();
                    Session["FullName"] = user.FullName;
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ViewBag.Error = " <div class='text-danger'> Mật khẩu không chính xác</div>";
                }
            }
            else
            {
                ViewBag.Error = " <div class='text-danger'> Tài khoản " +username+ " không tồn tại</div> ";
            }
            return View("Login");
        }

        public ActionResult Logout()
        {
            Session["UserAdmin"] = "";
            Session["UserId"] = "";
            Session["FullName"] = "";
            return Redirect("~/Admin/login");
        }
    }
}