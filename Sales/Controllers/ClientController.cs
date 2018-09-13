using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.Models;
using Sales.ViewModels;

namespace Sales.Controllers
{
    public class ClientController : Controller
    {

        private readonly SalesContext _db;
        public ClientController(SalesContext context)
        {
            _db = context;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.PromotionalCode == model.Code);
            if (user != null)
            {
                await Authenticate(model.Code); // аутентификация
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("Code", "Такой промокод не обнаружен");
            return View(model);
        }
        private async Task Authenticate(string promotionalCode)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, promotionalCode)
            };
            // создаем объект ClaimsIdentity
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        [HttpPost]
        public async Task<string> GenPromoCode()
        {
            //сгенерим рандомную строку, и представим ее как промокод
            var promo = Guid.NewGuid().ToString("N").Substring(0,6).ToUpper();
            var user = await _db.Users.FirstOrDefaultAsync(u => u.PromotionalCode == promo);
            if (user != null || string.IsNullOrEmpty(promo)) return "Ошибка генерации промокода, попробуйте снова";
            // добавляем пользователя в бд
            _db.Users.Add(new User { PromotionalCode = promo});
            await _db.SaveChangesAsync();
            return promo;
        }
    }
}
