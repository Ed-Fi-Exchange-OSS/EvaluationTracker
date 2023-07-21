using EdFi.OdsApi.Sdk.Client;
using EdFi.OdsApi.SdkClient;

namespace eppeta.webapi.Service
{
    public class AuthenticationConfigurationService : IAuthenticationConfigurationService
    {
        private readonly string oauthUrl;
        private readonly string clientKey;
        private readonly string clientSecret;

        public AuthenticationConfigurationService(string oauthUrl, string clientKey, string clientSecret)
        {
            this.oauthUrl = oauthUrl;
            this.clientKey = clientKey;
            this.clientSecret = clientSecret;
        }

        public Configuration GetAuthenticatedConfiguration()
        {
            // Trust all SSL certs -- needed unless signed SSL certificates are configured.
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => true);

            // Explicitly configures outgoing network calls to use the latest version of TLS where possible.
            // Due to our reliance on some older libraries, the.NET framework won't necessarily default
            // to the latest unless we explicitly request it. Some hosting environments will not allow older versions
            // of TLS, and thus calls can fail without this extra configuration.
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

            // TokenRetriever makes the oauth calls. It has RestSharp dependency, install via NuGet
            var tokenRetriever = new TokenRetriever(oauthUrl, clientKey, clientSecret);

            // Plug Oauth access token. Tokens will need to be refreshed when they expire
            var configuration = new Configuration()
            {
                AccessToken = tokenRetriever.ObtainNewBearerToken(),
                BasePath = "https://localhost:443/api/data/v3/"
            };

            return configuration;
        }
    }
}
