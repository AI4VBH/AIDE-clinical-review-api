Feature: ClinicalReviewDicomApi

@ClinicalReview_DicomApi
Scenario: Correct Dicom file is returned from Minio
	Given I have Dicom files in Minio
	When I send a request to get Dicom file payload/study/workflows/task1/execution1/instance1.dcm
	Then I can see correct Dicom file is returned

@ClinicalReview_DicomApi
Scenario: Missing Dicom file returns 404
	Given I have Dicom files in Minio
	When I send a request to get Dicom file payload/study/workflows/task1/execution1/instance2.dcm
	Then I receive a not found response
