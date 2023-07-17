// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.DTO;
using eppeta.webapi.Identity.Models;

namespace eppeta.webapi.unitTests.DTO;

[TestFixture]
public class UserAccountResponseTests
{
    private const string DefaultId = "9598e0a2-d06f-49b6-871e-f78bf179befb";
    private const string DefaultFirstName = "George";
    private const string DefaultLastName = "Washington";
    private const string DefaultEmail = "george.washington@example.com";

    public class WhenCreatingUserAccountResponse
    {
        public class GivenValidInput
        {
            private UserAccountResponse _response;

            [SetUp]
            public void SetUp()
            {
                var input = new ApplicationUser
                {
                    Email = DefaultEmail,
                    FirstName = DefaultFirstName,
                    LastName = DefaultLastName,
                    Id = DefaultId
                };

                _response = UserAccountResponse.From(input);
            }

            [Test]
            public void ThenItLoadsTheEmailAddress()
            {
                _response.Email.ShouldBe(DefaultEmail);
            }

            [Test]
            public void ThenItLoadsTheFirstName()
            {
                _response.FirstName.ShouldBe(DefaultFirstName);
            }

            [Test]
            public void ThenItLoadsTheLastName()
            {
                _response.LastName.ShouldBe(DefaultLastName);
            }

            [Test]
            public void ThenItLoadsTheId()
            {
                _response.Id.ShouldBe(DefaultId);
            }
        }
    }
}
