﻿using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.ApiClients
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApiClient _apiClient;
        private readonly IUserPrincipal _loggedInUser;

        public AuthenticationService(IApiClient apiClient, IUserPrincipal _loggedInUser)
        {
            _apiClient = apiClient;
            this._loggedInUser = _loggedInUser;
        }

        public async Task<AuthenticationModel> AuthenticateUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new
                    ArgumentException("username and password should have values other than white space.");
            }

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            using (var response = await _apiClient.Client.PostAsync("Token", data))
            {
                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.BadRequest)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                // If it is Bad Request, then read the error into the model.
                var authentication = await response.Content.ReadAsAsync<AuthenticationModel>();
                return authentication;
            }
        }

        public async Task LoadLoggedInUserInfoAsync(string token)
        {
            _apiClient.AddAuthorizationRequestHeaders(token);

            try
            {
                using (var response = await _apiClient.Client.GetAsync("User/Info"))
                {
                    // TODO: Feature: Add Refresh Token.
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{response.ReasonPhrase}, authorization token might have been expired or currupted.");
                    }

                    var userInfo = await response.Content.ReadAsAsync<UserPrincipal>();

                    // Fill in the model that is in the DI.
                    _loggedInUser.Id = userInfo.Id;
                    _loggedInUser.Token = token;
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

        public void EndUserSession()
        {
            _loggedInUser.Token = null;
            _loggedInUser.EmailAddress = null;
            _loggedInUser.CreatedDate = DateTime.MinValue;
            _loggedInUser.FirstName = null;
            _loggedInUser.LastName = null;
            _loggedInUser.Id = null;

            _apiClient.ClearRequestHeaders();
        }
    }
}
