namespace FaceProxy.Web
{
    using Microsoft.AspNet.Mvc;
    using FaceProxy.Web.Services;

    public class FaceController : Controller
    {
        private readonly FaceApi _faceApi = new FaceApi("74847df195954443bea84965b272a072");
        
        [HttpGet]
        public JsonResult Index(string url)
        {
            var response = _faceApi.DetectAsync(url);
            return Json(response);        
        }
    }
}