using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.Models;

namespace Sales.Controllers
{
    public class HomeController : Controller
    {
        private readonly SalesContext _db;


        public HomeController(SalesContext context)
        {
            _db = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            var userId = _db.Users.First(u => u.PromotionalCode == User.Identity.Name).Id;
            //Проверим на наличие заказов по данному промокоду
            if (_db.Orders.Any(c => c.UserId == userId))
            {
                //поскольку тут зависимость многие ко многим, то используем доп сущность для связки
                var userOrders = _db.Orders.First(c => c.UserId == userId);
                var orderBooks = _db.OrderBooks.Where(d => d.OrderId == userOrders.Id).Select(s => s.BookId);
                var booksByUser = _db.Books.Where(c => orderBooks.Contains(c.Id));
                //Вернем список книг, которые заказали по этому промокоду и исключим их из каталога
                ViewBag.OrderBooks = booksByUser;
                ViewBag.BookList = _db.Books.Where(c => c.Count > 0 && !orderBooks.Contains(c.Id));
            }
            else
            {
                //Если заказов нет, то выведем все доступные книги
                ViewBag.BookList = _db.Books.Where(c => c.Count > 0);
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public string CheckOrder(int[] bookIds)
        {
            //Получаем массив ИД книг, для последуещей привязки
            var userId = _db.Users.First(u => u.PromotionalCode == User.Identity.Name).Id;
            var books = _db.Books.Where(d => bookIds.Contains(d.Id)).ToList();
            foreach (var book in books)
            {
                if (book.Count > 0)
                {
                    //Отнять 1 от количества, ведь 1 промокод = 1 экземпляр книги
                    book.Count--;
                }
                else
                    return "Произошла ошибка при оформлении заказа";
            }
            //на всякий случай дополнительно проверим, мало ли 
            if (books.Sum(s=>s.Price)<2000)
            {
                return "Вы набрали на сумму менее 2000";
            }
            //проверим на наличие заказов по данному промокоду, тоже мало ли
            if (_db.Orders.Any(u=>u.UserId == userId))
            {
                return "По данному промокоду уже совершен заказ";
            }
            //сформируем записи в БД, для хранения информации по заказам
            var order = _db.Orders.Add(new Order
            {
                UserId = userId
            });
            _db.SaveChanges();
            foreach (var bId in bookIds)
            {
                _db.OrderBooks.Add(new OrderBooks
                {
                    BookId = bId,
                    OrderId = order.Entity.Id
                });
            }
            _db.SaveChanges();
            return "Заказ успешно оформлен";
        }
    }
}
