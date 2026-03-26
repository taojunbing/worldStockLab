using Microsoft.AspNetCore.Identity;


namespace worldStockLab.Web.Models
{
    //应用用户类，继承自IdentityUser，可以添加自定义属性
    //IdentityUser已经包含了基本的用户属性，如用户名、密码哈希、邮箱等，我们可以在此基础上扩展更多的用户信息
    public class ApplicationUser:IdentityUser
    {
        //添加一个显示名称属性，可以在注册和用户资料中使用
        //可以扩展用户字段
        public string? DisplayName { get; set; } //显示名称
    }
}

