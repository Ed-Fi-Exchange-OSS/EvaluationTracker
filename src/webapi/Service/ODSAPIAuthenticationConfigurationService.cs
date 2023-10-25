// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using EdFi.OdsApi.Sdk.Client;
using EdFi.OdsApi.SdkClient;

namespace eppeta.webapi.Service
{
    public class ODSAPIAuthenticationConfigurationService : IODSAPIAuthenticationConfigurationService
    {
        private readonly string oauthUrl;
        private readonly string clientKey;
        private readonly string clientSecret;

        public ODSAPIAuthenticationConfigurationService(string oauthUrl, string clientKey, string clientSecret)
        {
            this.oauthUrl = oauthUrl;
            this.clientKey = clientKey;
            this.clientSecret = clientSecret;
        }

        public async Task<Configuration> GetAuthenticatedConfiguration()
        {
            // TokenRetriever makes the oauth calls. It has RestSharp dependency, install via NuGet
            var tokenRetriever = new TokenRetriever(oauthUrl, clientKey, clientSecret);

            // Plug Oauth access token. Tokens will need to be refreshed when they expire
            var configuration = new Configuration()
            {
                AccessToken = await tokenRetriever.ObtainNewBearerToken(),
                BasePath = $"{ AppSettings.OdsApiBasePath.TrimEnd('/')}/data/v3"
            };

            return configuration;
        }
    }
}
