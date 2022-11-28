# Copyright 2022 Crown Copyright
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
# http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

Feature: ClinicalReviewRequestEvent

@ClinicalReviewRequestEvent
Scenario Outline: Publish a clinical review request event and see the event and study details are saved in MongoDB
	Given I have artifacts in minio <paths>
	When I publish a Clinical Review Request Event <clinicialReviewEvent>
	Then I can see ClinicalReviewRecord in Mongo
	And I can see StudyRecord in Mongo matches <studyDetails>
	Examples: 
	| paths                                                                                   | clinicialReviewEvent              | studyDetails                       |
	| study/dcm/series/, study/workflows/task1/execution1/                                    | ClinicalReviewRequestEvent_1.json | ExpectedClinicalReviewStudy_1.json |
	| study/dcm/series/, study/workflows/task2/execution1/                                    | ClinicalReviewRequestEvent_2.json | ExpectedClinicalReviewStudy_2.json |
	| study/dcm/series/, study/workflows/task1/execution1/, study/workflows/task2/execution1/ | ClinicalReviewRequestEvent_1.json | ExpectedClinicalReviewStudy_1.json |
	| study/dcm/series/, study/workflows/task3/execution1/                                    | ClinicalReviewRequestEvent_3.json | ExpectedClinicalReviewStudy_3.json |
	| study/dcm/series/, study/workflows/task4/execution1/                                    | ClinicalReviewRequestEvent_4.json | ExpectedClinicalReviewStudy_4.json |

@ClinicalReviewRequestEvent
Scenario Outline: Publish a clinical review request event and see thecorrect roles are applied
	Given I have artifacts in minio <paths>
	When I publish a Clinical Review Request Event <name>
	Then I can see the correct roles are applied
	Examples: 
	| paths                                                | name                              |
	| study/dcm/series/, study/workflows/task1/execution1/ | ClinicalReviewRequestEvent_5.json |
    | study/dcm/series/, study/workflows/task1/execution1/ | ClinicalReviewRequestEvent_6.json |
	| study/dcm/series/, study/workflows/task1/execution1/ | ClinicalReviewRequestEvent_7.json |

