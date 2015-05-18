using Microsoft.AspNet.Mvc;
using FaceProxy.Web.Models;

namespace FaceProxy.Web
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(CreateUser());
        }
        
        [HttpPost]
        public JsonResult Create()
        {
            var primary = "74847df195954443bea84965b272a072";
            return Json(primary);        
        }

        public User CreateUser()
        {
            User user = new User()
            {
                Name = "My name",
                Address = "My address"
            };

            return user;
        }
    }
}