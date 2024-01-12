// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

namespace eppeta.webapi.Mapping
{
    public static class MappingHelper
    {
        public static void CopyMatchingPKProperties(object srcObject, object dstObject)
        {
            if (srcObject is null)
            {
                throw new ArgumentNullException(nameof(srcObject));
            }

            if (dstObject is null)
            {
                throw new ArgumentNullException(nameof(dstObject));
            }

            string[] pkCols =
                {
                "EducationOrganizationId",
                "PerformanceEvaluationTitle",
                "PerformanceEvaluationTypeDescriptor",
                "SchoolYear",
                "TermDescriptor",
                "EvaluationTitle",
                "EvaluationPeriodDescriptor",
                "EvaluationDate",
                "PersonId",
                "SourceSystemDescriptor",
                "EvaluationObjectiveTitle",
                "EvaluationElementTitle"
            };
            var srcObjType = srcObject.GetType();
            var dstObjType = dstObject.GetType();
            foreach (var pkCol in pkCols)
            {
                var srcProp = srcObjType.GetProperty(pkCol);
                if (srcProp == null)
                {
                    Console.WriteLine($"Property {pkCol} not found in object type {srcObject.GetType().Name}");
                    continue;
                }
                var dstProp = dstObjType.GetProperty(pkCol);
                if (dstProp == null)
                {
                    Console.WriteLine($"Property {pkCol} not found in object type {dstObject.GetType().Name}");
                    continue;
                }

                dstProp.SetValue(dstObject, srcProp.GetValue(srcObject));
            }
        }
    }
}
