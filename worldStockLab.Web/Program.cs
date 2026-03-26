using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using worldStockLab.Web.Data;
using worldStockLab.Web.Models;
using worldStockLab.Web.Services;

namespace worldStockLab.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure Entity Framework with SqlServer
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //配置ASP.NET Core Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //注册YahooFinanceService作为HTTP客户端服务，允许在应用程序中通过依赖注入使用该服务来调用Yahoo Finance API
            //builder.Services.AddHttpClient<YahooFinanceService>();
            builder.Services.AddHttpClient<FinnhubService>();

            //注册FearGreedService作为HTTP客户端服务，允许在应用程序中通过依赖注入使用该服务来调用恐惧与贪婪指数API
            builder.Services.AddHttpClient<FearGreedService>();


            builder.Services.AddHostedService<StockPriceUpdater>();

            //配置应用程序的Cookie设置，指定登录路径为/Account/Login
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });

            var app = builder.Build();

            //在应用程序启动时调用Nasdaq100Seeder.Seed方法，使用依赖注入获取ApplicationDbContext实例，并将其传递给Seed方法，以便在数据库中预填充纳斯达克100指数的相关数据
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                Nasdaq100Seeder.Seed(context);
            }


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            

            //启用静态文件中间件，允许访问wwwroot目录下的静态资源
            app.UseRouting();

            //启用认证和授权中间件
            app.UseAuthentication();

            //启用授权中间件，确保用户必须经过授权才能访问受保护的资源
            app.UseAuthorization();

            //启用静态资源中间件，允许访问wwwroot目录下的静态资源
            app.MapStaticAssets();

            //添加博客首页路由
            app.MapControllerRoute(
                name: "blog-index",
                pattern: "blog",
                defaults: new { controller = "Blog", action = "Index" });

            //添加博客详情路由，匹配/blog/{slug}的URL模式，默认使用Blog控制器的Detail方法处理请求
            app.MapControllerRoute(
                name: "blog",
                pattern: "blog/{slug?}",
                defaults: new {controller="Blog",action="Detail" });

            //添加Tag路由，匹配/tag/{tag}的URL模式，默认使用Blog控制器的Tag方法处理请求
            app.MapControllerRoute(
                name: "tag", pattern: "tag/{tag}",
                defaults: new {controller="Blog",action="Tag"});

            app.MapControllerRoute(
               name: "stock",
               pattern: "stock/{symbol}",
               defaults: new { controller = "Stock", action = "Detail" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            //.WithStaticAssets(); 通常不需要增加，默认的静态文件中间件已经处理了静态资源的请求



            //在应用启动时初始化数据库，创建默认用户和角色
            using (var scope = app.Services.CreateScope())
            {
                //获取服务提供器
                var services = scope.ServiceProvider;

                //获取UserManager和RoleManager实例，用于管理用户和角色
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                //获取ApplicationDbContext实例，用于访问数据库
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //调用DbInitializer的Initialize方法，传入UserManager和RoleManager实例，进行数据库初始化

                DbInitializer.SeedArticles(context);
                DbInitializer.Initialize(userManager, roleManager).Wait();
            }

            app.Run();
        }
    }
}
