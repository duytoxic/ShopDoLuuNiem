using System;

namespace ShopThoiTrang.Controllers
{
    internal class OrderHistory
    {
        public object Id { get; set; }
        public object CatId { get; set; }
        public object Name { get; set; }
        public object Slug { get; set; }
        public object Detail { get; set; }
        public object Metadesc { get; set; }
        public object Metakey { get; set; }
        public object Img { get; set; }
        public object Number { get; set; }
        public object Price { get; set; }
        public object Pricesale { get; set; }
        public object Status { get; set; }
        public object CateName { get; set; }
        public int OrderId { get; internal set; }
        public int ProductId { get; internal set; }
        public DateTime Date { get; internal set; }
    }
}