using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using worldStockLab.Web.Models;

namespace worldStockLab.Web.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(UserManager<ApplicationUser> _userManager,
    RoleManager<IdentityRole> _roleManager)
        {//检查是否存在管理员角色，如果不存在则创建
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            //检查是否存在管理员用户，如果不存在则创建
            if (await _userManager.FindByNameAsync("admin") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@worldstocklab.com"
                };

                await _userManager.CreateAsync(adminUser, "Admin123!");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        public static void SeedArticles(ApplicationDbContext context)
        {
            if (context.Articles.Any()) return;

            var articles = new List<Article>
    {
        new Article {
            Title = "AI浪潮下的NVDA未来增长逻辑",
            Summary = "分析英伟达在AI时代的核心竞争力",
            Content = "随着生成式AI爆发，NVDA成为算力核心...",
            Category = "科技",
            Symbol = "NVDA",
            CreatedAt = DateTime.Now,
            Slug = "nvda-ai-growth"
        },

        new Article {
            Title = "苹果AAPL还能继续增长吗？",
            Summary = "从iPhone和服务业务分析苹果未来",
            Content = "苹果的增长逻辑正在从硬件转向服务...",
            Category = "科技",
            Symbol = "AAPL",
            CreatedAt = DateTime.Now,
            Slug = "aapl-growth"
        },

        new Article {
            Title = "微软MSFT在AI时代的布局",
            Summary = "OpenAI合作带来的巨大潜力",
            Content = "微软通过Azure和OpenAI建立生态...",
            Category = "科技",
            Symbol = "MSFT",
            CreatedAt = DateTime.Now,
            Slug = "msft-ai"
        },

        new Article {
            Title = "特斯拉TSLA的估值是否过高？",
            Summary = "从自动驾驶与能源业务看估值",
            Content = "特斯拉不仅是汽车公司...",
            Category = "新能源",
            Symbol = "TSLA",
            CreatedAt = DateTime.Now,
            Slug = "tsla-valuation"
        },

        new Article {
            Title = "谷歌GOOGL的广告业务还能增长吗",
            Summary = "广告与AI搜索的未来",
            Content = "谷歌正面临AI搜索带来的挑战...",
            Category = "互联网",
            Symbol = "GOOGL",
            CreatedAt = DateTime.Now,
            Slug = "googl-ads"
        },

        new Article {
            Title = "亚马逊AMZN云业务AWS分析",
            Summary = "AWS仍然是利润核心",
            Content = "亚马逊的核心利润来源于云业务...",
            Category = "互联网",
            Symbol = "AMZN",
            CreatedAt = DateTime.Now,
            Slug = "amzn-aws"
        },

        new Article {
            Title = "NVDA与AMD谁更值得投资？",
            Summary = "GPU双雄对比分析",
            Content = "NVDA在AI芯片领先，而AMD在性价比...",
            Category = "半导体",
            Symbol = "NVDA",
            CreatedAt = DateTime.Now,
            Slug = "nvda-vs-amd"
        },

        new Article {
            Title = "苹果Vision Pro会改变未来吗？",
            Summary = "空间计算的未来",
            Content = "苹果试图重新定义计算设备...",
            Category = "科技",
            Symbol = "AAPL",
            CreatedAt = DateTime.Now,
            Slug = "apple-vision"
        },

        new Article {
            Title = "特斯拉FSD自动驾驶进展",
            Summary = "自动驾驶商业化进程分析",
            Content = "FSD正在逐步成熟...",
            Category = "新能源",
            Symbol = "TSLA",
            CreatedAt = DateTime.Now,
            Slug = "tsla-fsd"
        },

        new Article {
            Title = "微软与OpenAI合作深度解析",
            Summary = "AI生态布局核心",
            Content = "微软通过投资OpenAI获得优势...",
            Category = "科技",
            Symbol = "MSFT",
            CreatedAt = DateTime.Now,
            Slug = "msft-openai"
        }
    };

            context.Articles.AddRange(articles);
            context.SaveChanges();
        }
    }
}
