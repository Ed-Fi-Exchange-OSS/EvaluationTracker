// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

namespace eppeta.webapi.DTO
{
    public class Candidate
    {
        public string CandidateName { get; set; } = string.Empty;
        public string PersonId { get; set; } = string.Empty;
        public string SourceSystemDescriptor { get; set; } = string.Empty;
    }
}
