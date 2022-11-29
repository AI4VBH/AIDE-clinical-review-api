# Copyright 2022 Guy’s and St Thomas’ NHS Foundation Trust
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