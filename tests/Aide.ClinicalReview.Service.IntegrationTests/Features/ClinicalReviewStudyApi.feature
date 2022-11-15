Feature: ClinicalReviewStudyApi

@ClinicalReview_StudyApi @ignore
Scenario: Clinical Review Studies are returned
	Given I have Clinical Review studies 'ClinicalReviewStudy.json' in Mongo
	When I send a request to get Clinical Review Studies
	Then I can see correct Studies are returned
