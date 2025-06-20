# Monolithic Application: Angular + ASP.NET Core (.NET 8)

This repository contains a fullstack monolithic application composed of:

- **ClientApp**: Angular 17 (standalone application)
- **ServerApp**: ASP.NET Core 8 Web API with PostgreSQL and SignalR

---

## 🔧 Project Structure

myApp/
├── ClientApp/     # Frontend Angular application
└── ServerApp/     # Backend .NET Web API application

---

## 🚀 Getting Started

### Prerequisites

Make sure you have the following installed:

- .NET 8 SDK: https://dotnet.microsoft.com/download
- Node.js & npm: https://nodejs.org/
- Angular CLI v20+: https://angular.dev/tools/cli
- PostgreSQL: https://www.postgresql.org/
- Visual Studio or VS Code: https://code.visualstudio.com/

---

## 🖥️ Running the Application Locally

### 1. Run the Backend (ServerApp)

cd ServerApp
dotnet ef database update  # optional, if using EF migrations
dotnet run

By default, the API will be available at: https://localhost:5001 or http://localhost:5000

> Make sure your appsettings.Development.json has the correct PostgreSQL connection string.

---

### 2. Run the Frontend (ClientApp)

cd ClientApp
npm install
ng serve

Once the server is running, open your browser and navigate to http://localhost:4200/. The application will automatically reload whenever you modify any of the source files.

---

## 📦 Building the Application

### Build Angular Frontend

ng build

This will compile your project and store the build artifacts in the dist/ directory. By default, the production build optimizes your application for performance and speed.

### Publish ASP.NET Backend

cd ServerApp
dotnet publish -c Release

---

## 🧪 Testing

### Run Angular Unit Tests

ng test

Runs unit tests via Karma (https://karma-runner.github.io).

### Run Angular End-to-End Tests

ng e2e

> Angular CLI does not include an e2e framework by default. You can integrate Cypress, Playwright, or Protractor.

---

## 🧱 Angular CLI Commands Reference

### Code Scaffolding

To generate a new component, run:

ng generate component component-name

For a complete list of available schematics (such as components, directives, or pipes), run:

ng generate --help

---

## 📚 Additional Resources

- Angular CLI Documentation: https://angular.dev/tools/cli
- ASP.NET Core Docs: https://learn.microsoft.com/aspnet/core/
- PostgreSQL Docs: https://www.postgresql.org/docs/
- SignalR Documentation: https://learn.microsoft.com/aspnet/core/signalr/introduction

---

## 📝 Notes

- SignalR is enabled in the backend for real-time request updates.
- This structure works well for local development or deployment as a full monolithic app.
- If needed, ClientApp and ServerApp can later be split into separate deployable services.

---

## 📄 License

MIT License
