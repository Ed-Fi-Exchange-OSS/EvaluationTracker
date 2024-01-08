// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.DTO;
using eppeta.webapi.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Collections.Immutable;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;
using eppeta.webapi.DTO;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore;

namespace eppeta.webapi.Controllers;

/*
 * Original source:
https://github.com/openiddict/openiddict-samples/blob/dev/samples/Hollastin/Hollastin.Server/Controllers/AuthorizationController.cs
 */

public class AuthorizationController : Controller
{
    public const string TokenPath = "/connect/token";

    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthorizationController(SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [HttpPost($"~{TokenPath}"), IgnoreAntiforgeryToken, Produces("application/json")]
    public async Task<IActionResult> Exchange([FromForm] PasswordTokenRequest tokenRequest)
    {
        var openIdRequest = HttpContext.GetOpenIddictServerRequest();
        if (openIdRequest?.GrantType != GrantTypes.Password && openIdRequest?.GrantType != GrantTypes.RefreshToken)
        {
            return BadRequest(new { error = "The application only accepts the password grant type at this time." });
        }

        var user = await _userManager.FindByNameAsync(tokenRequest.Username);
        if (openIdRequest.GrantType == GrantTypes.RefreshToken)
        {
            user = null;
            // Get the claims principal associated with the refresh token.
            var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            var emailClaim = (info?.Principal?.Identity as ClaimsIdentity)?.Claims.First(z => z.Type == "email");
            if (emailClaim != null)
            {
               user = await _userManager.FindByNameAsync(emailClaim.Value);
            }

            //var user2 = await _userManager.GetUserAsync(info.Principal);
            // Retrieve the user profile corresponding to the refresh token.
            if (user == null)
            {
                var properties = new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The refresh token is no longer valid."
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            if (!await _signInManager.CanSignInAsync(user))
            {
                var properties = new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                       "The user is no longer allowed to sign in."
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
        }

        else if (openIdRequest.GrantType == GrantTypes.Password)
        {
            if (user == null)
            {
                var properties = new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The username/password couple is invalid."
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            // Validate the username/password parameters and ensure the account is not locked out.
            var result = await _signInManager.CheckPasswordSignInAsync(user, tokenRequest.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                var properties = new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The username/password couple is invalid."
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
        }

        // Create the claims-based identity that will be used by OpenIddict to generate tokens.
        var identity = new ClaimsIdentity(
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: Claims.Name,
            roleType: Claims.Role);

        // Add the claims that will be persisted in the tokens.
        _ = identity.SetClaim(Claims.Subject, await _userManager.GetUserIdAsync(user))
                .SetClaim(Claims.Email, await _userManager.GetEmailAsync(user))
                .SetClaim(Claims.Name, $"{user.FirstName} {user.LastName}")
                .SetClaims(Claims.Role, (await _userManager.GetRolesAsync(user)).ToImmutableArray());
        tokenRequest.Scopes = "offline_access";
        // Set the list of scopes granted to the client application.
        _ = identity.SetScopes(new[]
        {
                Scopes.OpenId,
                Scopes.Email,
                Scopes.Profile,
                Scopes.Roles,
                Scopes.OfflineAccess
            }.Intersect(tokenRequest.GetScopes()));

        _ = identity.SetDestinations(GetDestinations);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        switch (claim.Type)
        {
            case Claims.Name:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Profile))
                {
                    yield return Destinations.IdentityToken;
                }

                yield break;

            case Claims.Email:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Email))
                {
                    yield return Destinations.IdentityToken;
                }

                yield break;

            case Claims.Role:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Roles))
                {
                    yield return Destinations.IdentityToken;
                }

                yield break;

            // Never include the security stamp in the access and identity tokens, as it's a secret value.
            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }
}
