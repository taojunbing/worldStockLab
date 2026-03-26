namespace worldStockLab.Web.Models
{
    public class HomeViewModel
    {
        //股票数据
        public List<StockPrice> Stocks { get; set; }

        //最新文章
        public List<Article> Articles { get; set; }

        //涨跌幅榜
        public List<StockPrice> Gainers { get; set; } = new();  //初始化为一个新的空列表，避免在视图中访问时出现空引用异常

        public List<StockPrice> Losers { get; set; }=new();
        public List<StockPrice> HotStocks { get; internal set; }
    }
}
