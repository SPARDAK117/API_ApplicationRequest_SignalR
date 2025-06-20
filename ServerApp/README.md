API_ApplicationRequest_SignalR
Application Request Management System with .NET Core, PostgreSQL, and Angular
This project is a full-stack web application built with .NET 8, Angular, and PostgreSQL, designed to manage requests (requests, offers, complaints) in real time. Users can authenticate, submit new requests, and view a list of all submitted requests, which are automatically updated to "completed" by a background process.

🔑 Key Features:

JWT Authentication (in progress)

Request creation and retrieval using MediatR and CQRS pattern

Relational catalog for request types

Background service that updates request status to "completed" after 1 minute

Real-time updates using SignalR

Clean architecture based on DDD:

/Domain → Entities and contracts

/Application → Commands, queries, handlers

/Infrastructure → Services like SignalR and background jobs

/Persistence → EF Core & PostgreSQL access

/API → Web API controllers

/ClientApp → Angular frontend (standalone)

🧪 Tech Stack:

.NET 8 (ASP.NET Core Web API)

Angular 17 (standalone components)

PostgreSQL + Entity Framework Core

MediatR

SignalR

JWT Authentication (coming soon)

⚙️ Prerequisites:

.NET 8 SDK

Node.js + NPM

PostgreSQL 14+

Angular CLI

(Optional) DBeaver or pgAdmin for managing PostgreSQL

🛠️ Initialize PostgreSQL Database:
Run the following command to create all tables and populate the catalog:

psql -U postgres -d ApplicationRequestsDb -f ./Database/init_db.sql

Replace postgres with your PostgreSQL user if needed.
Alternatively, import the script via DBeaver or pgAdmin.

🧭 How to Run the App

🔧 Backend (.NET)

Go to the root folder:
cd myApp

Restore and run the API:
dotnet restore
dotnet run --project ServerApp

The API should run on https://localhost:5001 or the configured port.

🌐 Frontend (Angular)

Navigate to the Angular app:
cd ClientApp

Install and start:
npm install
ng serve

Visit http://localhost:4200 to view the app.

🔁 Real-Time Communication:
The app uses SignalR to push updates to connected clients when a request is automatically updated from "submitted" to "completed". A background process runs every 10 seconds to check and update eligible requests.

📘 Changelog:

Request registration using MediatR and EF Core

RequestType catalog with FK relationship

Background HostedService for automatic status updates

Real-time SignalR integration

DTOs and mappings for clean data contracts

Layered architecture using DDD principles

JWT authentication groundwork in place

✅ Clone the repo, follow the steps, and you're ready to test. Feedback and contributions are welcome.