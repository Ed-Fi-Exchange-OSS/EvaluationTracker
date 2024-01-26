// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Account.Models;
using eppeta.webapi.Evaluations.Models;
using eppeta.webapi.Identity.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Runtime.ConstrainedExecution;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace eppeta.webapi.Account.Data
{

    // Disabled because Entity Framework sets the DbSets collections and we don't need to guard against null as we normally would.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8603 // Possible null reference
    public class AccountDbContext : DbContext, IAccountRepository
    {
        public DbSet<PasswordReset> PasswordReset { get; set; }

        public AccountDbContext(DbContextOptions<AccountDbContext> options)
            : base(options)
        {
        }

        public async Task<string> GetUserFromToken(string token)
        {
            var tokenRetrieved = await PasswordReset.Where(s => s.Token == token && s.ExpirationTime >= DateTime.Now).FirstOrDefaultAsync();
            if (tokenRetrieved != null)
            {
                return tokenRetrieved.UserId;
            }
            return string.Empty;
        }

        public async Task<bool> SavePasswordResetToken(PasswordReset passwordReset)
        {
           PasswordReset.Update(passwordReset);
           _ = await SaveChangesAsync();
            return true;
        }

        public async Task<bool> RevokePasswordResetToken(string token)
        {
            var existing = await PasswordReset.Where(s => s.Token == token).FirstOrDefaultAsync();
            if (existing != null)
            {
                PasswordReset.Remove(existing);
                _ = await SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> ValidatePasswordResetToken(string token)
        {
            var tokenRetrieved = await PasswordReset.Where(s => s.Token == token && s.ExpirationTime >= DateTime.Now).FirstOrDefaultAsync();
            if (tokenRetrieved != null)
            {
                return true;
            }
            return false;
        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
