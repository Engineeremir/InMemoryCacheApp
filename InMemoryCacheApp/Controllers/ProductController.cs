using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheApp.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
                _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            
            MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            memoryCacheEntryOptions.AbsoluteExpiration = DateTime.UtcNow.AddSeconds(10);
            //memoryCacheEntryOptions.SlidingExpiration = TimeSpan.FromSeconds(10);
            memoryCacheEntryOptions.Priority = CacheItemPriority.High;
            memoryCacheEntryOptions.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}=>{value}=>reason:{reason}");
            });
            _memoryCache.Set<string>("time", DateTime.UtcNow.ToString(),memoryCacheEntryOptions);
            
            return View();
        }
        public IActionResult Show()
        {
            _memoryCache.TryGetValue("callback", out string callback);
            _memoryCache.TryGetValue("time", out string timeCache);


            ViewBag.time = timeCache;
            ViewBag.callback = callback;
            return View();
        }
    }
}
