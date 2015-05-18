using Microsoft.AspNet.Mvc;
using FaceProxy.Web.Models;
using System.Dynamic;
using System.IO;
using System.Net;

namespace FaceProxy.Web
{

    public class FaceApi {

        #region private members

        /// <summary>
        /// The service host.
        /// </summary>
        private const string ServiceHost = "https://api.projectoxford.ai/face/v0";

        /// <summary>
        /// The subscription key name.
        /// </summary>
        private const string SubscriptionKeyName = "subscription-key";

        /// <summary>
        /// The detection.
        /// </summary>
        private const string DetectionsQuery = "detections";

        /// <summary>
        /// The verification.
        /// </summary>
        private const string VerificationsQuery = "verifications";

        /// <summary>
        /// The training query.
        /// </summary>
        private const string TrainingQuery = "training";

        /// <summary>
        /// The person groups.
        /// </summary>
        private const string PersonGroupsQuery = "persongroups";

        /// <summary>
        /// The persons.
        /// </summary>
        private const string PersonsQuery = "persons";

        /// <summary>
        /// The faces query string.
        /// </summary>
        private const string FacesQuery = "faces";

        /// <summary>
        /// The identifications.
        /// </summary>
        private const string IdentificationsQuery = "identifications";

        /// <summary>
        /// The subscription key.
        /// </summary>
        private string subscriptionKey;

        /// <summary>
        /// The default resolver.
        /// </summary>
        private CamelCasePropertyNamesContractResolver defaultResolver = new CamelCasePropertyNamesContractResolver();

        #endregion
            
        /// <summary>
        /// Detects an URL asynchronously.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="analyzesFaceLandmarks">If set to <c>true</c> [analyzes face landmarks].</param>
        /// <param name="analyzesAge">If set to <c>true</c> [analyzes age].</param>
        /// <param name="analyzesGender">If set to <c>true</c> [analyzes gender].</param>
        /// <param name="analyzesHeadPose">If set to <c>true</c> [analyzes head pose].</param>
        /// <returns>The detected faces.</returns>
        public async Task<Dynamic> DetectAsync(string url, bool analyzesFaceLandmarks = false, bool analyzesAge = false, bool analyzesGender = false, bool analyzesHeadPose = false)
        {
            var requestUrl = string.Format(
                "{0}/{1}?analyzesFaceLandmarks={2}&analyzesAge={3}&analyzesGender={4}&analyzesHeadPose={5}&{6}={7}",
                ServiceHost,
                DetectionsQuery,
                analyzesFaceLandmarks,
                analyzesAge,
                analyzesGender,
                analyzesHeadPose,
                SubscriptionKeyName,
                this.subscriptionKey);
            var request = WebRequest.Create(requestUrl);

            dynamic requestBody = new ExpandoObject();
            requestBody.url = url;

            return await this.SendAsync<ExpandoObject, Dynamic>("POST", requestBody, request);
        }
    }

    public class FaceController : Controller
    {
        public JsonResult Index()
        {
            var primary = "74847df195954443bea84965b272a072";

            var response = new { name = "Radu", role = "sucker"};

            return Json(response);        
        }

        [HttpPost]
        public JsonResult Index(string tweet)
        {
            // TODO:
            // Parse json
            // Get image url from tweet: tweet.media_url
            // Send the image url to Face API with this key
            var primary = "74847df195954443bea84965b272a072";

            // adjust/parse the response?
            var response = FaceApi.DetectAsync(tweet.media_url);

            // send it back as json 
            return Json(tweet);        
        }
    }
}