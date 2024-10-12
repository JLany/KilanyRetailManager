using RetailManager.UI.Core.Configuration;
using RetailManager.UI.Core.Exceptions;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.DelegatingHandlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly IAuthenticationService _auth;
        private readonly IUserPrincipal _user;
        private bool _refreshing;

        public AuthenticationDelegatingHandler(
            IAuthenticationService auth,
            IUserPrincipal user)
        {
            _auth = auth;
            _user = user;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(
                Constants.JwtBearerScheme,
                _user.Token);

            var response = await base.SendAsync(request, cancellationToken);

            if (!_refreshing && response.StatusCode == HttpStatusCode.Unauthorized)
            {
                try
                {
                    _refreshing = true;

                    var result = await _auth.RefreshTokenAsync();

                    if (result.Success)
                    {
                        response = await SendAsync(request, cancellationToken);
                    }
                    else
                    {
                        throw new UnauthorizedException("Token refresh failed");
                    }
                }
                finally
                {
                    _refreshing = false;
                }
            }

            return response;
        }
    }
}
