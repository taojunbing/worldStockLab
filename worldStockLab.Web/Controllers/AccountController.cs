using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using worldStockLab.Web.Models;

namespace worldStockLab.Web.Controllers
{
    public class AccountController:Controller
    {
        //注入SignInManager来处理登录逻辑
        private readonly SignInManager<ApplicationUser> _signInManager;

        //构造函数注入SignInManager
        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        //登录页面
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //处理登录请求
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password,string returnUrl = null)
        {
            //使用SignInManager进行登录验证
            var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // 如果有 ReturnUrl 就跳回原页面
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return Redirect("/Admin");
            }
            else
            {
                //登录失败，显示错误消息
                ModelState.AddModelError(string.Empty, "Invalid login attempt.无效的登录尝试。");

                // ViewBag.Error = "用户名或密码错误";
                return View();
            }

            
            
        }

        //注销登录
        public async Task<IActionResult> Logout()
        {
            //使用SignInManager进行注销
            await _signInManager.SignOutAsync();
            //注销成功，重定向到登录页面
            //return RedirectToAction(nameof(Login));
            return Redirect("/");
        }
    }
}
