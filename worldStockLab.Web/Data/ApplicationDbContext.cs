using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using worldStockLab.Web.Models;

namespace worldStockLab.Web.Data
{
    // 应用数据库上下文类，继承自IdentityDbContext，包含了用户身份相关的表和我们自定义的股票数据表
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        // 构造函数，接受DbContextOptions参数并传递给基类构造函数
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        

        // 定义一个DbSet属性来表示股票数据表
        //股票表
        public DbSet<Stock> Stocks { get; set; }

        //博客文章表
        public DbSet<Article> Articles { get; set; } 

        public DbSet<Tag> Tags { get; set; } //标签表

        public DbSet<ArticleTag> ArticleTags { get; set; } //文章标签关联表

        public DbSet<StockPrice> StockPrices { get; set; } //股票价格表

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 必须先调用父类
            base.OnModelCreating(builder);

            // 配置 ArticleTag 复合主键
            builder.Entity<ArticleTag>()
                .HasKey(at => new { at.ArticleId, at.TagId });  //HasKey方法指定ArticleId和TagId作为复合主键

            // ArticleTag -> Article
            builder.Entity<ArticleTag>()
                .HasOne(at => at.Article)  //HasOne方法指定ArticleTag与Article之间的一对多关系
                .WithMany(a => a.ArticleTags)  //WithMany方法指定Article与ArticleTag之间的多对一关系
                .HasForeignKey(at => at.ArticleId);  //HasForeignKey方法指定ArticleId作为外键

            // ArticleTag -> Tag
            builder.Entity<ArticleTag>()
                .HasOne(at => at.Tag)
                .WithMany(t => t.ArticleTags)
                .HasForeignKey(at => at.TagId);
        }
    }
}
