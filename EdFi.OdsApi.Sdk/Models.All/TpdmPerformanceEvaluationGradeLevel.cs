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
    /// TpdmPerformanceEvaluationGradeLevel
    /// </summary>
    [DataContract(Name = "tpdm_performanceEvaluationGradeLevel")]
    public partial class TpdmPerformanceEvaluationGradeLevel : IEquatable<TpdmPerformanceEvaluationGradeLevel>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TpdmPerformanceEvaluationGradeLevel" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected TpdmPerformanceEvaluationGradeLevel() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TpdmPerformanceEvaluationGradeLevel" /> class.
        /// </summary>
        /// <param name="gradeLevelDescriptor">The grade levels involved with the performance evaluation. (required).</param>
        public TpdmPerformanceEvaluationGradeLevel(string gradeLevelDescriptor = default)
        {
            GradeLevelDescriptor = gradeLevelDescriptor ?? throw new ArgumentNullException("gradeLevelDescriptor is a required property for TpdmPerformanceEvaluationGradeLevel and cannot be null");
        }

        /// <summary>
        /// The grade levels involved with the performance evaluation.
        /// </summary>
        /// <value>The grade levels involved with the performance evaluation.</value>
        [DataMember(Name = "gradeLevelDescriptor", IsRequired = true, EmitDefaultValue = false)]
        public string GradeLevelDescriptor { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            _ = sb.Append("class TpdmPerformanceEvaluationGradeLevel {\n");
            _ = sb.Append("  GradeLevelDescriptor: ").Append(GradeLevelDescriptor).Append("\n");
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
            return Equals(input as TpdmPerformanceEvaluationGradeLevel);
        }

        /// <summary>
        /// Returns true if TpdmPerformanceEvaluationGradeLevel instances are equal
        /// </summary>
        /// <param name="input">Instance of TpdmPerformanceEvaluationGradeLevel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TpdmPerformanceEvaluationGradeLevel input)
        {
            return input != null
&& (GradeLevelDescriptor == input.GradeLevelDescriptor ||
                    (GradeLevelDescriptor != null &&
                    GradeLevelDescriptor.Equals(input.GradeLevelDescriptor)));
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
                if (GradeLevelDescriptor != null)
                {
                    hashCode = (hashCode * 59) + GradeLevelDescriptor.GetHashCode();
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
            // GradeLevelDescriptor (string) maxLength
            if (GradeLevelDescriptor != null && GradeLevelDescriptor.Length > 306)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for GradeLevelDescriptor, length must be less than 306.", new[] { "GradeLevelDescriptor" });
            }

            yield break;
        }
    }

}
