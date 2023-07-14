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

        var result = await _userManager.FindByIdAsync(id);
        if (result is null || result.DeletedAt is not null) { return NotFound(); }

        return Ok(UserAccountResponse.From(result));
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _identityRepository.FindAllUsers();

        return Ok(users.Select(x => UserAccountResponse.From(x)));
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
                if (!roleSuccess)
                {
                    return new JsonResult(ModelState);
                }

                return Created($"/{Route}/{user.Id}", UserAccountResponse.From(user));
            }

            AddErrors(result);
        }

        return BadRequest(new { validationError = ModelState });


        async Task<bool> AddToRole(ApplicationUser user)
        {
            var role = Roles.MentorTeacher;
            if (AppSettings.Authentication.NewUsersAreAdministrators)
            {
                role = Roles.Administrator;
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                AddErrors(roleResult);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return roleResult.Succeeded;
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

                return NoContent();
            }
        }

        return BadRequest(ModelState);
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
        await _identityRepository.Update(lookup);

        await _userManager.RemovePasswordAsync(lookup);
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
