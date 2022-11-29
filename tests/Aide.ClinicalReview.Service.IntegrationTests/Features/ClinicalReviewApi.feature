﻿# Copyright 2022 Guy’s and St Thomas’ NHS Foundation Trust
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

Feature: ClinicalReviewApi

    @ClinicalReviewApi
    @ignore
    Scenario: Accepted Clinical Review request generates a Task Callback
        When I accept the Clinical Review Task ClinicalReviewTask.json
        Then I can see Clinical Review Task is updated
        And I can see a Task Callback is generated