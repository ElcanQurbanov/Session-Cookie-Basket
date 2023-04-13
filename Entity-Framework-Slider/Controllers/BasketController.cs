using Entity_Framework_Slider.Data;
using Entity_Framework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Entity_Framework_Slider.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context=context;
        }

        public IActionResult Index()
        {
            int basketCount;

            if (Request.Cookies["basket"] != null)
            {
                basketCount = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]).Count;
            }
            else
            {
                basketCount = 0;
            }

            ViewBag.Count = basketCount;

            List <BasketVM> basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

            return View(basket);

            

        }
    }
}
