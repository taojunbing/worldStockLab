namespace worldStockLab.Web.Models
{
    //标签模型
    public class Tag
    {
        public int Id { get; set; } //标签ID
        public string Name { get; set; } //标签名称
        public string Slug { get; set; } //标签URL友好标识 SEO URL

        public List<ArticleTag> ArticleTags { get; set; } //标签与文章的多对多关系
    }
}
