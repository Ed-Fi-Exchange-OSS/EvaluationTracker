// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using Microsoft.IdentityModel.Tokens;
using System.Net.NetworkInformation;

namespace eppeta.webapi;

public class AppSettings
{
    // This class follows the Singleton design pattern: there is only ever one instance, and
    // it is publicly accessible only through static methods.

    private static AppSettings? _instance;
    private readonly IConfigurationRoot _configuration;

    private AppSettings(IConfigurationRoot configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    private Lazy<Authentication> _authentication = new(() => new Authentication(
                    GetInstance()._configuration.GetValue<string>("Authentication:IssuerUrl"),
                    GetInstance()._configuration.GetValue<string>("Authentication:Audience"),
                    GetInstance()._configuration.GetValue<string>("Authentication:Authority"),
                    GetInstance()._configuration.GetValue<string>("Authentication:SigningKey"),
                    GetInstance()._configuration.GetValue<bool>("Authentication:NewUsersAreAdministrators"),
                    GetInstance()._configuration.GetValue<bool>("Authentication:RequireHttps")
                ));

    private static AppSettings GetInstance()
    {
        return _instance ?? throw new InvalidOperationException("AppSettings has not been initialized");
    }

    public static void Initialize(IConfigurationRoot configuration)
    {
        _instance = new AppSettings(configuration);
    }

    public static Authentication Authentication
    {
        get
        {
            return GetInstance()._authentication.Value;
        }
    }

    public static string[] AllowedOrigins
    {
        get
        {
            return GetInstance()._configuration.GetValue<string>("CorsAllowedOrigins").Split(",");
        }
    }

    public static string OdsApiBasePath
    {
        get
        {
            return GetInstance()._configuration.GetValue<string>("OdsApiBasePath");
        }
    }

    // Add a method to accept all SSL certs if the TrustAllSSLCerts is true in the appsettings.json file.
    public static void TrustAllSSLCerts()
    {
        if (GetInstance()._configuration.GetValue<bool>("TrustAllSSLCerts"))
        {
            // Trust all SSL certs -- needed unless signed SSL certificates are configured.
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => true);

            // Due to our reliance on some older libraries, the.NET framework won't necessarily default
            // to the latest unless we explicitly request it. Some hosting environments will not allow older versions
            // of TLS, and thus calls can fail without this extra configuration.
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
        }
    }
}

public class Authentication
{
    public Uri? IssuerUrl { get; set; }

    public string Audience { get; set; } = string.Empty;

    public string Authority { get; set; } = string.Empty;

    public SecurityKey? SigningKey { get; set; }

    public bool NewUsersAreAdministrators { get; set; } = false;

    public bool RequireHttps { get; set; } = false;

    public Authentication(string issuerUrl, string audience, string authority, string base64EncodedSigningKey, bool newUsersAreAdministrators, bool requireHttps)
    {
        IssuerUrl = new Uri(issuerUrl);
        Audience = audience;
        Authority = authority;
        SigningKey = string.IsNullOrEmpty(base64EncodedSigningKey) ? null : new SymmetricSecurityKey(Convert.FromBase64String(base64EncodedSigningKey));
        NewUsersAreAdministrators = newUsersAreAdministrators;
        RequireHttps = requireHttps;
    }

    public Authentication() { }
}
