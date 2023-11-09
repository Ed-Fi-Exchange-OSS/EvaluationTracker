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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EdFi.OdsApi.Sdk.Models.All
{
    /// <summary>
    /// TpdmEducatorPreparationProgram
    /// </summary>
    [DataContract(Name = "tpdm_educatorPreparationProgram")]
    public partial class TpdmEducatorPreparationProgram : IEquatable<TpdmEducatorPreparationProgram>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TpdmEducatorPreparationProgram" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected TpdmEducatorPreparationProgram() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TpdmEducatorPreparationProgram" /> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="programName">The name of the Educator Preparation Program. (required).</param>
        /// <param name="programTypeDescriptor">The type of program. (required).</param>
        /// <param name="educationOrganizationReference">educationOrganizationReference (required).</param>
        /// <param name="accreditationStatusDescriptor">The current accreditation status of the Educator Preparation Program..</param>
        /// <param name="gradeLevels">An unordered collection of educatorPreparationProgramGradeLevels. The grade levels served at the EPP Program..</param>
        /// <param name="programId">A unique number or alphanumeric code assigned to a program by a school, school system, a state, or other agency or entity..</param>
        /// <param name="etag">A unique system-generated value that identifies the version of the resource..</param>
        public TpdmEducatorPreparationProgram(string id = default(string), string programName = default(string), string programTypeDescriptor = default(string), EdFiEducationOrganizationReference educationOrganizationReference = default(EdFiEducationOrganizationReference), string accreditationStatusDescriptor = default(string), List<TpdmEducatorPreparationProgramGradeLevel> gradeLevels = default(List<TpdmEducatorPreparationProgramGradeLevel>), string programId = default(string), string etag = default(string))
        {
            // to ensure "programName" is required (not null)
            if (programName == null)
            {
                throw new ArgumentNullException("programName is a required property for TpdmEducatorPreparationProgram and cannot be null");
            }
            this.ProgramName = programName;
            // to ensure "programTypeDescriptor" is required (not null)
            if (programTypeDescriptor == null)
            {
                throw new ArgumentNullException("programTypeDescriptor is a required property for TpdmEducatorPreparationProgram and cannot be null");
            }
            this.ProgramTypeDescriptor = programTypeDescriptor;
            // to ensure "educationOrganizationReference" is required (not null)
            if (educationOrganizationReference == null)
            {
                throw new ArgumentNullException("educationOrganizationReference is a required property for TpdmEducatorPreparationProgram and cannot be null");
            }
            this.EducationOrganizationReference = educationOrganizationReference;
            this.Id = id;
            this.AccreditationStatusDescriptor = accreditationStatusDescriptor;
            this.GradeLevels = gradeLevels;
            this.ProgramId = programId;
            this.Etag = etag;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// The name of the Educator Preparation Program.
        /// </summary>
        /// <value>The name of the Educator Preparation Program.</value>
        [DataMember(Name = "programName", IsRequired = true, EmitDefaultValue = false)]
        public string ProgramName { get; set; }

        /// <summary>
        /// The type of program.
        /// </summary>
        /// <value>The type of program.</value>
        [DataMember(Name = "programTypeDescriptor", IsRequired = true, EmitDefaultValue = false)]
        public string ProgramTypeDescriptor { get; set; }

        /// <summary>
        /// Gets or Sets EducationOrganizationReference
        /// </summary>
        [DataMember(Name = "educationOrganizationReference", IsRequired = true, EmitDefaultValue = false)]
        public EdFiEducationOrganizationReference EducationOrganizationReference { get; set; }

        /// <summary>
        /// The current accreditation status of the Educator Preparation Program.
        /// </summary>
        /// <value>The current accreditation status of the Educator Preparation Program.</value>
        [DataMember(Name = "accreditationStatusDescriptor", EmitDefaultValue = true)]
        public string AccreditationStatusDescriptor { get; set; }

        /// <summary>
        /// An unordered collection of educatorPreparationProgramGradeLevels. The grade levels served at the EPP Program.
        /// </summary>
        /// <value>An unordered collection of educatorPreparationProgramGradeLevels. The grade levels served at the EPP Program.</value>
        [DataMember(Name = "gradeLevels", EmitDefaultValue = false)]
        public List<TpdmEducatorPreparationProgramGradeLevel> GradeLevels { get; set; }

        /// <summary>
        /// A unique number or alphanumeric code assigned to a program by a school, school system, a state, or other agency or entity.
        /// </summary>
        /// <value>A unique number or alphanumeric code assigned to a program by a school, school system, a state, or other agency or entity.</value>
        [DataMember(Name = "programId", EmitDefaultValue = true)]
        public string ProgramId { get; set; }

        /// <summary>
        /// A unique system-generated value that identifies the version of the resource.
        /// </summary>
        /// <value>A unique system-generated value that identifies the version of the resource.</value>
        [DataMember(Name = "_etag", EmitDefaultValue = false)]
        public string Etag { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class TpdmEducatorPreparationProgram {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  ProgramName: ").Append(ProgramName).Append("\n");
            sb.Append("  ProgramTypeDescriptor: ").Append(ProgramTypeDescriptor).Append("\n");
            sb.Append("  EducationOrganizationReference: ").Append(EducationOrganizationReference).Append("\n");
            sb.Append("  AccreditationStatusDescriptor: ").Append(AccreditationStatusDescriptor).Append("\n");
            sb.Append("  GradeLevels: ").Append(GradeLevels).Append("\n");
            sb.Append("  ProgramId: ").Append(ProgramId).Append("\n");
            sb.Append("  Etag: ").Append(Etag).Append("\n");
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
            return this.Equals(input as TpdmEducatorPreparationProgram);
        }

        /// <summary>
        /// Returns true if TpdmEducatorPreparationProgram instances are equal
        /// </summary>
        /// <param name="input">Instance of TpdmEducatorPreparationProgram to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TpdmEducatorPreparationProgram input)
        {
            if (input == null)
            {
                return false;
            }
            return
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) &&
                (
                    this.ProgramName == input.ProgramName ||
                    (this.ProgramName != null &&
                    this.ProgramName.Equals(input.ProgramName))
                ) &&
                (
                    this.ProgramTypeDescriptor == input.ProgramTypeDescriptor ||
                    (this.ProgramTypeDescriptor != null &&
                    this.ProgramTypeDescriptor.Equals(input.ProgramTypeDescriptor))
                ) &&
                (
                    this.EducationOrganizationReference == input.EducationOrganizationReference ||
                    (this.EducationOrganizationReference != null &&
                    this.EducationOrganizationReference.Equals(input.EducationOrganizationReference))
                ) &&
                (
                    this.AccreditationStatusDescriptor == input.AccreditationStatusDescriptor ||
                    (this.AccreditationStatusDescriptor != null &&
                    this.AccreditationStatusDescriptor.Equals(input.AccreditationStatusDescriptor))
                ) &&
                (
                    this.GradeLevels == input.GradeLevels ||
                    this.GradeLevels != null &&
                    input.GradeLevels != null &&
                    this.GradeLevels.SequenceEqual(input.GradeLevels)
                ) &&
                (
                    this.ProgramId == input.ProgramId ||
                    (this.ProgramId != null &&
                    this.ProgramId.Equals(input.ProgramId))
                ) &&
                (
                    this.Etag == input.Etag ||
                    (this.Etag != null &&
                    this.Etag.Equals(input.Etag))
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
                if (this.Id != null)
                {
                    hashCode = (hashCode * 59) + this.Id.GetHashCode();
                }
                if (this.ProgramName != null)
                {
                    hashCode = (hashCode * 59) + this.ProgramName.GetHashCode();
                }
                if (this.ProgramTypeDescriptor != null)
                {
                    hashCode = (hashCode * 59) + this.ProgramTypeDescriptor.GetHashCode();
                }
                if (this.EducationOrganizationReference != null)
                {
                    hashCode = (hashCode * 59) + this.EducationOrganizationReference.GetHashCode();
                }
                if (this.AccreditationStatusDescriptor != null)
                {
                    hashCode = (hashCode * 59) + this.AccreditationStatusDescriptor.GetHashCode();
                }
                if (this.GradeLevels != null)
                {
                    hashCode = (hashCode * 59) + this.GradeLevels.GetHashCode();
                }
                if (this.ProgramId != null)
                {
                    hashCode = (hashCode * 59) + this.ProgramId.GetHashCode();
                }
                if (this.Etag != null)
                {
                    hashCode = (hashCode * 59) + this.Etag.GetHashCode();
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
            // ProgramName (string) maxLength
            if (this.ProgramName != null && this.ProgramName.Length > 255)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for ProgramName, length must be less than 255.", new[] { "ProgramName" });
            }

            // ProgramTypeDescriptor (string) maxLength
            if (this.ProgramTypeDescriptor != null && this.ProgramTypeDescriptor.Length > 306)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for ProgramTypeDescriptor, length must be less than 306.", new[] { "ProgramTypeDescriptor" });
            }

            // AccreditationStatusDescriptor (string) maxLength
            if (this.AccreditationStatusDescriptor != null && this.AccreditationStatusDescriptor.Length > 306)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for AccreditationStatusDescriptor, length must be less than 306.", new[] { "AccreditationStatusDescriptor" });
            }

            // ProgramId (string) maxLength
            if (this.ProgramId != null && this.ProgramId.Length > 20)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for ProgramId, length must be less than 20.", new[] { "ProgramId" });
            }

            yield break;
        }
    }

}
