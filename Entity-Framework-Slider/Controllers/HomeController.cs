﻿using Entity_Framework_Slider.Data;
using Entity_Framework_Slider.Models;
using Entity_Framework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Diagnostics;

namespace Entity_Framework_Slider.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
			_context = context;
        }


		[HttpGet]
        public async Task<IActionResult> Index()
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

            //HttpContext.Session.SetString("name", "Pervin");

            //Response.Cookies.Append("surname", "Rehimli", new CookieOptions { MaxAge = TimeSpan.FromMinutes(30)});

            //Book book = new Book
            //{
            //	Id = 1,
            //	Name = "Xosrov ve Sirin"
            //};

            //Response.Cookies.Append("book", JsonConvert.SerializeObject(book));



            List<Slider> sliders = await _context.Sliders.ToListAsync();

            SliderInfo? sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync();

			IEnumerable<Blog> blogs = await _context.Blogs.Where(m=> !m.SoftDelete).ToListAsync();

			IEnumerable<Category> categories = await _context.Categories.Where(m => !m.SoftDelete).ToListAsync();

			IEnumerable<Product> products = await _context.Products.Include(m=>m.Images).Where(m => !m.SoftDelete).ToListAsync();

			About? about = await _context.Abouts.Where(m => !m.SoftDelete).FirstOrDefaultAsync();


            IEnumerable<Advantage> advantages = await _context.Advantages.Where(m => !m.SoftDelete).ToListAsync();


            IEnumerable<Instagram> instagram = await _context.instagrams.Where(m => !m.SoftDelete).ToListAsync();

            IEnumerable<Say> says = await _context.says.Where(m => !m.SoftDelete).ToListAsync();




            List<int> nums = new List<int>() { 1, 2, 3, 4, 5, 6 };


			var res = nums.FirstOrDefault();
			ViewBag.num = res;

			HomeVM model = new()
			{
				Sliders = sliders,
				SliderInfo = sliderInfo,
				Blogs = blogs,
				Categories = categories,
				Products = products,
				Advantages = advantages,
				About = about,
				Instagrams = instagram,
				
				says = says,

			};
			
			return View(model);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddBasket(int? id)
		{
			if (id is null) return BadRequest();

			Product dbProduct = await _context.Products.Include(m=>m.Images).FirstOrDefaultAsync(m=>m.Id == id);

			if (dbProduct == null) return NotFound();

			List<BasketVM> basket;

			if (Request.Cookies["basket"] != null)
			{
				basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
			}
			else
			{
				basket = new List<BasketVM>();
			}

			BasketVM? existProduct = basket?.FirstOrDefault(m => m.Id == dbProduct.Id);

			if (existProduct == null)
			{
                basket?.Add(new BasketVM
                {
                    Id = dbProduct.Id,
                    Count = 1,
					Price = dbProduct.Price,
					Image = dbProduct.Images.FirstOrDefault().Image,
					TotalPrice = (int)dbProduct.Price
                });
			}
			else
			{
				existProduct.Count++;
				existProduct.TotalPrice = (int)dbProduct.Price * existProduct.Count;
			}

			

			Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));

			return RedirectToAction(nameof(Index));
			
		}




		//public IActionResult Test()
		//{
		//	var sessionData = HttpContext.Session.GetString("name");
		//	var cookieData = Request.Cookies["surname"];

		//	var objectData = JsonConvert.DeserializeObject<Book>(Request.Cookies["book"]);

		//	return Json(objectData);
		//}
	}

	//class Book
	//{
 //       public int Id { get; set; }
 //       public string? Name { get; set; }
 //   }
}