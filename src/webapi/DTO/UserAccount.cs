// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Identity.Models;
using System.ComponentModel.DataAnnotations;

namespace eppeta.webapi.DTO
{
    public class UserAccount
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        public virtual ApplicationUser ToApplicationUser()
        {
            return new ApplicationUser { UserName = Email, Email = Email, FirstName = FirstName, LastName = LastName, RequirePasswordChange = false, Id = Guid.NewGuid().ToString() };
        }
    }
}
