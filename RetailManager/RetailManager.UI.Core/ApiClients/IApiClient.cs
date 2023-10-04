using RetailManager.UI.Core.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.ApiClients
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
    }
}