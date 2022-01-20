using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrang.Models;
using System.IO;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private ShopThoiTrangDbContext db = new ShopThoiTrangDbContext();

        // GET: Admin/Product
        public ActionResult Index()
        {
            var list = db.Products
                .Join(
                    db.Categorys,
                    p => p.CatId,
                    c => c.Id,
                    (p,c) => new ProductCategory
                    {
                        Id = p.Id,
                        CatId = p.CatId,
                        Name = p.Name,
                        Slug = p.Slug,
                        Detail = p.Detail,
                        Metadesc = p.Metadesc,
                        Metakey = p.Metakey,
                        Img = p.Img,
                        Number = p.Number,
                        Price = p.Price,
                        Pricesale = p.Pricesale,
                        Status = p.Status,
                        CateName = c.Name,
                    }
                )
                .Where(m => m.Status != 0)
                .ToList();
            return View(list);
        }

        public ActionResult Trash()
        {
            var list = db.Products.Where(m => m.Status == 0).OrderByDescending(m => m.Created_At).ToList();
            return View("Trash", list);
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                string slug = product.Name.ToLower();
                // xu ly hinh anh
                var Img = Request.Files["fileimg"];
                string[] FileExetention = { ".jpg", ".png", ".gif" };
                if(Img.ContentLength != 0)
                {
                    if (FileExetention.Contains(Img.FileName.Substring(Img.FileName.LastIndexOf("."))))
                    {
                        // ok -> upload file image
                        string imgName = slug + Img.FileName.Substring(Img.FileName.LastIndexOf("."));
                        product.Img = imgName; // save to database
                        string PathImg = Path.Combine(Server.MapPath("~/Public/images/product"), imgName);
                        Img.SaveAs(PathImg); // save file to server
                    }
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                string slug = product.Name.ToLower();
                // xu ly hinh anh
                var Img = Request.Files["fileimg"];
                string[] FileExetention = { ".jpg", ".png", ".gif" };
                if (Img.ContentLength != 0)
                {
                    if (FileExetention.Contains(Img.FileName.Substring(Img.FileName.LastIndexOf("."))))
                    {
                        // ok -> upload file image
                        string imgName = slug + Img.FileName.Substring(Img.FileName.LastIndexOf("."));
                        product.Img = imgName; // save to database
                        // delete image
                        string DelPath = Path.Combine(Server.MapPath("~/Public/images/product"), product.Img);
                        if (System.IO.File.Exists(DelPath))
                        {
                            System.IO.File.Delete(DelPath);
                        }

                        string PathImg = Path.Combine(Server.MapPath("~/Public/images/product"), imgName);
                        Img.SaveAs(PathImg); // save file to server
                    }
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //change status category 1 =2, 2 =1
        public ActionResult Status(int id)
        {
            Product product = db.Products.Find(id);
            int status = product.Status == 1 ? 2 : 1;
            product.Status = status;
            product.Updated_By = 1;
            product.Updated_At = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // delete item status = 0
        public ActionResult DelTrash(int id)
        {
            Product product = db.Products.Find(id);
            product.Status = 0;
            product.Updated_By = 1;
            product.Updated_At = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        // restore item in trash status => 2
        public ActionResult ReTrash(int id)
        {
            Product product = db.Products.Find(id);
            product.Status = 2;
            product.Updated_By = 1;
            product.Updated_At = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash", "Product");
        }
    }
}
