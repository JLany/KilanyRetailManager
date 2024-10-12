using RetailManager.UI.Core.Clients;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using RetailManager.UI.Core.Models.Requests;
using RetailManager.UI.Core.Models.Results;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationClient _auth;
        private readonly IApiClient _apiClient;
        private readonly IUserPrincipal _loggedInUser;

        public AuthenticationService(
            IAuthenticationClient auth,
            IApiClient apiClient,
            IUserPrincipal loggedInUser)
        {
            _auth = auth;
            _apiClient = apiClient;
            _loggedInUser = loggedInUser;
        }

        public async Task<AuthenticationResult> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new
                    ArgumentException("username and password should have values other than white space.");
            }

            var login = new LoginRequest
            {
                Username = username,
                Password = password,
                Grant_type = "password",
            };

            var response = await _auth.LoginAsync(login);

            if (!response.IsSuccessStatusCode)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    StatusCode = response.StatusCode
                };
            }

            _loggedInUser.Token = response.Content.AccessToken;
            _loggedInUser.Expiration = response.Content.Expiration;
            _loggedInUser.RefreshToken = response.Content.RefreshToken;

            return new AuthenticationResult
            {
                Success = true,
                StatusCode = response.StatusCode,
                Error = "Invalid username or password"
            };
        }

        public async Task<AuthenticationModel> AuthenticateUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new
                    ArgumentException("username and password should have values other than white space.");
            }

            //var data = new FormUrlEncodedContent(new[]
            //{
            //    new KeyValuePair<string, string>("grant_type", "password"),
            //    new KeyValuePair<string, string>("username", username),
            //    new KeyValuePair<string, string>("password", password),
            //});

            //using (var response = await _apiClient.Client.PostAsync("Token", data))

            var login = new LoginRequest
            {
                Username = username,
                Password = password,
                Grant_type = "password",
            };
            
            using (var response = await _auth.LoginAsync(login))
            {
                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.BadRequest)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                // If it is Bad Request, then read the error into the model.
                //var authentication = await response.Content.ReadAsAsync<AuthenticationModel>();
                //return authentication;
                return new AuthenticationModel();
                //return response.Content;
            }
        }

        public async Task LoadLoggedInUserInfoAsync(string token)
        {
            _apiClient.AddAuthorizationRequestHeaders(token);

            try
            {
                using (var response = await _apiClient.Client.GetAsync("/User/Info"))
                {
                    // TODO: Feature: Add Refresh Token.
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{response.ReasonPhrase}, authorization token might have been expired or currupted.");
                    }

                    var userInfo = await response.Content.ReadAsAsync<UserPrincipal>();

                    // Fill in the model that is in the DI.
                    _loggedInUser.Id = userInfo.Id;
                    _loggedInUser.FirstName = userInfo.FirstName;
                    _loggedInUser.LastName = userInfo.LastName;
                    _loggedInUser.EmailAddress = userInfo.EmailAddress;
                    _loggedInUser.CreatedDate = userInfo.CreatedDate;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _apiClient.ClearRequestHeaders();
            }
        }

        public async Task<AuthenticationResult> RefreshTokenAsync()
        {
            var request = new RefreshTokenRequest
            {
                AccessToken = _loggedInUser.Token,
                RefreshToken = _loggedInUser.RefreshToken
            };

            var response = await _auth.RefreshAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Error = "Refresh token expired or currpted",
                    StatusCode = response.StatusCode
                };
            }

            _loggedInUser.Token = response.Content.AccessToken;
            _loggedInUser.Expiration = response.Content.Expiration;

            return new AuthenticationResult
            {
                Success = true,
                StatusCode = response.StatusCode
            };
        }

        public async Task LogoutAsync()
        {
            var response = await _auth.LogoutAsync();

            _loggedInUser.Token = null;
            _loggedInUser.EmailAddress = null;
            _loggedInUser.CreatedDate = DateTime.MinValue;
            _loggedInUser.FirstName = null;
            _loggedInUser.LastName = null;
            _loggedInUser.Id = null;
            _loggedInUser.Expiration = DateTime.MinValue;
            _loggedInUser.RefreshToken = null;
        }
    }
}
