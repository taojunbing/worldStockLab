using Microsoft.AspNetCore.Mvc;

namespace worldStockLab.Web.Models
{
    //股票行情模型，包含股票代码、名称、当前价格、涨跌幅等信息
    public class StockQuote
    {
       //股票代码，例如AAPL
        public string Symbol { get; set; }

        //股票名称，例如Apple Inc.
        public string Name { get; set; }

        //当前价格，例如150.25
        public decimal Price { get; set; }

        //涨跌
        public decimal Change { get; set; }

        //涨跌幅，例如+1.5%或-0.8%
        public decimal ChangePercent { get; set; }

        //更新时间
        public DateTime Time { get; set; }

        public long Volume { get; set; }
        public long MarketCap { get; set; }
        public decimal High52Week { get; set; }
        public decimal Low52Week { get; set; }

    }
}
