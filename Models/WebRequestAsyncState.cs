namespace FaceProxy.Web.Models
{
	/// <summary>
    /// This class is used to pass on "state" between each Begin/End call
    /// It also carries the user supplied "state" object all the way till
    /// the end where is then hands off the state object to the
    /// WebRequestCallbackState object.
    /// </summary>
    internal class WebRequestAsyncState
    {
        /// <summary>
        /// Gets or sets request bytes of the request parameter for http post.
        /// </summary>
        public byte[] RequestBytes { get; set; }

        /// <summary>
        /// Gets or sets the HttpWebRequest object.
        /// </summary>
        public System.Net.HttpWebRequest WebRequest { get; set; }

        /// <summary>
        /// Gets or sets the request state object.
        /// </summary>
        public object State { get; set; }
    }
}