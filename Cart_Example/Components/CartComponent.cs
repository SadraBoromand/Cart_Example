using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cart_Example.Data;
using Cart_Example.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cart_Example.Components
{
    public class CartComponent : ViewComponent
    {
        private MyContext _ctx;

        public CartComponent(MyContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ShowCartViewModel> _list = new List<ShowCartViewModel>();

            if (User.Identity.IsAuthenticated)
            {
                string CurrentUserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var order = _ctx.Orders.SingleOrDefault(o => o.UserId == CurrentUserID && !o.IsFinaly);

                if (order != null)
                {
                    _list.AddRange(_ctx.OrderDetails
                        .Include(o => o.Product)
                        .Where(od => od.OrderId == order.OrderId)
                        .Select(s => new ShowCartViewModel()
                        {
                            Title = s.Product.Title,
                            Count = s.Count,
                            ImageName = s.Product.ImageName
                        }));
                }

            }

            return View("/Views/Shared/_ShowCart.cshtml", _list);
        }
    }
}
