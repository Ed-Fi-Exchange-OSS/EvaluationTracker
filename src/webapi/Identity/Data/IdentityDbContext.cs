// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eppeta.webapi.Identity.Data;

public class IdentityDbContext : IdentityDbContext<ApplicationUser>, IIdentityRepository
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    public async Task<IReadOnlyList<ApplicationUser>> FindAllUsers()
    {
        // Ignore the "soft deleted" records
        return await Users.Where(x => x.DeletedAt == null).ToListAsync();
    }

    public async Task<bool> Update(ApplicationUser user)
    {
        // This method does not change email address. For that, use the identity
        // UserManager object in a Controller.

        var existing = await Users.FirstOrDefaultAsync(x => x.Id == user.Id);
        if (existing is null)
        {
            return false;
        }

        existing.FirstName = user.FirstName;
        existing.LastName = user.LastName;
        existing.RequirePasswordChange = user.RequirePasswordChange;
        existing.DeletedAt = user.DeletedAt;

        _ = await SaveChangesAsync();

        return true;
    }

    public async Task RemoveAccessTokens(ApplicationUser user)
    {
        await UserTokens.Where(x => x.UserId == user.Id)
            .ForEachAsync(x => UserTokens.Remove(x));

        return;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _ = modelBuilder.HasDefaultSchema("eppeta");

        _ = modelBuilder.Entity<ApplicationUser>().ToTable("Users");
        _ = modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        _ = modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        _ = modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        _ = modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        _ = modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        _ = modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        _ = modelBuilder.Entity<ApplicationUser>().Property(x => x.Id).HasMaxLength(225);
        _ = modelBuilder.Entity<ApplicationUser>().Property(x => x.FirstName).HasMaxLength(100);
        _ = modelBuilder.Entity<ApplicationUser>().Property(x => x.LastName).HasMaxLength(100);

        _ = modelBuilder.Entity<IdentityRole>().Property(x => x.Id).HasMaxLength(225);

        _ = modelBuilder.Entity<IdentityUserClaim<string>>().Property(x => x.UserId).HasMaxLength(225);

        _ = modelBuilder.Entity<IdentityRoleClaim<string>>().Property(x => x.RoleId).HasMaxLength(225);

        _ = modelBuilder.Entity<IdentityUserLogin<string>>().Property(x => x.UserId).HasMaxLength(225);

        _ = modelBuilder.Entity<IdentityUserRole<string>>().Property(x => x.UserId).HasMaxLength(225);
        _ = modelBuilder.Entity<IdentityUserRole<string>>().Property(x => x.RoleId).HasMaxLength(225);

        _ = modelBuilder.Entity<IdentityUserToken<string>>().Property(x => x.UserId).HasMaxLength(225);
        _ = modelBuilder.Entity<IdentityUserToken<string>>().Property(x => x.LoginProvider).HasMaxLength(112);
        _ = modelBuilder.Entity<IdentityUserToken<string>>().Property(x => x.Name).HasMaxLength(112);
    }
}
