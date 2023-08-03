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


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EdFi.OdsApi.Sdk.Models.All
{
    /// <summary>
    /// SchoolExtensions
    /// </summary>
    [DataContract(Name = "schoolExtensions")]
    public partial class SchoolExtensions : IEquatable<SchoolExtensions>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolExtensions" /> class.
        /// </summary>
        /// <param name="tPDM">tPDM.</param>
        public SchoolExtensions(TpdmSchoolExtension tPDM = default(TpdmSchoolExtension))
        {
            this.TPDM = tPDM;
        }

        /// <summary>
        /// Gets or Sets TPDM
        /// </summary>
        [DataMember(Name = "TPDM", EmitDefaultValue = false)]
        public TpdmSchoolExtension TPDM { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class SchoolExtensions {\n");
            sb.Append("  TPDM: ").Append(TPDM).Append("\n");
            sb.Append("}\n");
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
            return this.Equals(input as SchoolExtensions);
        }

        /// <summary>
        /// Returns true if SchoolExtensions instances are equal
        /// </summary>
        /// <param name="input">Instance of SchoolExtensions to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SchoolExtensions input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.TPDM == input.TPDM ||
                    (this.TPDM != null &&
                    this.TPDM.Equals(input.TPDM))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.TPDM != null)
                {
                    hashCode = (hashCode * 59) + this.TPDM.GetHashCode();
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
            yield break;
        }
    }

}