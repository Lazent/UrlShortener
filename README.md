# 🔗 URL Shortener

Full-stack URL shortening application with user authentication and role-based access control.

---

## 🛠 Tech Stack

**Backend:** ASP.NET Core MVC, Entity Framework Core, MSSQL  
**Frontend:** Angular 19, Tailwind CSS, TypeScript

---

## 🚀 Quick Start

### Backend

```bash
cd UrlShortener
dotnet restore
dotnet ef database update
dotnet run
```

The API runs on:  
https://localhost:7277

---

### Frontend

```bash
cd UrlShortener.Client
npm install
ng serve
```

The application runs on:  
http://localhost:4200

---

## 🔑 Default Users

| Login | Password | Role |
|-------|----------|--------|
| admin | admin123 | Admin |
| user  | user123  | User  |

---

## ✨ Features

- Authentication: Login/Register functionality with Admin and User roles.
- URL Shortening: Convert long URLs into manageable short links.
- CRUD Operations: Create, view, and delete shortened URLs.
- Access Control:
  - Anonymous: View the list of URLs only.
  - Users: Create and delete their own URLs.
  - Admins: Full access to manage all URLs in the system.
- Copy to Clipboard: One-click copy functionality for short URLs.
- URL Details: Detailed information view for each URL entry.

---

## 📡 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/account/login | User Login |
| POST | /api/account/register | User Registration |
| GET | /api/urls | Get all URLs |
| GET | /api/urls/{id} | Get specific URL by ID |
| POST | /api/urls | Create a new short URL |
| DELETE | /api/urls/{id} | Delete a URL |
| GET | /{shortCode} | Redirect to the original URL |

---

## 📂 Project Structure

```text
UrlShortener/
├── UrlShortener/           # Backend API Project
│   ├── Controllers/Api/    # REST Controllers
│   ├── Data/               # DBContext & Seeding
│   ├── Models/             # Database Entities
│   └── Migrations/         # EF Core Migrations
└── UrlShortener.Client/    # Angular Frontend Project
    └── src/app/
        ├── components/     # UI Components
        ├── services/       # API Services
        ├── guards/         # Route Guards (Auth/Admin)
        └── models/         # TypeScript Interfaces
```

---

## 📊 Database Schema

Users:  
- Id  
- Login  
- PasswordHash  
- IsAdmin  

ShortUrls:  
- Id  
- OriginalUrl  
- ShortCode  
- CreatedAt  
- CreatedById  

AboutContents:  
- Id  
- Content  
- LastUpdated  

---

## ⚙️ Development & Configuration

- CORS: Pre-configured for communication with http://localhost:4200.
- Authentication: We use simple session-based authentication stored in localStorage.
- Seeding: The database is automatically seeded with test data upon the first migration/run.
- Angular: Built using modern Standalone Components.
- Design: Responsive UI implemented with Tailwind CSS.
