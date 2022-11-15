Feature: ClinicalReviewApi

@ClinicalReviewApi @ignore
Scenario: Accepted Clinical Review request generates a Task Callback
	When I accept the Clinical Review Task ClinicalReviewTask.json
	Then I can see Clinical Review Task is updated
	And I can see a Task Callback is generated
