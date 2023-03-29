using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using P230_Pronia.DAL;
using P230_Pronia.Entities;
using P230_Pronia.View_Models.Cookie;

namespace P230_Pronia.Controllers
{
    public class PlantController:Controller
    {
        private readonly ProniaDbContext _context;

        public PlantController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Plant> plants = _context.Plants.
                 Include(p => p.PlantTags).ThenInclude(pt => pt.Tag).
                 Include(p => p.PlantCategories).ThenInclude(pc => pc.Category).
                 Include(p => p.PlantDeliveryInformation).
                  Include(p => p.PlantImages)
                 .ToList();
            return View(plants);
          
        }

        public IActionResult Detail(int id,int CategoryId)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Plant? plant = _context.Plants
                  .Include(p => p.PlantTags).ThenInclude(pt => pt.Tag)
                  .Include(p => p.PlantCategories).ThenInclude(pc => pc.Category)
                  .Include(p => p.PlantDeliveryInformation)
                  .Include(p => p.PlantImages)
                  .FirstOrDefault(p => p.Id == id);



            ViewBag.RelatedPlant = _context.Plants
                .Include(p => p.PlantImages)
                .Include(pc => pc.PlantCategories).ThenInclude(pc => pc.Category)

                .Where(p => p.PlantCategories.Any(pc => pc.Category.Id == CategoryId && pc.Category.Id != id && p.Id != id))
                .ToList();

            return View(plant);
        }

        

        public IActionResult AddBasket(int id)
        {
            if (id <= 0) return NotFound();
            Plant plant = _context.Plants.FirstOrDefault(p => p.Id == id);
            if (plant is null) return NotFound();
            var cookies = HttpContext.Request.Cookies["basket"];
            CookieBasketVM basket = new();
            if (cookies is null)
            {
                CookieBasketItemVM item = new CookieBasketItemVM
                {
                    Id = plant.Id,
                    Quantity = 1,
                    Price = (double)plant.Price
                };
                basket.cookieBasketItemVMs.Add(item);
                basket.TotalPrice = (double)plant.Price;
            }
            else
            {
                basket = JsonConvert.DeserializeObject<CookieBasketVM>(cookies);
                CookieBasketItemVM existed = basket.cookieBasketItemVMs.Find(c => c.Id == id);
                if (existed is null)
                {
                    CookieBasketItemVM newItem = new()
                    {
                        Id = plant.Id,
                        Quantity = 1,
                        Price = (double)plant.Price
                    };
                    basket.cookieBasketItemVMs.Add(newItem);
                    basket.TotalPrice += newItem.Price;
                }
                else
                {
                    existed.Quantity++;
                    basket.TotalPrice += existed.Price;
                }
            }
            var basketStr = JsonConvert.SerializeObject(basket);

            HttpContext.Response.Cookies.Append("basket", basketStr);

            return RedirectToAction("Index", "Plant");


        }

        public IActionResult RemoveBasket(int id)
        {
            var cookies = HttpContext.Request.Cookies["basket"];
            var basket = JsonConvert.DeserializeObject<CookieBasketVM>(cookies);
            var item = basket.cookieBasketItemVMs.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                basket.cookieBasketItemVMs.Remove(item);
                basket.TotalPrice-=item.Quantity*item.Price;
            var basketStr= JsonConvert.SerializeObject(basket);
             HttpContext.Response.Cookies.Append("basket",basketStr);
            }

            return RedirectToAction("Index", "Plant");

        }

    }
    }

