using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.DataAccess.Repository.IRepository;
using Web.Models;
using Web.Models.ViewModels;

namespace MyWeb_MVC.Areas.Customer
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _uniOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _uniOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new()
            {
                ShoppingCartList = _uniOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderTotal = 0
            };
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Count * cart.Price);
            }

            return View(ShoppingCartVM);
        }
        public IActionResult Sumary()
        {
            return View();
        }
        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _uniOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _uniOfWork.ShoppingCart.Update(cartFromDb);
            _uniOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _uniOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                //remove from cart
                _uniOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {

                cartFromDb.Count -= 1;
                _uniOfWork.ShoppingCart.Update(cartFromDb);
            }
            _uniOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _uniOfWork.ShoppingCart.Get(u => u.Id == cartId);
            //remove from cart
            _uniOfWork.ShoppingCart.Remove(cartFromDb);
            _uniOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        private double GetPriceBasedQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }

            }
        }
    }
}
