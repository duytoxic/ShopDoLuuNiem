using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Controllers
{
    public class GiohangController : Controller
    {
        ShopThoiTrangDbContext db = new ShopThoiTrangDbContext();
        // GET: Giohang
        public ActionResult Index()
        {
            List<CartItem> listcart = (List<CartItem>)Session["cart"];
            return View("Index", listcart);
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
                List<CartItem> listcart = (List<CartItem>) Session["cart"];
                CartItem cartitem = new CartItem(product.Id, product.Name, product.Img, product.Price, 1);
                if (listcart.Where(m=>m.ProductId == id).Count() != 0)
                {
                    // đã có sp trong giỏ hàng
                    cartitem.Quantity += 1;
                    foreach(var item in listcart)
                    {
                        if(item.ProductId == id)
                        {
                            item.Quantity += 1;
                        }
                    }
                    Session["cart"] = listcart;
                }
                else
                {
                    listcart.Add(cartitem);
                    Session["cart"] = listcart;
                }     
            }

            return RedirectToAction("Index", "Giohang");
        }

        public ActionResult RemoveCartItem(int id)
        {
            if (!Session["cart"].Equals(""))
            {
                List<CartItem> listcart = (List<CartItem>)Session["cart"];
                int vt = 0;
                foreach (var item in listcart)
                {
                    if (item.ProductId == id)
                    {
                        listcart.RemoveAt(vt);
                        break;
                    }
                    vt++;
                }
                Session["cart"] = listcart;
            }
            return RedirectToAction("Index", "Giohang");
        }

        public ActionResult UpdateCart(FormCollection form)
        {
            if (!string.IsNullOrEmpty(form["update"]))
            {
                var listquantity = form["quantity"];
                var listarr = listquantity.Split(',');

                List<CartItem> listcart = (List<CartItem>)Session["cart"];
                int vt = 0;
                foreach (var item in listcart)
                {
                    item.Quantity = int.Parse(listarr[vt]);
                    vt++;
                }
                Session["cart"] = listcart;
            }
            return RedirectToAction("Index", "Giohang");
        }

        public ActionResult RemoveAllCart()
        {
            if (!Session["cart"].Equals(""))
            {
                List<CartItem> listcart = (List<CartItem>)Session["cart"];
                int vt = 0;
                foreach (var item in listcart)
                {
                    listcart.RemoveAt(vt);
                    vt++;
                }
                Session["cart"] = listcart;
            }
            return RedirectToAction("Index", "Giohang");
        }

        public ActionResult ThanhToan()
        {  
            List<CartItem> listcart = (List<CartItem>)Session["cart"];
            if (Session["UserCustomer"].Equals(""))
            {
                return Redirect("~/dang-nhap");
            }

            int userid = int.Parse(Session["CustomerId"].ToString());
            User user = db.Users.Find(userid);
            ViewBag.user = user;
            return View("Thanhtoan", listcart);
        }

        public ActionResult CheckOut(FormCollection form)
        {
            // add to db to table order and orderdetail
            int userid = int.Parse(Session["CustomerId"].ToString());
            User user = db.Users.Find(userid);
            ViewBag.user = user;

            string note = form["note"];
            Order order = new Order();
            order.UserId = userid;
            order.Note = note;
            order.Status = 1;
            order.Date = DateTime.Now;
            db.Orders.Add(order);
            db.SaveChanges();
            List<CartItem> listcart = (List<CartItem>)Session["cart"];
            foreach(CartItem item in listcart)
            {
                Orderdetail orderdetail = new Orderdetail();
                orderdetail.OrderId = order.Id;
                orderdetail.ProductId = item.ProductId;
                orderdetail.Price = item.Price;
                orderdetail.Qty = item.Quantity;
                orderdetail.Amount = item.Quantity * item.Price;

                db.Orderdetails.Add(orderdetail);
                db.SaveChanges();
            }

            return Redirect("~/thanh-toan");
        }

    }
}