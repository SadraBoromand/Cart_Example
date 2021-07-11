using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cart_Example.Data;
using Cart_Example.Models;
using Cart_Example.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Cart_Example.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private MyContext _ctx;

        public OrdersController(MyContext ctx)
        {
            _ctx = ctx;
        }

        public IActionResult AddToCart(int id)
        {
            string CurrentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Order order = _ctx.Orders.SingleOrDefault(o => o.UserId == CurrentUserID && !o.IsFinaly);
            if (order == null)
            {
                order = new Order()
                {
                    UserId = CurrentUserID,
                    CreateDate = DateTime.Now,
                    IsFinaly = false,
                    Sum = 0
                };
                _ctx.Orders.Add(order);
                _ctx.SaveChanges();

                _ctx.OrderDetails.Add(new OrderDetail()
                {
                    OrderId = order.OrderId,
                    Count = 1,
                    Price = _ctx.Products.Find(id).Price,
                    ProductId = id
                });
                _ctx.SaveChanges();
            }
            else
            {
                var details = _ctx.OrderDetails.SingleOrDefault(d => d.OrderId == order.OrderId && d.ProductId == id);
                if (details == null)
                {
                    _ctx.OrderDetails.Add(new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        Count = 1,
                        Price = _ctx.Products.Find(id).Price,
                        ProductId = id
                    });
                }
                else
                {
                    details.Count += 1;
                    _ctx.Update(details);
                }

                _ctx.SaveChanges();
            }
            UpdateSumOrder(order.OrderId);
            return Redirect("/");
        }

        public void UpdateSumOrder(int orderId)
        {
            var order = _ctx.Orders.Find(orderId);
            order.Sum = _ctx.OrderDetails
                .Where(o => o.OrderId == order.OrderId)
                .Select(d => d.Count * d.Price)
                .Sum();
            _ctx.Update(order);
            _ctx.SaveChanges();
        }


        public IActionResult ShowOrder()
        {

            string CurrentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Order order = _ctx.Orders.SingleOrDefault(o => o.UserId == CurrentUserID && !o.IsFinaly);

            List<ShowOrderViewModel> _list = new List<ShowOrderViewModel>();

            if (order != null)
            {
                var orderDetails = _ctx.OrderDetails
                    .Include(o => o.Product)
                    .Where(o => o.OrderId == order.OrderId)
                    .Select(od => new ShowOrderViewModel()
                    {
                        OrderDetailId = od.OrderDeatilId,
                        Count = od.Count,
                        ImageName = od.Product.ImageName,
                        Title = od.Product.Title,
                        price = od.Price,
                        Sum = od.Count * od.Price
                    });
                _list.AddRange(orderDetails);
            }

            return View(_list);
        }

        public IActionResult Delete(int id)
        {
            var orderDetail = _ctx.OrderDetails.Find(id);
            _ctx.Remove(orderDetail);
            _ctx.SaveChanges();
            return RedirectToAction("ShowOrder");
        }
        public IActionResult Command(int id,string command)
        {
            var orderDetail = _ctx.OrderDetails.Find(id);

            switch (command)
            {
                case "up":
                    orderDetail.Count += 1;
                    _ctx.Update(orderDetail);

                    break;
                case "down":
                    orderDetail.Count -= 1;
                    _ctx.Update(orderDetail);

                    if (orderDetail.Count==0)
                    {
                        _ctx.OrderDetails.Remove(orderDetail);
                        _ctx.Update(orderDetail);

                    }
                    break;
            }

            _ctx.SaveChanges();
            return RedirectToAction("ShowOrder");
        }
    }
}
