# API_ApplicationRequest_SignalR
Application Request Management System with .NET Core, PostgreSQL, and Angular
This project is a full-stack web application built with .NET 8, Angular, and PostgreSQL, designed to manage requests (requests, offers, complaints) in real time. Users can authenticate, submit new requests, and view a list of all submitted requests, which are automatically updated to "completed" by a background process.

🔑 Key Features:
🔐 JWT Authentication (in progress)

📝 Request creation and retrieval using MediatR and CQRS pattern

📄 Relational catalog for request types

⏱️ Background service that automatically updates request status to "completed" after 1 minute

📡 Real-time updates using SignalR

🧱 Clean architecture based on DDD and layered separation:

Domain: Entities and contracts

Application: Commands, queries, and handlers

Infrastructure: Services like SignalR and HostedService

Persistence: EF Core and data access

API: Presentation layer with controllers

🧪 Tech Stack:
.NET 8 + Web API

Entity Framework Core + PostgreSQL

MediatR

SignalR

Angular (coming soon)

JWT (in development)

📘 Changelog:
Implemented request registration (ApplicationRequest) using MediatR and EF Core

Added RequestType catalog with foreign key relationship

Configured a background HostedService that updates request status to completed after 1 minute

Integrated SignalR for notifying clients in real-time upon request updates

Added DTOs and mappings for request queries

Structured the project by layers: Domain, Application, Infrastructure, Persistence, and API

Prepared backend for JWT authentication integration