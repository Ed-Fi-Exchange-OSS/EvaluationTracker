// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eppeta.webapi.Account.Models
{
    [Table("PasswordReset", Schema = "eppeta")]
    public class PasswordReset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Token { get; set; } = string.Empty;

        public DateTime ExpirationTime { get; set; }

        public PasswordReset(string userId, string token, DateTime expirationTime)
        {
            UserId = userId;
            Token = token;
            ExpirationTime = expirationTime;
        }
    }
}
