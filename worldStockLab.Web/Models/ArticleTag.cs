namespace worldStockLab.Web.Models
{
    /// 文章标签关联模型（关系表）
    public class ArticleTag
    {
        public int Id { get; set; }

        public int ArticleId { get; set; } //文章ID

        public Article Article { get; set; } //关联的文章

        public int TagId { get; set; } //标签ID

        public Tag Tag { get; set; } //关联的标签

        
     
    }
}
