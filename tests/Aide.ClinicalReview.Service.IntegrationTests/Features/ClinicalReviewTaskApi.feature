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

Feature: ClinicalReviewTaskApi

    @ClinicalReview_TaskApi
    Scenario Outline: Clinical Review Tasks are returned when there are Clinical Review Tasks for the role
        Given I have Clinical Review Tasks '<clinicalReviewTasks>' in Mongo
        When I send a request to get Clinical Review Tasks with parameter <name> and <value>
        Then I can see correct Clinical Review Tasks are returned

        Examples:
          | clinicalReviewTasks     | name  | value           |
          | ClinicalReviewTask.json | roles | clinician       |
          | ClinicalReviewTask.json | roles | clinician,other |

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
          | clinicalReviewTasks                                    | name            | value  |
          | ClinicalReviewTask.json,ClinicalReviewTask_Search.json | patientName     | George |
          | ClinicalReviewTask.json,ClinicalReviewTask_Search.json | patientId       | 999999 |
          | ClinicalReviewTask.json,ClinicalReviewTask_Search.json | applicationName | stroke |

    @ClinicalReview_TaskApi
    Scenario: Clinical Review service returns bad request when roles are not added
        Given I have Clinical Review Tasks 'ClinicalReviewTask.json' in Mongo
        When I send a request to get Clinical Review Tasks with no role
        Then Clinical Review Service Returns Bad request

    @ClinicalReview_TaskApi
    Scenario: Clinical Review task can be approved and request generates a Task Callback
        Given I have Clinical Review Tasks 'ClinicalReviewTask.json' in Mongo
        When I send a request to edit clinical review task with 'ClinicalReviewTask_Accept.json' and execution Id '8facc52c-8b43-45ae-8399-8681c719ec2c'
        Then clinical review task has been updated in Mongo '8facc52c-8b43-45ae-8399-8681c719ec2c'
        And I can see a Task Callback is generated

    @ClinicalReview_TaskApi @ignore
    Scenario: Clinical Review task can be rejected and request generates a Task Callback
        Given I have Clinical Review Tasks 'ClinicalReviewTask.json' in Mongo
        When I send a request to edit clinical review task with 'ClinicalReviewTask_Reject.json' and execution Id '8facc52c-8b43-45ae-8399-8681c719ec2c'
        Then clinical review task has been updated in Mongo '8facc52c-8b43-45ae-8399-8681c719ec2c'
        And I can see a Task Callback is generated

    @ClinicalReview_TaskApi @ignore
    Scenario: Clinical Review task cannot be approved when execution Id is invalid 
        Given I have Clinical Review Tasks 'ClinicalReviewTask.json' in Mongo
        When I send a request to edit clinical review task with 'ClinicalReviewTask_Reject.json' and execution Id '8facc52c-8b43-45ae-8399-8681c719ec2c'
        Then clinical review task has been updated in Mongo '8facc52c-8b43-45ae-8399-8681c719ec2c'
        Then Clinical Review Service Returns Bad request
       
    @ClinicalReview_TaskApi @ignore
    Scenario: Clinical Review task cannot be approved when request body is invalid
        Given I have Clinical Review Tasks 'ClinicalReviewTask.json' in Mongo
        When I send a request to edit clinical review task with 'ClinicalReviewTask_Reject.json' and execution Id '8facc52c-8b43-45ae-8399-8681c719ec2c'
        Then clinical review task has been updated in Mongo '8facc52c-8b43-45ae-8399-8681c719ec2c'
        Then Clinical Review Service Returns Bad request

    @ClinicalReview_TaskApi @ignore
    Scenario: Clinical Review task cannot be rejected when reject reason is missing
        Given I have Clinical Review Tasks 'ClinicalReviewTask.json' in Mongo
        When I send a request to edit clinical review task with 'ClinicalReviewTask_Reject.json' and execution Id '8facc52c-8b43-45ae-8399-8681c719ec2c'
        Then Clinical Review Service Returns Bad request

    @ClinicalReview_TaskApi @ignore
    Scenario: Clinical Review task cannot be approved or rejected when review is not found
        Given I have Clinical Review Tasks 'ClinicalReviewTask.json' in Mongo
        When I accept the Clinical Review Task ClinicalReviewTask.json
        Then Clinical Review service Returns Not found

    @ClinicalReview_TaskApi @ignore
    Scenario: Clinical Review task cannot be approved or rejected when already reviewed in the DB
        Given I have no Clinical Review Tasks in Mongo
        When I accept the Clinical Review Task ClinicalReviewTask.json
        Then I can see no Clinical Review Tasks are returned

    @ClinicalReview_TaskApi @ignore
    Scenario: Clinical Review task cannot be approved when role does not match
        Given I have Clinical Review Tasks 'ClinicalReviewTask.json' in Mongo



