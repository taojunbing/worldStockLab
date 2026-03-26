namespace worldStockLab.Web.Models
{
    /// StockPrice类表示股票价格信息，包括股票代码、当前价格、涨跌幅等属性
    public class StockPrice
    {
        public int Id { get; set; }

        public string Symbol { get; set; }

        public string Name { get; set; }   // 公司名称（SEO用）

        public string Type { get; set; }   // Stock / ETF

        public string Category { get; set; } // Nasdaq100 / Leveraged / Core

        public decimal Price { get; set; }

        public decimal Change { get; set; }

        public decimal ChangePercent { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int ViewCount { get; set; } = 0; // 浏览量

        // =========================
        // ⭐ 新增字段（金融数据）
        // =========================

        // 成交量
        public long Volume { get; set; }

        // 市值（单位：美元）
        public long MarketCap { get; set; }

        // 52周最高价
        public decimal High52Week { get; set; }

        // 52周最低价
        public decimal Low52Week { get; set; }

        // =========================
        // ⭐ 新增：资产类型（核心）
        // =========================
        public string AssetType { get; set; }  // Stock / ETF / Crypto


    }
}