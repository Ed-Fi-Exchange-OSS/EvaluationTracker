// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Account.Models;
using eppeta.webapi.Evaluations.Models;
using Microsoft.AspNetCore.Identity;

namespace eppeta.webapi.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser() : base()
    {
    }

    public ApplicationUser(string userName) : base(userName)
    {
    }

    public bool RequirePasswordChange { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime? DeletedAt { get; set; }

    // One-to-many relationship with PerformanceEvaluationRating
    // One-to-many relationship with EvaluationRating
    // One-to-many relationship with EvaluationObjectiveRating
    // One-to-many relationship with EvaluationElementRating
    // One-to-many relationship with PasswordReset
    public ICollection<PerformanceEvaluationRating> PerformanceEvaluationRatings { get; set; } = new List<PerformanceEvaluationRating>();

    public ICollection<EvaluationRating> EvaluationRatings { get; set; } = new List<EvaluationRating>();

    public ICollection<EvaluationObjectiveRating> EvaluationObjectiveRatings { get; set; } = new List<EvaluationObjectiveRating>();

    public ICollection<EvaluationElementRating> EvaluationElementRatings { get; set; } = new List<EvaluationElementRating>();

    public ICollection<PasswordReset> PasswordReset { get; set; } = new List<PasswordReset>();

}
