using System.Net.Http;

namespace RetailManager.UI.Core.Interfaces
{
    /// <summary>
    /// Any UI Component trying to connect with the API should use this service to connect through.
    /// Direct connection is discouraged.
    /// </summary>
    public interface IApiClient
    {
        /// <summary>
        /// Internal client for connecting with the API.
        /// </summary>
        HttpClient Client { get; }

        void AddAuthorizationRequestHeaders(string token);
        void ClearRequestHeaders();
    }
}