# Application Deployment Overview

This application is deployed to **AWS** across a highly available and secure infrastructure.

## Infrastructure Details

- **EC2 Instances**: The application is hosted on two EC2 instances, each deployed in a different AWS Availability Zone for high availability and fault tolerance.
  - The app is a **Dockerized** .NET and React app, with the Docker image hosted on Docker Hub.
  - EC2 instances are deployed in a **public subnet** but do not allow direct public access. Traffic is routed exclusively through the Application Load Balancer (ALB), ensuring that the instances remain secure.

- **PostgreSQL Database**: The PostgreSQL database is deployed in a **private subnet**, which ensures isolation from the public internet and secures data within the VPC.

- **Application Load Balancer (ALB)**: 
  - The application is accessed via an **Application Load Balancer**, which distributes traffic across the two EC2 instances in separate availability zones.
  - The ALB enforces secure communication using **HTTPS**, with SSL certificates managed by **AWS Certificate Manager (ACM)**.
  - The ALB also performs health checks to ensure instances are available and can serve traffic.

## Networking and Security

- **VPC Setup**: The application runs in a custom **VPC** with a public subnet for EC2 instances and a private subnet for the database. Security groups are configured to control traffic to and from these resources.
- **Security Groups**: EC2 instances are protected by security groups, allowing only traffic from the ALB and blocking all other public traffic.
- **IAM Roles**: Appropriate IAM roles and policies are assigned to ensure secure access to AWS resources.

## Docker and CI/CD

- The Dockerized app is built and hosted on **Docker Hub**.
- The image is pulled from Docker Hub and deployed to both EC2 instances.

## Key Features

- **High Availability**: Deployed across two Availability Zones for resilience and minimal downtime.
- **Security**: 
  - EC2 instances are secured behind the Application Load Balancer.
  - The PostgreSQL database is isolated in a private subnet.
  - All traffic is routed through HTTPS, secured by SSL.
- **Scalability**: The infrastructure can easily scale by adding more EC2 instances to the load balancer.

