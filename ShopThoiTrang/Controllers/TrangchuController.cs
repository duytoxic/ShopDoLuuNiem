using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Controllers
{
    public class TrangchuController : Controller
    {
        private ShopThoiTrangDbContext db = new ShopThoiTrangDbContext();
        // GET: Trangchu
        public ActionResult Index()
        {
            var listcat = db.Categorys.Where(m => m.Status == 1).ToList();
            return View(listcat);
        }

        public ActionResult ProductHome(int catid, string catname)
        {
            var listproduct = db.Products.Where(m => m.Status == 1 && m.CatId == catid).ToList();
            return View("ProductHome", listproduct);
        }

        public ActionResult CartAdd(int id)
        {
            return View("Index");
        }
        public ActionResult AddToCart(int id)
        {
            Product product = db.Products.Find(id);
            if (Session["cart"].Equals(""))
            {
                List<CartItem> listcart = new List<CartItem>();
                CartItem cartitem = new CartItem(product.Id, product.Name, product.Img, product.Price, 1);
                listcart.Add(cartitem);
                Session["cart"] = listcart;
            }
            else
            {
                List<CartItem> listcart = (List<CartItem>)Session["cart"];
                CartItem cartitem = new CartItem(product.Id, product.Name, product.Img, product.Price, 1);
                listcart.Add(cartitem);
                Session["cart"] = listcart;
            }

            return View("Index");
        }

    }
}