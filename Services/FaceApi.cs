namespace FaceProxy.Web.Services
{
    using System.Dynamic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Serialization;
    using System;
    using Newtonsoft.Json;
    using FaceProxy.Web.Models;
    
	public class FaceApi
    {

        public FaceApi(string apiKey) {
            subscriptionKey = apiKey;
        }

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
        
        private CamelCasePropertyNamesContractResolver defaultResolver = new CamelCasePropertyNamesContractResolver();

        /// <summary>
        /// Detects an URL asynchronously.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="analyzesFaceLandmarks">If set to <c>true</c> [analyzes face landmarks].</param>
        /// <param name="analyzesAge">If set to <c>true</c> [analyzes age].</param>
        /// <param name="analyzesGender">If set to <c>true</c> [analyzes gender].</param>
        /// <param name="analyzesHeadPose">If set to <c>true</c> [analyzes head pose].</param>
        /// <returns>The detected faces.</returns>
        public async Task<dynamic> DetectAsync(string url, bool analyzesFaceLandmarks = false, bool analyzesAge = true, bool analyzesGender = false, bool analyzesHeadPose = false)
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
            var request = System.Net.WebRequest.Create(requestUrl);

            dynamic requestBody = new ExpandoObject();
            requestBody.url = url;

            return await this.SendAsync<ExpandoObject, dynamic>("POST", requestBody, request);
        }

        /// <summary>
        /// Processes the asynchronous response.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="webResponse">The web response.</param>
        /// <returns>Task object.</returns>
        private T ProcessAsyncResponse<T>(System.Net.HttpWebResponse webResponse)
        {
            using (webResponse)
            {
                if (webResponse.StatusCode == HttpStatusCode.OK ||
                    webResponse.StatusCode == HttpStatusCode.Accepted ||
                    webResponse.StatusCode == HttpStatusCode.Created)
                {
                    if (webResponse.ContentLength != 0)
                    {
                        using (var stream = webResponse.GetResponseStream())
                        {
                            if (stream != null)
                            {
                                return this.DeserializeServiceResponse<T>(stream);
                            }
                        }
                    }
                }
            }

            return default(T);
        }
        
        /// <summary>
        /// Deserializes the service response.
        /// </summary>
        /// <typeparam name="T">The type of the response.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>Service response.</returns>
        private T DeserializeServiceResponse<T>(System.IO.Stream stream)
        {
            string message = string.Empty;
            using (StreamReader reader = new StreamReader(stream))
            {
                message = reader.ReadToEnd();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ContractResolver = this.defaultResolver;

            return JsonConvert.DeserializeObject<T>(message, settings);
        }
        
        /// <summary>
        /// Set request content type.
        /// </summary>
        /// <param name="request">Web request object.</param>
        private void SetCommonHeaders(WebRequest request)
        {
            request.ContentType = "application/json";
        }
        
        /// <summary>
        /// Serialize the request body to byte array.
        /// </summary>
        /// <typeparam name="T">Type of request object.</typeparam>
        /// <param name="requestBody">Strong typed request object.</param>
        /// <returns>Byte array.</returns>
        private byte[] SerializeRequestBody<T>(T requestBody)
        {
            if (requestBody == null || requestBody is Stream)
            {
                return null;
            }
            else
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                settings.ContractResolver = this.defaultResolver;

                return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestBody, settings));
            }
        }
        
        /// <summary>
        /// The helper method to do post or put async with rest call.
        /// </summary>
        /// <typeparam name="TRequest">Type of request.</typeparam>
        /// <typeparam name="TResponse">Type of response.</typeparam>
        /// <param name="method">Http method.</param>
        /// <param name="requestBody">Request data object.</param>
        /// <param name="request">Request parameter.</param>
        /// <param name="setHeadersCallback">The set headers callback.</param>
        /// <returns>
        /// The task object.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The request.</exception>
        private async Task<TResponse> SendAsync<TRequest, TResponse>(string method, TRequest requestBody, System.Net.WebRequest request, Action<System.Net.WebRequest> setHeadersCallback = null)
        {
            try
            {
                if (request == null)
                {
                    throw new System.Exception("request");
                }

                request.Method = method;
                if (null == setHeadersCallback)
                {
                    this.SetCommonHeaders(request);
                }
                else
                {
                    setHeadersCallback(request);
                }

                if (requestBody is Stream)
                {
                    request.ContentType = "application/octet-stream";
                }

                var asyncState = new WebRequestAsyncState()
                {
                    RequestBytes = this.SerializeRequestBody(requestBody),
                    WebRequest = (HttpWebRequest)request,
                };

                var continueRequestAsyncState = await Task.Factory.FromAsync<Stream>(
                                                    asyncState.WebRequest.BeginGetRequestStream,
                                                    asyncState.WebRequest.EndGetRequestStream,
                                                    asyncState,
                                                    TaskCreationOptions.None).ContinueWith<WebRequestAsyncState>(
                                                       task =>
                                                       {
                                                           var requestAsyncState = (WebRequestAsyncState)task.AsyncState;
                                                           if (requestBody != null)
                                                           {
                                                               using (var requestStream = task.Result)
                                                               {
                                                                   if (requestBody is Stream)
                                                                   {
                                                                       var inputStream = requestBody as Stream;
                                                                       inputStream.CopyTo(requestStream);
                                                                   }
                                                                   else
                                                                   {
                                                                       requestStream.Write(requestAsyncState.RequestBytes, 0, requestAsyncState.RequestBytes.Length);
                                                                   }
                                                               }
                                                           }

                                                           return requestAsyncState;
                                                       });

                var continueWebRequest = continueRequestAsyncState.WebRequest;
                var response = await Task.Factory.FromAsync<WebResponse>(
                                            continueWebRequest.BeginGetResponse,
                                            continueWebRequest.EndGetResponse,
                                            continueRequestAsyncState);

                return this.ProcessAsyncResponse<TResponse>(response as HttpWebResponse);
            }
            catch (Exception e)
            {
                //this.HandleException(e);
                return default(TResponse);
            }
        }
        
        public string RequestUrl(bool analyzesFaceLandmarks = false, bool analyzesAge = false, bool analyzesGender = false, bool analyzesHeadPose = false) {
            return string.Format(
                "{0}/{1}?analyzesFaceLandmarks={2}&analyzesAge={3}&analyzesGender={4}&analyzesHeadPose={5}&{6}={7}",
                ServiceHost,
                DetectionsQuery,
                analyzesFaceLandmarks,
                analyzesAge,
                analyzesGender,
                analyzesHeadPose,
                SubscriptionKeyName,
                this.subscriptionKey);
        }

    }
}