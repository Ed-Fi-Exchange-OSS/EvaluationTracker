# Sample T-TESS Evaluation Data

We used the Texas Teacher Evaluation & Support System (T-TESS) evaluation for testing during development. The T-TESS evaluation is not a requirement to use the Evalation Tracker, but it makes for a good set of test data.

We have provided two sets of evaluation metadata for testing:

* [Spring 2024 Evaluation 1](./T-TESS%20Evaluation%20Metadata/Spring%202024%20Evaluation%202/)
* [Spring 2024 Evaluation 2](./T-TESS%20Evaluation%20Metadata/Spring%202024%20Evaluation%201/)

Each set of evaluation metadata are standalone, they share a naming convention based on the Clinical Experience and Performance starter kit so that an evaluator can easily differentiate between evaluations. Either evaluation can be loaded w/o the other, or you are free to use your own evaluation metadata.

Each set of evaluation metadata contains 5 files:

* Descriptors.json - The additional descriptors required to use the T-TESS. These EvaluationRatingLevelDescriptors include the values missing from the standard EPDM Populated Template.
* PerformanceEvaluation.json - The PerformanceEvaluation metadata
* Evaluation.json - The Evaluation metadata
* EvaluationObjectives.json - The EvaluationObjectives metadata
* EvaluationElements.json - The EvaluationElement metadata

The data in each file needs to be loaded into the ODS/API in the file order listed above. There are multiple JSON snippets in the descriptors, evaluation objectives and evaluation elements files. You will need to copy each snippet out to load into the ODS/API.
