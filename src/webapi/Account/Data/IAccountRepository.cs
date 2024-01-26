// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Account.Models;
using eppeta.webapi.Evaluations.Models;
using eppeta.webapi.Identity.Models;

namespace eppeta.webapi.Account.Data;

public interface IAccountRepository
{
    Task<string> GetUserFromToken(string token);
    Task<bool> RevokePasswordResetToken(string token);
    Task<bool> SavePasswordResetToken(PasswordReset passwordReset);
    Task<bool> ValidatePasswordResetToken(string token);
}
