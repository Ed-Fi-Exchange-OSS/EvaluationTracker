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
    /// TpdmPerformanceEvaluationRatingResult
    /// </summary>
    [DataContract(Name = "tpdm_performanceEvaluationRatingResult")]
    public partial class TpdmPerformanceEvaluationRatingResult : IEquatable<TpdmPerformanceEvaluationRatingResult>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TpdmPerformanceEvaluationRatingResult" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected TpdmPerformanceEvaluationRatingResult() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TpdmPerformanceEvaluationRatingResult" /> class.
        /// </summary>
        /// <param name="rating">The numerical summary rating or score for the evaluation. (required).</param>
        /// <param name="ratingResultTitle">The title of Rating Result. (required).</param>
        /// <param name="resultDatatypeTypeDescriptor">The datatype of the result. (required).</param>
        public TpdmPerformanceEvaluationRatingResult(double rating = default, string ratingResultTitle = default, string resultDatatypeTypeDescriptor = default)
        {
            Rating = rating;
            RatingResultTitle = ratingResultTitle ?? throw new ArgumentNullException("ratingResultTitle is a required property for TpdmPerformanceEvaluationRatingResult and cannot be null");
            ResultDatatypeTypeDescriptor = resultDatatypeTypeDescriptor ?? throw new ArgumentNullException("resultDatatypeTypeDescriptor is a required property for TpdmPerformanceEvaluationRatingResult and cannot be null");
        }

        /// <summary>
        /// The numerical summary rating or score for the evaluation.
        /// </summary>
        /// <value>The numerical summary rating or score for the evaluation.</value>
        [DataMember(Name = "rating", IsRequired = true, EmitDefaultValue = false)]
        public double Rating { get; set; }

        /// <summary>
        /// The title of Rating Result.
        /// </summary>
        /// <value>The title of Rating Result.</value>
        [DataMember(Name = "ratingResultTitle", IsRequired = true, EmitDefaultValue = false)]
        public string RatingResultTitle { get; set; }

        /// <summary>
        /// The datatype of the result.
        /// </summary>
        /// <value>The datatype of the result.</value>
        [DataMember(Name = "resultDatatypeTypeDescriptor", IsRequired = true, EmitDefaultValue = false)]
        public string ResultDatatypeTypeDescriptor { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            _ = sb.Append("class TpdmPerformanceEvaluationRatingResult {\n");
            _ = sb.Append("  Rating: ").Append(Rating).Append("\n");
            _ = sb.Append("  RatingResultTitle: ").Append(RatingResultTitle).Append("\n");
            _ = sb.Append("  ResultDatatypeTypeDescriptor: ").Append(ResultDatatypeTypeDescriptor).Append("\n");
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
            return Equals(input as TpdmPerformanceEvaluationRatingResult);
        }

        /// <summary>
        /// Returns true if TpdmPerformanceEvaluationRatingResult instances are equal
        /// </summary>
        /// <param name="input">Instance of TpdmPerformanceEvaluationRatingResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TpdmPerformanceEvaluationRatingResult input)
        {
            return input != null
&& (
                    Rating == input.Rating ||
                    Rating.Equals(input.Rating)
                ) &&
                (
                    RatingResultTitle == input.RatingResultTitle ||
                    (RatingResultTitle != null &&
                    RatingResultTitle.Equals(input.RatingResultTitle))
                ) &&
                (
                    ResultDatatypeTypeDescriptor == input.ResultDatatypeTypeDescriptor ||
                    (ResultDatatypeTypeDescriptor != null &&
                    ResultDatatypeTypeDescriptor.Equals(input.ResultDatatypeTypeDescriptor))
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
                var hashCode = 41;
                hashCode = (hashCode * 59) + Rating.GetHashCode();
                if (RatingResultTitle != null)
                {
                    hashCode = (hashCode * 59) + RatingResultTitle.GetHashCode();
                }
                if (ResultDatatypeTypeDescriptor != null)
                {
                    hashCode = (hashCode * 59) + ResultDatatypeTypeDescriptor.GetHashCode();
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
            // RatingResultTitle (string) maxLength
            if (RatingResultTitle != null && RatingResultTitle.Length > 50)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for RatingResultTitle, length must be less than 50.", new[] { "RatingResultTitle" });
            }

            // ResultDatatypeTypeDescriptor (string) maxLength
            if (ResultDatatypeTypeDescriptor != null && ResultDatatypeTypeDescriptor.Length > 306)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for ResultDatatypeTypeDescriptor, length must be less than 306.", new[] { "ResultDatatypeTypeDescriptor" });
            }

            yield break;
        }
    }

}
