# DatingApp – ASP.NET Core & Angular Full-Stack Web Application

A modern full-stack **Dating Application** built from scratch using **ASP.NET Core 9.0** for the backend and **Angular 20** for the frontend.  
The app features secure authentication, real-time messaging, photo uploads, and a responsive, attractive UI.  
Deployed to **Azure**, this project demonstrates the complete lifecycle of building and deploying a full-stack application.

---

## 🚀 Features

- **User Authentication** – Register, login, JWT-based authentication & role-based access control
- **User Profiles** – Create and manage user profiles with photo gallery
- **Photo Upload** – Image upload with cropping and preview
- **Real-Time Messaging** – Private chat with online presence indicators via SignalR
- **Search & Filters** – Paging, sorting, and filtering of profiles
- **Responsive UI** – Built with Tailwind CSS 4 & DaisyUI for a modern look
- **Routing & Guards** – Angular routing with secured routes
- **Form Handling** – Angular Template Forms & Reactive Forms with validation
- **Deployment** – Published to Azure with CI/CD-ready configuration

---

## 🛠️ Tech Stack

**Backend**  
- ASP.NET Core 9.0 (Web API)
- Entity Framework Core
- SignalR (real-time communication)
- AutoMapper
- JWT Authentication

**Frontend**  
- Angular 20
- Tailwind CSS 4 + DaisyUI
- RxJS & Angular Forms
- ngx-gallery (photo gallery component)

**Database**  
- SQL Server (Local or Azure SQL)

**Other Tools**  
- Visual Studio Code
- DotNet CLI & Angular CLI
- Azure App Service (deployment)
- GitHub Actions (optional for CI/CD)

---

## 📦 Installation & Setup

### Prerequisites
- [Node.js](https://nodejs.org/) (LTS)
- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download)
- [Angular CLI](https://angular.io/cli)
- SQL Server (local or cloud)
- Git

### Backend Setup
```bash
cd API
dotnet restore
dotnet ef database update
dotnet run
