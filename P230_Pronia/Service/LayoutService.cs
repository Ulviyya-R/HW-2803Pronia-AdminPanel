using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using P230_Pronia.DAL;
using P230_Pronia.Entities;
using P230_Pronia.View_Models.Cookie;

namespace P230_Pronia.Service


{
    public class LayoutService
    {
        private readonly ProniaDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public LayoutService(ProniaDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public List<Setting> GetSettings()
        {
            List<Setting> settings = _context.Settings.ToList();
            return settings;
        }

        public CookieBasketVM? GetBasket()
        {
            var cookies = _accessor.HttpContext.Request.Cookies["basket"];
            CookieBasketVM basket=new();
            if (cookies != null)
            {
                basket = JsonConvert.DeserializeObject<CookieBasketVM>(cookies);
                foreach (CookieBasketItemVM item in basket.cookieBasketItemVMs)
                {
                    Plant plant = _context.Plants.FirstOrDefault(p => p.Id == item.Id);
                    if (plant == null)
                    {
                        basket.cookieBasketItemVMs.Remove(item);
                        basket.TotalPrice -= item.Quantity * item.Price;
                    }
                }
            }
                return basket;
        }
        
        public List<Plant> GetPlants() 
        {
            List<Plant> plant = _context.Plants.Include(p =>p.PlantImages).ToList();
            return plant;
        }
        
    }
}
