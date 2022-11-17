Feature: ClinicalReviewTaskApi

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

@ClinicalReview_TaskApi @ignore
Scenario: No Clinical Review Tasks are returned when the DB is empty
	Given I have no Clinical Review Tasks in Mongo
	When I send a request to get Clinical Review Tasks
	Then I can see no Clinical Review Tasks are returned

@ClinicalReview_TaskApi @ignore
Scenario Outline: Correct Clinical Review Tasks are returned based on parameters
	Given I have Clinical Review Tasks '<clinicalReviewTasks>' in Mongo
	When I send a request to get Clinical Review Tasks with parameter <name> and <value>
	Then I can see no Clinical Review Tasks are returned
	Examples: 
	| clinicalReviewTasks | name			| value |
	|                     | patientName     | test  |
	|                     | patientName     | none  |
	|                     | patientId       | test  |
	|                     | reviewedTaskId  | test  |
	|                     | reviewedTaskId  | none  |
	|                     | roles           | none  |
