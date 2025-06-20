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

Once running, navigate to: http://localhost:4200

---

## 🧪 Testing

### Run Angular Unit Tests

ng test

### Run Angular End-to-End Tests

ng e2e

> Angular CLI does not include an e2e framework by default. You can integrate Cypress, Playwright, or Protractor.

---

## 📦 Building for Production

To build the frontend:

ng build

The output will be in dist/, which can be served statically or integrated into the backend.

To publish the backend:

cd ServerApp
dotnet publish -c Release

---

## 📚 Useful Commands

### Generate Angular Component

ng generate component my-component

To see all available schematics:

ng generate --help

---

## 📝 Notes

- SignalR is enabled in the backend for real-time updates.
- This architecture is suitable for development or deployment as a unified monolith.
- You can separate ClientApp and ServerApp into independent services if needed in the future.

---

## 📄 License

MIT License
