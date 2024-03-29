// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.
/*
 * Ed-Fi Operational Data Store API
 *
 * The Ed-Fi ODS / API enables applications to read and write education data stored in an Ed-Fi ODS through a secure REST interface.  ***  > *Note: Consumers of ODS / API information should sanitize all data for display and storage. The ODS / API provides reasonable safeguards against cross-site scripting attacks and other malicious content, but the platform does not and cannot guarantee that the data it contains is free of all potentially harmful content.*  *** 
 *
 * The version of the OpenAPI document: 3
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace EdFi.OdsApi.Sdk.Models.All
{
    /// <summary>
    /// TpdmCandidateRace
    /// </summary>
    [DataContract(Name = "tpdm_candidateRace")]
    public partial class TpdmCandidateRace : IEquatable<TpdmCandidateRace>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TpdmCandidateRace" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected TpdmCandidateRace() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TpdmCandidateRace" /> class.
        /// </summary>
        /// <param name="raceDescriptor">The general racial category which most clearly reflects the individual&#39;s recognition of his or her community or with which the individual most identifies. The data model allows for multiple entries so that each individual can specify all appropriate races. (required).</param>
        public TpdmCandidateRace(string raceDescriptor = default)
        {
            RaceDescriptor = raceDescriptor ?? throw new ArgumentNullException("raceDescriptor is a required property for TpdmCandidateRace and cannot be null");
        }

        /// <summary>
        /// The general racial category which most clearly reflects the individual&#39;s recognition of his or her community or with which the individual most identifies. The data model allows for multiple entries so that each individual can specify all appropriate races.
        /// </summary>
        /// <value>The general racial category which most clearly reflects the individual&#39;s recognition of his or her community or with which the individual most identifies. The data model allows for multiple entries so that each individual can specify all appropriate races.</value>
        [DataMember(Name = "raceDescriptor", IsRequired = true, EmitDefaultValue = false)]
        public string RaceDescriptor { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            _ = sb.Append("class TpdmCandidateRace {\n");
            _ = sb.Append("  RaceDescriptor: ").Append(RaceDescriptor).Append("\n");
            _ = sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return Equals(input as TpdmCandidateRace);
        }

        /// <summary>
        /// Returns true if TpdmCandidateRace instances are equal
        /// </summary>
        /// <param name="input">Instance of TpdmCandidateRace to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TpdmCandidateRace input)
        {
            return input != null
&& (RaceDescriptor == input.RaceDescriptor ||
                    (RaceDescriptor != null &&
                    RaceDescriptor.Equals(input.RaceDescriptor)));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                if (RaceDescriptor != null)
                {
                    hashCode = (hashCode * 59) + RaceDescriptor.GetHashCode();
                }
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            // RaceDescriptor (string) maxLength
            if (RaceDescriptor != null && RaceDescriptor.Length > 306)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for RaceDescriptor, length must be less than 306.", new[] { "RaceDescriptor" });
            }

            yield break;
        }
    }

}
