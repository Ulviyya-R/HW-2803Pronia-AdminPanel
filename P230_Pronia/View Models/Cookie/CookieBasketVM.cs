namespace P230_Pronia.View_Models.Cookie
{
    public class CookieBasketVM
    {
        public List<CookieBasketItemVM> cookieBasketItemVMs { get; set; }
        public double TotalPrice { get; set; }

        public CookieBasketVM()
        {
            cookieBasketItemVMs = new();
        }
    }
}
