using worldStockLab.Web.Models;
namespace worldStockLab.Web.Data
{
    public static class Nasdaq100Seeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.StockPrices.Any())
            {
                // NASDAQ-100 核心股票列表（精简但高质量版本）
                var symbols = new List<string>
            {
                "AAPL","MSFT","NVDA","AMZN","GOOGL","META","TSLA",
                "AVGO","COST","PEP","ADBE","CSCO","CMCSA","NFLX",
                "INTC","AMD","TXN","QCOM","AMGN","HON","INTU",
                "SBUX","ISRG","ADI","BKNG","MDLZ","GILD","VRTX",
                "LRCX","MU","REGN","PANW","KLAC","SNPS","CDNS",
                "ASML","ADP","FTNT","ORLY","MNST","KDP","MAR",
                "PAYX","CTAS","PCAR","ROST","EXC","XEL","EA",
                "WDAY","FAST","ODFL","CSX","NXPI","MELI","AZN",
                "IDXX","TEAM","ZS","CRWD","DDOG","SNOW","ABNB",
                "MRVL","PDD","BIDU","JD","LCID","RIVN","DOCU",
                "ZM","SHOP","SQ","PYPL","UBER","LYFT","OKTA",
                "NET","ROKU","COIN","FSLR","ENPH","SEDG",
                "DLTR","WBA","ILMN","BIIB","DXCM","SGEN",
                "CPRT","CHTR","AEP","VRSK","ANSS","MTCH"
            };
                var etfs = new List<StockPrice>
                {
                    new StockPrice { Symbol = "TQQQ", Name="ProShares UltraPro QQQ", Type="ETF", Category="Leveraged" },
                    new StockPrice { Symbol = "SQQQ", Name="ProShares UltraPro Short QQQ", Type="ETF", Category="Leveraged" },
                    new StockPrice { Symbol = "SOXL", Name="Direxion Daily Semiconductor Bull 3X", Type="ETF", Category="Leveraged" },
                    new StockPrice { Symbol = "SOXS", Name="Direxion Daily Semiconductor Bear 3X", Type="ETF", Category="Leveraged" },
                    new StockPrice { Symbol = "UPRO", Name="ProShares UltraPro S&P500", Type="ETF", Category="Leveraged" },
                    new StockPrice { Symbol = "SPXU", Name="ProShares UltraPro Short S&P500", Type="ETF", Category="Leveraged" }
                };

                //1、插入NASDAQ数据
                foreach(var symbol in symbols)
                {
                    if (!context.StockPrices.Any(s => s.Symbol == symbol))
                    {
                        context.StockPrices.Add(new StockPrice
                        {
                            Symbol = symbol,
                            Name = symbol,
                            Type = "Stock",
                            Category = "Nasdaq100",
                            Price = 0,
                            Change = 0,
                            ChangePercent = 0,
                            ViewCount = 0,
                            UpdatedAt = DateTime.Now
                        });
                    }
                }

                //插入ETF数据
                foreach(var etf in etfs)
                {
                    if (!context.StockPrices.Any(s => s.Symbol == etf.Symbol))
                    {
                        etf.Price = 0;
                        etf.Change = 0;
                        etf.ChangePercent = 0;
                        etf.ViewCount = 0;
                        etf.UpdatedAt = DateTime.Now;

                        context.StockPrices.Add(etf);
                    }
                }

                // ✅ 3. 一次性保存（性能更好）
                context.SaveChanges();
            }

               
            }
        }
    }

