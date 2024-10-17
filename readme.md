# Application Deployment Overview

This application is deployed to **AWS** across a highly available and secure infrastructure.

## Infrastructure Details

- **EC2 Instances**: The application is hosted on two EC2 instances, each deployed in a different AWS Availability Zone for high availability and fault tolerance.

  - The app is a **Dockerized** .NET and React app, with the Docker image hosted on Docker Hub.
  - The React app is built using **Vite** during the CI/CD pipeline, and the build artifacts (static files) are copied into the **wwwroot** folder of the .NET API, allowing the API to serve the frontend.
  - EC2 instances are deployed in a **public subnet** but do not allow direct public access. Traffic is routed exclusively through the Application Load Balancer (ALB) via https.

- **PostgreSQL Database**: The PostgreSQL database is deployed in a **private subnet**, which ensures isolation from the public internet and secures data within the VPC.

- **Application Load Balancer (ALB)**:
  - The application is accessed via an **Application Load Balancer**, which distributes traffic across the two EC2 instances in separate availability zones.
  - The ALB enforces secure communication using **HTTPS**, with SSL certificates managed by **AWS Certificate Manager (ACM)**.
  - The ALB also performs health checks to ensure instances are available and can serve traffic.

## Networking and Security

- **VPC Setup**: The application runs in a custom **VPC** with a public subnet for EC2 instances and a private subnet for the database. Security groups are configured to control traffic to and from these resources.
- **Security Groups**: EC2 instances are protected by security groups, allowing only traffic from the ALB and blocking all other public traffic.
- **IAM Roles**: Appropriate IAM roles and policies are assigned to ensure secure access to AWS resources.
- **Least Privilege Access**: IAM policies follow the principle of **least privilege**, granting only the permissions necessary for each component to function properly. For example, the EC2 instance role includes permissions to interact with **SSM** for deployment automation.

## Docker and CI/CD

- The Dockerized app is built and hosted on **Docker Hub**.
- The **React frontend** is built using **Vite** during the CI/CD pipeline and the generated static files are placed in the **wwwroot** directory of the .NET API.
- A **CI/CD pipeline** is configured using GitHub Actions to automate the build and deployment process:
  - The **Docker image** is automatically built and pushed to Docker Hub on commits to the main branch.
  - The deployment to EC2 instances is triggered via **AWS CLI** using **AWS Systems Manager (SSM)** to run a shell script that pulls the latest Docker image and redeploys the application.
  - The GitHub Actions workflow is configured with **least privilege IAM credentials** to ensure security, granting only the permissions necessary to deploy the app.

## Automatic Deployment to EC2

- **Deployment Script**: The deployment to EC2 instances is automated via a shell script that is executed using **AWS Systems Manager (SSM)**. The script performs the following steps:

  1. **Pull the latest Docker image** from Docker Hub.
  2. **Stop the existing container** running the application.
  3. **Run the new container** with the updated image.

- **AWS Systems Manager (SSM)**: SSM is used to connect to the EC2 instances and run commands without requiring direct SSH access, enhancing security.
  - The GitHub Actions workflow uses **AWS CLI** to send commands to the instances via **SSM**. This approach ensures that deployment is automated and secure, without exposing SSH ports or using SSH keys.
  - The IAM user and roles involved in the deployment have been configured with **least privilege** to limit the permissions to only what is necessary for deployment, minimizing security risks.

## Key Features

- **High Availability**: Deployed across two Availability Zones for resilience and minimal downtime.
- **Security**:
  - The PostgreSQL database is isolated in a private subnet.
  - All traffic is routed through HTTPS, secured by SSL.
  - Deployment automation via **SSM** avoids direct SSH access, reducing potential attack vectors.
- **Scalability**: The infrastructure can easily scale by adding more EC2 instances to the load balancer.
