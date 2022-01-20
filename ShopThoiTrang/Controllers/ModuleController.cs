using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Controllers
{
    public class ModuleController : Controller
    {
        private ShopThoiTrangDbContext db = new ShopThoiTrangDbContext();
        // GET: Module
        public ActionResult ListCategoryMenu()
        {
            var list = db.Categorys.Where(m => m.Status == 1).ToList();
            return View("ListCategoryMenu", list);
        }

        public ActionResult ProductDetail(String slug)
        {
            return View("ProductDetail");
        }
    }
}