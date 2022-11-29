<!--
  ~ Copyright 2022 Guy’s and St Thomas’ NHS Foundation Trust
  ~
  ~ Licensed under the Apache License, Version 2.0 (the "License");
  ~ you may not use this file except in compliance with the License.
  ~ You may obtain a copy of the License at
  ~
  ~ http://www.apache.org/licenses/LICENSE-2.0
  ~
  ~ Unless required by applicable law or agreed to in writing, software
  ~ distributed under the License is distributed on an "AS IS" BASIS,
  ~ WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  ~ See the License for the specific language governing permissions and
  ~ limitations under the License.
-->

<a name="readme-top"></a>

<div align="center">

[![Build/Test](https://github.com/AI4VBH/AIDE-clinical-review-api/actions/workflows/test.yml/badge.svg)](https://github.com/AI4VBH/AIDE-clinical-review-api/actions/workflows/test.yml)
[![Security Scanning](https://github.com/AI4VBH/AIDE-clinical-review-api/actions/workflows/security.yml/badge.svg)](https://github.com/AI4VBH/AIDE-clinical-review-api/actions/workflows/security.yml)

</div>

<br />

<div align="center">
  <a href="https://github.com/AI4VBH/AIDE-clinical-review-api">
    <img src="aide-logo.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">AIDE Clinical Review API</h3>

  <p align="center">
    The AIDE Clinical Review API is responsible for receiving tasks from the <a href="https://github.com/Project-MONAI/monai-deploy-workflow-manager" target="_blank">MONAI Deploy Task Manager</a> and preparing them for review by clinicians. The Clinical Review API is a back-end service which receives requests from the <a href="https://github.com/AI4VBH/AIDE-API" target="_blank">AIDE API</a>.
    <br />
    <br />
    <a href="https://github.com/AI4VBH/AIDE-clinical-review-api/issues">Report Bug</a>
    ·
    <a href="https://github.com/AI4VBH/AIDE-clinical-review-api/issues">Request Feature</a>
  </p>
</div>

## Dependencies

Start by cloning or creating a fork of this repository. See GitHub's documentation for help with this: https://docs.github.com/en/get-started/quickstart/fork-a-repo

Secondly ensure that you download and install the latest LTS release of [.NET](https://dotnet.microsoft.com/en-us/download).

The Clinical Review API requires MongoDB, RabbitMQ and MinIO to be installed to run.

### MongoDB

To install MongoDB Community Edition on your local machine, follow the documentation here: https://www.mongodb.com/docs/manual/administration/install-community/

Next, refer to the defaults in `src/Aide.ClinicalReview.Service/appsettings.Development.json`, which detail the expected database name that you will need to create. Installing [MongoDB Compass](https://www.mongodb.com/docs/compass/current/install/) to create the database is recommended.

### RabbitMQ

RabbitMQ in easiest to run within Docker for local development. First ensure that you have <a href="https://docs.docker.com/get-docker/" target="_blank">installed Docker</a>, then execute the following command:

```bash
$ docker run -d --hostname rabbitmq --name rabbitmq -e RABBITMQ_DEFAULT_USER=admin -e RABBITMQ_DEFAULT_PASS=admin -e RABBITMQ_DEFAULT_VHOST=monaideploy rabbitmq:3-management
```

### MinIO

MinIO is easiest to run within Docker for local development. First ensure that you have <a href="https://docs.docker.com/get-docker/" target="_blank">installed Docker</a>, then follow the documentation on running a [Single-Node Single Drive](https://min.io/docs/minio/container/operations/install-deploy-manage/deploy-minio-single-node-single-drive.html#minio-snsd) installation within Docker.

Next, refer to the defaults in `src/Aide.ClinicalReview.Service/appsettings.Development.json`, which detail the expected accessKey and accessToken that should be configured during installation.

## Getting started

Assuming you have followed the steps in [Dependencies](#dependencies), you should be able to run the API within your IDE of choice or via running the .Net CLI within the `src/Aide.ClinicalReview.Service` folder:

```bash
$ dotnet run
```

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<div align="right">(<a href="#readme-top">back to top</a>)</div>

<!-- LICENSE -->
## License

AIDE is Apache 2.0 licensed. Please review the [LICENCE](LICENCE) for details on how the code can be used.