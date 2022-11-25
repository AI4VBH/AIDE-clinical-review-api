# Copyright 2022 Crown Copyright
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

Feature: ClinicalReviewEvent

    @ClinicalReviewEvent
    @ignore
    Scenario: Publish a clinical review event and see the event is saved in Mongo
        When I publish a Clinical Review Event name
        Then I can see Clinical Review Task is saved in Mongo
        And I can see Clinical Review Study is saved in Mongo