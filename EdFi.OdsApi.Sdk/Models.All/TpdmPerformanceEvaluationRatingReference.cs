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
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EdFi.OdsApi.Sdk.Models.All
{
    /// <summary>
    /// TpdmPerformanceEvaluationRatingReference
    /// </summary>
    [DataContract(Name = "tpdm_performanceEvaluationRatingReference")]
    public partial class TpdmPerformanceEvaluationRatingReference : IEquatable<TpdmPerformanceEvaluationRatingReference>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TpdmPerformanceEvaluationRatingReference" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected TpdmPerformanceEvaluationRatingReference() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TpdmPerformanceEvaluationRatingReference" /> class.
        /// </summary>
        /// <param name="educationOrganizationId">The identifier assigned to an education organization. (required).</param>
        /// <param name="evaluationPeriodDescriptor">The period for the evaluation. (required).</param>
        /// <param name="performanceEvaluationTitle">An assigned unique identifier for the performance evaluation. (required).</param>
        /// <param name="performanceEvaluationTypeDescriptor">The type of performance evaluation conducted. (required).</param>
        /// <param name="personId">A unique alphanumeric code assigned to a person. (required).</param>
        /// <param name="schoolYear">The identifier for the school year. (required).</param>
        /// <param name="sourceSystemDescriptor">This descriptor defines the originating record source system for the person. (required).</param>
        /// <param name="termDescriptor">The term for the session during the school year. (required).</param>
        /// <param name="link">link.</param>
        public TpdmPerformanceEvaluationRatingReference(int educationOrganizationId = default(int), string evaluationPeriodDescriptor = default(string), string performanceEvaluationTitle = default(string), string performanceEvaluationTypeDescriptor = default(string), string personId = default(string), int schoolYear = default(int), string sourceSystemDescriptor = default(string), string termDescriptor = default(string), Link link = default(Link))
        {
            this.EducationOrganizationId = educationOrganizationId;
            // to ensure "evaluationPeriodDescriptor" is required (not null)
            if (evaluationPeriodDescriptor == null)
            {
                throw new ArgumentNullException("evaluationPeriodDescriptor is a required property for TpdmPerformanceEvaluationRatingReference and cannot be null");
            }
            this.EvaluationPeriodDescriptor = evaluationPeriodDescriptor;
            // to ensure "performanceEvaluationTitle" is required (not null)
            if (performanceEvaluationTitle == null)
            {
                throw new ArgumentNullException("performanceEvaluationTitle is a required property for TpdmPerformanceEvaluationRatingReference and cannot be null");
            }
            this.PerformanceEvaluationTitle = performanceEvaluationTitle;
            // to ensure "performanceEvaluationTypeDescriptor" is required (not null)
            if (performanceEvaluationTypeDescriptor == null)
            {
                throw new ArgumentNullException("performanceEvaluationTypeDescriptor is a required property for TpdmPerformanceEvaluationRatingReference and cannot be null");
            }
            this.PerformanceEvaluationTypeDescriptor = performanceEvaluationTypeDescriptor;
            // to ensure "personId" is required (not null)
            if (personId == null)
            {
                throw new ArgumentNullException("personId is a required property for TpdmPerformanceEvaluationRatingReference and cannot be null");
            }
            this.PersonId = personId;
            this.SchoolYear = schoolYear;
            // to ensure "sourceSystemDescriptor" is required (not null)
            if (sourceSystemDescriptor == null)
            {
                throw new ArgumentNullException("sourceSystemDescriptor is a required property for TpdmPerformanceEvaluationRatingReference and cannot be null");
            }
            this.SourceSystemDescriptor = sourceSystemDescriptor;
            // to ensure "termDescriptor" is required (not null)
            if (termDescriptor == null)
            {
                throw new ArgumentNullException("termDescriptor is a required property for TpdmPerformanceEvaluationRatingReference and cannot be null");
            }
            this.TermDescriptor = termDescriptor;
            this.Link = link;
        }

        /// <summary>
        /// The identifier assigned to an education organization.
        /// </summary>
        /// <value>The identifier assigned to an education organization.</value>
        [DataMember(Name = "educationOrganizationId", IsRequired = true, EmitDefaultValue = false)]
        public int EducationOrganizationId { get; set; }

        /// <summary>
        /// The period for the evaluation.
        /// </summary>
        /// <value>The period for the evaluation.</value>
        [DataMember(Name = "evaluationPeriodDescriptor", IsRequired = true, EmitDefaultValue = false)]
        public string EvaluationPeriodDescriptor { get; set; }

        /// <summary>
        /// An assigned unique identifier for the performance evaluation.
        /// </summary>
        /// <value>An assigned unique identifier for the performance evaluation.</value>
        [DataMember(Name = "performanceEvaluationTitle", IsRequired = true, EmitDefaultValue = false)]
        public string PerformanceEvaluationTitle { get; set; }

        /// <summary>
        /// The type of performance evaluation conducted.
        /// </summary>
        /// <value>The type of performance evaluation conducted.</value>
        [DataMember(Name = "performanceEvaluationTypeDescriptor", IsRequired = true, EmitDefaultValue = false)]
        public string PerformanceEvaluationTypeDescriptor { get; set; }

        /// <summary>
        /// A unique alphanumeric code assigned to a person.
        /// </summary>
        /// <value>A unique alphanumeric code assigned to a person.</value>
        [DataMember(Name = "personId", IsRequired = true, EmitDefaultValue = false)]
        public string PersonId { get; set; }

        /// <summary>
        /// The identifier for the school year.
        /// </summary>
        /// <value>The identifier for the school year.</value>
        [DataMember(Name = "schoolYear", IsRequired = true, EmitDefaultValue = false)]
        public int SchoolYear { get; set; }

        /// <summary>
        /// This descriptor defines the originating record source system for the person.
        /// </summary>
        /// <value>This descriptor defines the originating record source system for the person.</value>
        [DataMember(Name = "sourceSystemDescriptor", IsRequired = true, EmitDefaultValue = false)]
        public string SourceSystemDescriptor { get; set; }

        /// <summary>
        /// The term for the session during the school year.
        /// </summary>
        /// <value>The term for the session during the school year.</value>
        [DataMember(Name = "termDescriptor", IsRequired = true, EmitDefaultValue = false)]
        public string TermDescriptor { get; set; }

        /// <summary>
        /// Gets or Sets Link
        /// </summary>
        [DataMember(Name = "link", EmitDefaultValue = false)]
        public Link Link { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class TpdmPerformanceEvaluationRatingReference {\n");
            sb.Append("  EducationOrganizationId: ").Append(EducationOrganizationId).Append("\n");
            sb.Append("  EvaluationPeriodDescriptor: ").Append(EvaluationPeriodDescriptor).Append("\n");
            sb.Append("  PerformanceEvaluationTitle: ").Append(PerformanceEvaluationTitle).Append("\n");
            sb.Append("  PerformanceEvaluationTypeDescriptor: ").Append(PerformanceEvaluationTypeDescriptor).Append("\n");
            sb.Append("  PersonId: ").Append(PersonId).Append("\n");
            sb.Append("  SchoolYear: ").Append(SchoolYear).Append("\n");
            sb.Append("  SourceSystemDescriptor: ").Append(SourceSystemDescriptor).Append("\n");
            sb.Append("  TermDescriptor: ").Append(TermDescriptor).Append("\n");
            sb.Append("  Link: ").Append(Link).Append("\n");
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
            return this.Equals(input as TpdmPerformanceEvaluationRatingReference);
        }

        /// <summary>
        /// Returns true if TpdmPerformanceEvaluationRatingReference instances are equal
        /// </summary>
        /// <param name="input">Instance of TpdmPerformanceEvaluationRatingReference to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TpdmPerformanceEvaluationRatingReference input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.EducationOrganizationId == input.EducationOrganizationId ||
                    this.EducationOrganizationId.Equals(input.EducationOrganizationId)
                ) && 
                (
                    this.EvaluationPeriodDescriptor == input.EvaluationPeriodDescriptor ||
                    (this.EvaluationPeriodDescriptor != null &&
                    this.EvaluationPeriodDescriptor.Equals(input.EvaluationPeriodDescriptor))
                ) && 
                (
                    this.PerformanceEvaluationTitle == input.PerformanceEvaluationTitle ||
                    (this.PerformanceEvaluationTitle != null &&
                    this.PerformanceEvaluationTitle.Equals(input.PerformanceEvaluationTitle))
                ) && 
                (
                    this.PerformanceEvaluationTypeDescriptor == input.PerformanceEvaluationTypeDescriptor ||
                    (this.PerformanceEvaluationTypeDescriptor != null &&
                    this.PerformanceEvaluationTypeDescriptor.Equals(input.PerformanceEvaluationTypeDescriptor))
                ) && 
                (
                    this.PersonId == input.PersonId ||
                    (this.PersonId != null &&
                    this.PersonId.Equals(input.PersonId))
                ) && 
                (
                    this.SchoolYear == input.SchoolYear ||
                    this.SchoolYear.Equals(input.SchoolYear)
                ) && 
                (
                    this.SourceSystemDescriptor == input.SourceSystemDescriptor ||
                    (this.SourceSystemDescriptor != null &&
                    this.SourceSystemDescriptor.Equals(input.SourceSystemDescriptor))
                ) && 
                (
                    this.TermDescriptor == input.TermDescriptor ||
                    (this.TermDescriptor != null &&
                    this.TermDescriptor.Equals(input.TermDescriptor))
                ) && 
                (
                    this.Link == input.Link ||
                    (this.Link != null &&
                    this.Link.Equals(input.Link))
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
                hashCode = (hashCode * 59) + this.EducationOrganizationId.GetHashCode();
                if (this.EvaluationPeriodDescriptor != null)
                {
                    hashCode = (hashCode * 59) + this.EvaluationPeriodDescriptor.GetHashCode();
                }
                if (this.PerformanceEvaluationTitle != null)
                {
                    hashCode = (hashCode * 59) + this.PerformanceEvaluationTitle.GetHashCode();
                }
                if (this.PerformanceEvaluationTypeDescriptor != null)
                {
                    hashCode = (hashCode * 59) + this.PerformanceEvaluationTypeDescriptor.GetHashCode();
                }
                if (this.PersonId != null)
                {
                    hashCode = (hashCode * 59) + this.PersonId.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.SchoolYear.GetHashCode();
                if (this.SourceSystemDescriptor != null)
                {
                    hashCode = (hashCode * 59) + this.SourceSystemDescriptor.GetHashCode();
                }
                if (this.TermDescriptor != null)
                {
                    hashCode = (hashCode * 59) + this.TermDescriptor.GetHashCode();
                }
                if (this.Link != null)
                {
                    hashCode = (hashCode * 59) + this.Link.GetHashCode();
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
            // EvaluationPeriodDescriptor (string) maxLength
            if (this.EvaluationPeriodDescriptor != null && this.EvaluationPeriodDescriptor.Length > 306)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for EvaluationPeriodDescriptor, length must be less than 306.", new [] { "EvaluationPeriodDescriptor" });
            }

            // PerformanceEvaluationTitle (string) maxLength
            if (this.PerformanceEvaluationTitle != null && this.PerformanceEvaluationTitle.Length > 50)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for PerformanceEvaluationTitle, length must be less than 50.", new [] { "PerformanceEvaluationTitle" });
            }

            // PerformanceEvaluationTypeDescriptor (string) maxLength
            if (this.PerformanceEvaluationTypeDescriptor != null && this.PerformanceEvaluationTypeDescriptor.Length > 306)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for PerformanceEvaluationTypeDescriptor, length must be less than 306.", new [] { "PerformanceEvaluationTypeDescriptor" });
            }

            // PersonId (string) maxLength
            if (this.PersonId != null && this.PersonId.Length > 32)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for PersonId, length must be less than 32.", new [] { "PersonId" });
            }

            // SourceSystemDescriptor (string) maxLength
            if (this.SourceSystemDescriptor != null && this.SourceSystemDescriptor.Length > 306)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for SourceSystemDescriptor, length must be less than 306.", new [] { "SourceSystemDescriptor" });
            }

            // TermDescriptor (string) maxLength
            if (this.TermDescriptor != null && this.TermDescriptor.Length > 306)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for TermDescriptor, length must be less than 306.", new [] { "TermDescriptor" });
            }

            yield break;
        }
    }

}