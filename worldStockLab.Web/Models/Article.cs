using System;
namespace worldStockLab.Web.Models
{
    

    public class Article
    {
       
            
            public int Id { get; set; } // 文章ID

            public string Title { get; set; } // 文章标题

            public string Summary { get; set; } // 文章摘要

            public string Content { get; set; } // 文章内容

            public string Category { get; set; } // 文章分类

            public string Slug { get; set; }  // 文章URL友好标识 SEO URL

            public DateTime CreatedAt { get; set; } // 发布时间

            public List<ArticleTag> ArticleTags { get; set; } //文章与标签的多对多关系

            public string? Symbol { get; set; } //关联的股票代码（可选）



    }
}
