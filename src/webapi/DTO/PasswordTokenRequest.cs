// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace eppeta.webapi.DTO
{
    public class PasswordTokenRequest
    {
        [FromForm(Name = "grant_type")]
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("scopes")]
        public string Scopes { get; set; } = string.Empty;

        public string[] GetScopes()
        {
            return Scopes.Split(' ');
        }
    }
}
