using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Controllers
{
    public class KhachhangController : Controller
    {
        private ShopThoiTrangDbContext db = new ShopThoiTrangDbContext();
        // GET: Khachhang
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DangNhap(FormCollection form)
        {
            string username = form["username"];
            string password = form["pw"];
            User user = db.Users
                .Where(m => m.Roles == "user" && m.Status == 1 && (m.Username == username))
                .FirstOrDefault();

            if (user != null)
            {
                if (user.Password.Equals(password))
                {
                    Session["UserCustomer"] = username;
                    Session["CustomerId"] = user.Id.ToString();
                    Session["FullName"] = user.FullName;
                    return RedirectToAction("Thanhtoan", "Giohang");
                }
                else
                {
                    ViewBag.Error = " <div class='text-danger'> Mật khẩu không chính xác</div>";
                }
            }
            else
            {
                ViewBag.Error = " <div class='text-danger'> Tài khoản " + username + " không tồn tại</div> ";
            }
            return View("DangNhap");
        }

        public ActionResult DangKy()
        {
            return View();
        }
    }
}