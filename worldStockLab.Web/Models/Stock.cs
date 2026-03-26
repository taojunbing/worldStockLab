namespace worldStockLab.Web.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string Symbol { get; set; } 
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string Sector { get; set; }
        public long MarketCap { get; set; }

        public int ViewCount { get; set; } = 0; // 浏览量
    }
}
