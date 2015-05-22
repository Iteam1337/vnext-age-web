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
            string [] origin = new string [1];
            origin[0] = "*";
            Response.Headers.Add("Access-Control-Allow-Origin", origin);
            var response = _faceApi.DetectAsync(url, true, true, true, true);
            return Json(response);        
        }
    }
}