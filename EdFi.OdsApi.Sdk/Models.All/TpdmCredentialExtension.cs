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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OpenAPIDateConverter = EdFi.OdsApi.Sdk.Client.OpenAPIDateConverter;

namespace EdFi.OdsApi.Sdk.Models.All
{
    /// <summary>
    /// TpdmCredentialExtension
    /// </summary>
    [DataContract(Name = "tpdm_credentialExtension")]
    public partial class TpdmCredentialExtension : IEquatable<TpdmCredentialExtension>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TpdmCredentialExtension" /> class.
        /// </summary>
        /// <param name="certificationRouteDescriptor">The process, program, or pathway used to obtain certification..</param>
        /// <param name="credentialStatusDescriptor">The current status of the credential (e.g., active, suspended, etc.)..</param>
        /// <param name="educatorRoleDescriptor">The specific roles or positions within an organization that the credential is intended to authorize (e.g., Principal, Reading Specialist), typically associated with service and administrative certifications..</param>
        /// <param name="boardCertificationIndicator">Indicator that the credential was granted under the authority of a national Board Certification..</param>
        /// <param name="certificationTitle">The title of the certification obtained by the educator..</param>
        /// <param name="credentialStatusDate">The month, day, and year on which the credential status was effective..</param>
        /// <param name="personReference">personReference.</param>
        /// <param name="studentAcademicRecords">An unordered collection of credentialStudentAcademicRecords. Reference to the person&#39;s Student Academic Records for the school(s) with which the Credential is associated..</param>
        public TpdmCredentialExtension(string certificationRouteDescriptor = default, string credentialStatusDescriptor = default, string educatorRoleDescriptor = default, bool? boardCertificationIndicator = default, string certificationTitle = default, DateTime? credentialStatusDate = default, EdFiPersonReference personReference = default, List<TpdmCredentialStudentAcademicRecord> studentAcademicRecords = default)
        {
            CertificationRouteDescriptor = certificationRouteDescriptor;
            CredentialStatusDescriptor = credentialStatusDescriptor;
            EducatorRoleDescriptor = educatorRoleDescriptor;
            BoardCertificationIndicator = boardCertificationIndicator;
            CertificationTitle = certificationTitle;
            CredentialStatusDate = credentialStatusDate;
            PersonReference = personReference;
            StudentAcademicRecords = studentAcademicRecords;
        }

        /// <summary>
        /// The process, program, or pathway used to obtain certification.
        /// </summary>
        /// <value>The process, program, or pathway used to obtain certification.</value>
        [DataMember(Name = "certificationRouteDescriptor", EmitDefaultValue = true)]
        public string CertificationRouteDescriptor { get; set; }

        /// <summary>
        /// The current status of the credential (e.g., active, suspended, etc.).
        /// </summary>
        /// <value>The current status of the credential (e.g., active, suspended, etc.).</value>
        [DataMember(Name = "credentialStatusDescriptor", EmitDefaultValue = true)]
        public string CredentialStatusDescriptor { get; set; }

        /// <summary>
        /// The specific roles or positions within an organization that the credential is intended to authorize (e.g., Principal, Reading Specialist), typically associated with service and administrative certifications.
        /// </summary>
        /// <value>The specific roles or positions within an organization that the credential is intended to authorize (e.g., Principal, Reading Specialist), typically associated with service and administrative certifications.</value>
        [DataMember(Name = "educatorRoleDescriptor", EmitDefaultValue = true)]
        public string EducatorRoleDescriptor { get; set; }

        /// <summary>
        /// Indicator that the credential was granted under the authority of a national Board Certification.
        /// </summary>
        /// <value>Indicator that the credential was granted under the authority of a national Board Certification.</value>
        [DataMember(Name = "boardCertificationIndicator", EmitDefaultValue = true)]
        public bool? BoardCertificationIndicator { get; set; }

        /// <summary>
        /// The title of the certification obtained by the educator.
        /// </summary>
        /// <value>The title of the certification obtained by the educator.</value>
        [DataMember(Name = "certificationTitle", EmitDefaultValue = true)]
        public string CertificationTitle { get; set; }

        /// <summary>
        /// The month, day, and year on which the credential status was effective.
        /// </summary>
        /// <value>The month, day, and year on which the credential status was effective.</value>
        [DataMember(Name = "credentialStatusDate", EmitDefaultValue = true)]
        [JsonConverter(typeof(OpenAPIDateConverter))]
        public DateTime? CredentialStatusDate { get; set; }

        /// <summary>
        /// Gets or Sets PersonReference
        /// </summary>
        [DataMember(Name = "personReference", EmitDefaultValue = false)]
        public EdFiPersonReference PersonReference { get; set; }

        /// <summary>
        /// An unordered collection of credentialStudentAcademicRecords. Reference to the person&#39;s Student Academic Records for the school(s) with which the Credential is associated.
        /// </summary>
        /// <value>An unordered collection of credentialStudentAcademicRecords. Reference to the person&#39;s Student Academic Records for the school(s) with which the Credential is associated.</value>
        [DataMember(Name = "studentAcademicRecords", EmitDefaultValue = false)]
        public List<TpdmCredentialStudentAcademicRecord> StudentAcademicRecords { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            _ = sb.Append("class TpdmCredentialExtension {\n");
            _ = sb.Append("  CertificationRouteDescriptor: ").Append(CertificationRouteDescriptor).Append("\n");
            _ = sb.Append("  CredentialStatusDescriptor: ").Append(CredentialStatusDescriptor).Append("\n");
            _ = sb.Append("  EducatorRoleDescriptor: ").Append(EducatorRoleDescriptor).Append("\n");
            _ = sb.Append("  BoardCertificationIndicator: ").Append(BoardCertificationIndicator).Append("\n");
            _ = sb.Append("  CertificationTitle: ").Append(CertificationTitle).Append("\n");
            _ = sb.Append("  CredentialStatusDate: ").Append(CredentialStatusDate).Append("\n");
            _ = sb.Append("  PersonReference: ").Append(PersonReference).Append("\n");
            _ = sb.Append("  StudentAcademicRecords: ").Append(StudentAcademicRecords).Append("\n");
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
            return Equals(input as TpdmCredentialExtension);
        }

        /// <summary>
        /// Returns true if TpdmCredentialExtension instances are equal
        /// </summary>
        /// <param name="input">Instance of TpdmCredentialExtension to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TpdmCredentialExtension input)
        {
            return input != null
&& (
                    CertificationRouteDescriptor == input.CertificationRouteDescriptor ||
                    (CertificationRouteDescriptor != null &&
                    CertificationRouteDescriptor.Equals(input.CertificationRouteDescriptor))
                ) &&
                (
                    CredentialStatusDescriptor == input.CredentialStatusDescriptor ||
                    (CredentialStatusDescriptor != null &&
                    CredentialStatusDescriptor.Equals(input.CredentialStatusDescriptor))
                ) &&
                (
                    EducatorRoleDescriptor == input.EducatorRoleDescriptor ||
                    (EducatorRoleDescriptor != null &&
                    EducatorRoleDescriptor.Equals(input.EducatorRoleDescriptor))
                ) &&
                (
                    BoardCertificationIndicator == input.BoardCertificationIndicator ||
                    (BoardCertificationIndicator != null &&
                    BoardCertificationIndicator.Equals(input.BoardCertificationIndicator))
                ) &&
                (
                    CertificationTitle == input.CertificationTitle ||
                    (CertificationTitle != null &&
                    CertificationTitle.Equals(input.CertificationTitle))
                ) &&
                (
                    CredentialStatusDate == input.CredentialStatusDate ||
                    (CredentialStatusDate != null &&
                    CredentialStatusDate.Equals(input.CredentialStatusDate))
                ) &&
                (
                    PersonReference == input.PersonReference ||
                    (PersonReference != null &&
                    PersonReference.Equals(input.PersonReference))
                ) &&
                (
                    StudentAcademicRecords == input.StudentAcademicRecords ||
                    StudentAcademicRecords != null &&
                    input.StudentAcademicRecords != null &&
                    StudentAcademicRecords.SequenceEqual(input.StudentAcademicRecords)
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
                if (CertificationRouteDescriptor != null)
                {
                    hashCode = (hashCode * 59) + CertificationRouteDescriptor.GetHashCode();
                }
                if (CredentialStatusDescriptor != null)
                {
                    hashCode = (hashCode * 59) + CredentialStatusDescriptor.GetHashCode();
                }
                if (EducatorRoleDescriptor != null)
                {
                    hashCode = (hashCode * 59) + EducatorRoleDescriptor.GetHashCode();
                }
                if (BoardCertificationIndicator != null)
                {
                    hashCode = (hashCode * 59) + BoardCertificationIndicator.GetHashCode();
                }
                if (CertificationTitle != null)
                {
                    hashCode = (hashCode * 59) + CertificationTitle.GetHashCode();
                }
                if (CredentialStatusDate != null)
                {
                    hashCode = (hashCode * 59) + CredentialStatusDate.GetHashCode();
                }
                if (PersonReference != null)
                {
                    hashCode = (hashCode * 59) + PersonReference.GetHashCode();
                }
                if (StudentAcademicRecords != null)
                {
                    hashCode = (hashCode * 59) + StudentAcademicRecords.GetHashCode();
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
            // CertificationRouteDescriptor (string) maxLength
            if (CertificationRouteDescriptor != null && CertificationRouteDescriptor.Length > 306)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for CertificationRouteDescriptor, length must be less than 306.", new[] { "CertificationRouteDescriptor" });
            }

            // CredentialStatusDescriptor (string) maxLength
            if (CredentialStatusDescriptor != null && CredentialStatusDescriptor.Length > 306)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for CredentialStatusDescriptor, length must be less than 306.", new[] { "CredentialStatusDescriptor" });
            }

            // EducatorRoleDescriptor (string) maxLength
            if (EducatorRoleDescriptor != null && EducatorRoleDescriptor.Length > 306)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for EducatorRoleDescriptor, length must be less than 306.", new[] { "EducatorRoleDescriptor" });
            }

            // CertificationTitle (string) maxLength
            if (CertificationTitle != null && CertificationTitle.Length > 64)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for CertificationTitle, length must be less than 64.", new[] { "CertificationTitle" });
            }

            yield break;
        }
    }

}
