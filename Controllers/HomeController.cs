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