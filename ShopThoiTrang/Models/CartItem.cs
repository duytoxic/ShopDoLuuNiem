using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopThoiTrang.Models
{
    [Table("CartItems")]
    public class CartItem
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public CartItem(){}
        public CartItem(int productId, string name, string image, double price, int quantity) {
            this.ProductId = productId;
            this.Name = name;
            this.Image = image;
            this.Price = price;
            this.Quantity = quantity;
            this.TotalPrice = price*quantity;
        }
    }
}