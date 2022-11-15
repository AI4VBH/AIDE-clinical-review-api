Feature: ClinicalReviewEvent

@ClinicalReviewEvent @ignore
Scenario: Publish a clinical review event and see the event is saved in Mongo
	When I publish a Clinical Review Event name
	Then I can see Clinical Review Task is saved in Mongo
	And I can see Clinical Review Study is saved in Mongo