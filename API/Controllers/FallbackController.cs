using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// This controller redirect initial request comes to API to angulars Index.html file
    /// </summary>
    public class FallbackController : Controller            // to provide a MVC controller with view support
    {
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), 
                "wwwroot", "index.html"), "text/html");         // point to index.html of Angular client app
        }
    }
}