// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.DTO;
using eppeta.webapi.Identity.Data;
using eppeta.webapi.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using System.Data;

namespace eppeta.webapi.Controllers;

/*
 * Original source:
 * https://github.com/openiddict/openiddict-samples/blob/dev/samples/Hollastin/Hollastin.Server/Controllers/AccountController.cs
 */

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = Roles.Administrator)]
[Route(Route)]
public class AccountController : Controller
{
    public const string Route = "accounts";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityRepository _identityRepository;
    private readonly IOpenIddictTokenManager _tokenManager;

    public AccountController(UserManager<ApplicationUser> userManager, IIdentityRepository identityRepository, IOpenIddictTokenManager tokenManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
        _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        if (id is null || id == string.Empty) { return NotFound(); }
        var userResult = await _userManager.FindByIdAsync(id);
        if (!(userResult is null || userResult.DeletedAt is not null))
        {
            IList<string> userRolesResult = await _userManager.GetRolesAsync(userResult);
            var result = new {
                user= userResult,
                roles =  userRolesResult
            };
            return Ok(UserAccountResponse.From(userResult, userRolesResult));
        }

        return NotFound();
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _identityRepository.FindAllUsers();
        var userAndRoles =
            users.Select(async x => UserAccountResponse.From(x, await _userManager.GetRolesAsync(x)));

        return Ok(userAndRoles.Select(x => x?.Result));
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] Register model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user is not null)
            {
                return EmailConflict();
            }

            user = model.ToApplicationUser();
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleSuccess = await AddToRole(user);
                return !roleSuccess ? new JsonResult(ModelState) : Created($"/{Route}/{user.Id}", UserAccountResponse.From(user));
            }

            AddErrors(result);
        }

        return BadRequest(new { validationError = ModelState });


        async Task<bool> AddToRole(ApplicationUser user)
        {
            if (AppSettings.Authentication.NewUsersAreAdministrators)
            {
                var role = Roles.Administrator;
                var roleResult = await _userManager.AddToRoleAsync(user, role);
                if (!roleResult.Succeeded)
                {
                    AddErrors(roleResult);
                    Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
                return roleResult.Succeeded;
            }
            return true;           
        }
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromRoute] string id, [FromBody] UserAccountUpdateRequest model)
    {
        if (ModelState.IsValid)
        {
            if (id != model.Id)
            {
                ModelState.AddModelError("id", "Id in URL does not match the Id in the message body.");
                return BadRequest(ModelState);
            }

            var lookup = await _userManager.FindByIdAsync(id);
            if (lookup is null || lookup.DeletedAt is not null)
            {
                return NotFound();
            }

            lookup.FirstName = model.FirstName;
            lookup.LastName = model.LastName;
            lookup.RequirePasswordChange = model.RequirePasswordChange;

            if (model.EmailHasChanged(lookup))
            {
                lookup.UserName = lookup.Email = model.Email;
                var updateEmailResult = await _userManager.UpdateAsync(lookup);
                if (updateEmailResult.Succeeded)
                {
                    if (updateEmailResult.Succeeded)
                    {
                        var roleSuccess = await UpdateUserRoles(lookup, model.Roles);
                        return !roleSuccess ? new JsonResult(ModelState) : Created($"/{Route}/{lookup.Id}", UserAccountResponse.From(lookup));
                    }
                    return NoContent();
                }

                AddErrors(updateEmailResult);
            }
            else
            {
                // The UpdateAsync function always tries to replace the email
                // address. If you haven't changed the email address, it fails.
                // Therefore we need a workaround for changing properties
                // _other than_ email address.
                var updateResult = await _identityRepository.Update(lookup);
                if (!updateResult)
                {
                    // This means the user wasn't found, which shouldn't happen given that we have a lookup above.
                    throw new InvalidOperationException("User update failed because user no longer exists.");
                }
                if (updateResult)
                {
                    var roleSuccess = await UpdateUserRoles(lookup, model.Roles);
                    return !roleSuccess ? new JsonResult(ModelState) : Created($"/{Route}/{lookup.Id}", UserAccountResponse.From(lookup));
                }
                return NoContent();
            }
        }
        return BadRequest(ModelState);

        async Task<bool> UpdateUserRoles(ApplicationUser user, IEnumerable<string>? roles)
        {
            IList<string> currentUserRoles = await _userManager.GetRolesAsync(user);
            // Find items to remove (exist in currentItems but not in receivedItems)
            var itemsToRemove = roles?.Any() ?? false ? currentUserRoles.Except(roles).ToList() : currentUserRoles;
            if (itemsToRemove?.Any() ?? false)
            {
                // remove existing roles
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, itemsToRemove);
            }
            // add new roles
            if (roles?.Any() ?? false)
            {
                // check the list of roles to add, if already exist don't try to add again
                var itemsToAdd = roles.Except(currentUserRoles).ToList();
                if (itemsToAdd.Any())
                {
                    var roleResult = await _userManager.AddToRolesAsync(user, itemsToAdd);
                    if (!roleResult.Succeeded)
                    {
                        AddErrors(roleResult);
                        Response.StatusCode = StatusCodes.Status500InternalServerError;
                    }
                    return roleResult.Succeeded;
                }
            }
            return true;
        }
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        // Note: only performs a "soft delete" - does not remove the record from the database

        var lookup = await _userManager.FindByIdAsync(id);
        if (lookup is null)
        {
            return NotFound();
        }

        lookup.DeletedAt = DateTime.UtcNow;
        _ = await _identityRepository.Update(lookup);

        _ = await _userManager.RemovePasswordAsync(lookup);
        await _identityRepository.RemoveAccessTokens(lookup);

        var tokens = _tokenManager.FindBySubjectAsync(id);
        await foreach (OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreToken token in tokens)
        {
            token.ExpirationDate = DateTime.UtcNow.AddMinutes(-1);
            await _tokenManager.UpdateAsync(token);
        }

        return NoContent();
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ClientError.AddValidationError(ModelState, error.Description);
        }
    }

    private IActionResult EmailConflict()
    {
        return StatusCode(StatusCodes.Status409Conflict, new { error = "The requested email address is already in use." });
    }

}
