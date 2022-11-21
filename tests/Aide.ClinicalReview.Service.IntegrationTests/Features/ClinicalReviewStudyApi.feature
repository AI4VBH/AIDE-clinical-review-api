﻿Feature: Clinical Review Study Api

@ClinicalReview_StudyApi
Scenario: Clinical Review Studies are returned
	Given I have Clinical Review studies 'ClinicalReviewStudy.json' in Mongo
	When I send a request to get Clinical Review Studies '7aa6a79a-ed9a-4d15-b501-b902aa87be4a?roles=Clinician'
	Then I can see correct Studies are returned

@ClinicalReview_StudyApi
Scenario: Clinical Review Studies are not returned when user does not have the right role
	Given I have Clinical Review studies 'ClinicalReviewStudy.json' in Mongo
	When I send a request to get Clinical Review Studies '7aa6a79a-ed9a-4d15-b501-b902aa87be4a?roles=Doctor,Nurse'
	Then Clinical Review Study Returns Bad request

@ClinicalReview_StudyApi
Scenario: Clinical Review Studies are not returned when roles are not included
	Given I have Clinical Review studies 'ClinicalReviewStudy.json' in Mongo
	When I send a request to get Clinical Review Studies '7aa6a79a-ed9a-4d15-b501-b902aa87be4a'
	Then Clinical Review Study Returns Bad request

@ClinicalReview_StudyApi
Scenario: Clinical Review Studies are not returned when default guid is provided
	Given I have Clinical Review studies 'ClinicalReviewStudy.json' in Mongo
	When I send a request to get Clinical Review Studies '00000000-0000-0000-0000-000000000000?roles=Clinician'
	Then Clinical Review Study Returns Bad request

@ClinicalReview_StudyApi
Scenario: Clinical Review Studies are not returned when execution ID does not exist in the DB
	Given I have no Clinical Review Study in Mongo
	When I send a request to get Clinical Review Studies '7aa6a79a-ed9a-4d15-b501-b902aa87be4a?roles=Clinician'
	Then Clinical Review Study Returns Not found

@ClinicalReview_StudyApi @ignore 
Scenario: Clinical Review Studies are not returned when unauthorised
	Given I have Clinical Review studies 'ClinicalReviewStudy.json' in Mongo
	When  I am a unauthorised user
	Then No Clinical Review Study is returned


