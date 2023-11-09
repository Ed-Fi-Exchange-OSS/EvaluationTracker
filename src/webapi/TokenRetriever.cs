using System.Net;
using System.Runtime.Serialization;
using System.Security.Authentication;
using EdFi.OdsApi.Sdk.Client;

namespace EdFi.OdsApi.SdkClient
{
    public class TokenRetriever
    {
        private readonly string _oauthUrl;
        private readonly string _clientKey;
        private readonly string _clientSecret;

        public TokenRetriever(string oauthUrl, string clientKey, string clientSecret)
        {
            _oauthUrl = oauthUrl;
            _clientKey = clientKey;
            _clientSecret = clientSecret;
        }

        public async Task<string> ObtainNewBearerToken()
        {
            var oauthClient = new ApiClient(_oauthUrl);

            var configuration = new Configuration() { BasePath = _oauthUrl };
            var bearerTokenRequestOptions = new RequestOptions() { Operation = string.Empty };
            bearerTokenRequestOptions.FormParameters.Add("client_id", _clientKey);
            bearerTokenRequestOptions.FormParameters.Add("client_secret", _clientSecret);
            bearerTokenRequestOptions.FormParameters.Add("grant_type", "client_credentials");

            var bearerTokenResponse = await oauthClient.PostAsync<BearerTokenResponse>("oauth/token", bearerTokenRequestOptions, configuration);
            if (bearerTokenResponse.StatusCode != HttpStatusCode.OK)
            {
                var message = string.IsNullOrWhiteSpace(bearerTokenResponse.Data.Error) ? bearerTokenResponse.RawContent : bearerTokenResponse.Data.Error;

                throw new AuthenticationException($"Unable to retrieve an access token. Error message: ${message}");
            }

            if (bearerTokenResponse.Data.Error != string.Empty || bearerTokenResponse.Data.TokenType != "bearer")
            {
                throw new AuthenticationException(
                    "Unable to retrieve an access token. Please verify that your application secret is correct.");
            }

            return bearerTokenResponse.Data.AccessToken;
        }
    }

    [DataContract]
    internal class BearerTokenResponse
    {
        [DataMember(Name = "access_token", EmitDefaultValue = false)]
        public string AccessToken { get; set; } = string.Empty;

        [DataMember(Name = "expires_in", EmitDefaultValue = false)]
        public string ExpiresIn { get; set; } = string.Empty;

        [DataMember(Name = "token_type", EmitDefaultValue = false)]
        public string TokenType { get; set; } = string.Empty;

        [DataMember(Name = "error", EmitDefaultValue = false)]
        public string Error { get; set; } = string.Empty;
    }
}
