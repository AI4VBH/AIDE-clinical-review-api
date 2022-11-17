﻿Feature: ClinicalReviewTaskApi

@ClinicalReview_TaskApi
Scenario Outline: Clinical Review Tasks are returned when there are Clinical Review Tasks for the role
	Given I have Clinical Review Tasks '<clinicalReviewTasks>' in Mongo
	When I send a request to get Clinical Review Tasks with parameter <name> and <value>
	Then I can see correct Clinical Review Tasks are returned
	Examples:
	| clinicalReviewTasks     | name        | value           |
	| ClinicalReviewTask.json | roles       | clinician       |
	| ClinicalReviewTask.json | roles       | clinician,other |

@ClinicalReview_TaskApi
Scenario: Clinical Review Tasks are not returned when there are no Clinical Review Tasks for the role
	Given I have Clinical Review Tasks 'ClinicalReviewTask.json' in Mongo
	When I send a request to get Clinical Review Tasks with parameter roles and other
	Then I can see no Clinical Review Tasks are returned

@ClinicalReview_TaskApi
Scenario: No Clinical Review Tasks are returned when the DB is empty
	Given I have no Clinical Review Tasks in Mongo
	When I send a request to get Clinical Review Tasks
	Then I can see no Clinical Review Tasks are returned

@ClinicalReview_TaskApi
Scenario Outline: Correct Clinical Review Tasks are returned based on search parameters
	Given I have Clinical Review Tasks '<clinicalReviewTasks>' in Mongo
	When I send a request to get Clinical Review Tasks with parameter <name> and <value>
	Then I can see Clinical Review Tasks '<clinicalReviewTaskReturned>' are returned
	Examples: 
	| clinicalReviewTasks                                    | name            | value         | clinicalReviewTaskReturned                             |
	| ClinicalReviewTask.json,ClinicalReviewTask_Search.json | patientName     | Jane          | ClinicalReviewTask_Search.json                         |
	| ClinicalReviewTask.json,ClinicalReviewTask_Search.json | patientName     | bloggs        | ClinicalReviewTask.json,ClinicalReviewTask_Search.json |
	| ClinicalReviewTask.json,ClinicalReviewTask_Search.json | patientId       | 1234567       | ClinicalReviewTask.json                                |
	| ClinicalReviewTask.json,ClinicalReviewTask_Search.json | patientId       | 987654        | ClinicalReviewTask_Search.json                         |
	| ClinicalReviewTask.json,ClinicalReviewTask_Search.json | applicationName | application   | ClinicalReviewTask.json,ClinicalReviewTask_Search.json |
	| ClinicalReviewTask.json,ClinicalReviewTask_Search.json | applicationName | application_1 | ClinicalReviewTask.json                                |

@ClinicalReview_TaskApi
Scenario Outline: Clinical Review Tasks are not returned based on search parameters
	Given I have Clinical Review Tasks '<clinicalReviewTasks>' in Mongo
	When I send a request to get Clinical Review Tasks with parameter <name> and <value>
	Then I can see no Clinical Review Tasks are returned
	Examples: 
	| clinicalReviewTasks                                    | name            | value         | 
	| ClinicalReviewTask.json,ClinicalReviewTask_Search.json | patientName     | George        | 
	| ClinicalReviewTask.json,ClinicalReviewTask_Search.json | patientId       | 999999        | 
	| ClinicalReviewTask.json,ClinicalReviewTask_Search.json | applicationName | stroke        | 

@ClinicalReview_TaskApi
Scenario: Clinical Review service returns bad request when roles are not added
	Given I have Clinical Review Tasks 'ClinicalReviewTask.json' in Mongo
	When I send a request to get Clinical Review Tasks with no role
	Then I can Clinical Review Service Returns Bad request