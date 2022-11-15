Feature: ClinicalReviewTaskApi

@ClinicalReview_TaskApi @ignore
Scenario: Clinical Review Tasks are returned
	Given I have Clinical Review Tasks 'ClinicalReviewTask.json' in Mongo
	When I send a request to get Clinical Review Tasks
	Then I can see correct Tasks are returned
