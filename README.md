# 📝 Blog API

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-512BD4)](https://docs.microsoft.com/en-us/aspnet/core/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

> Production-ready RESTful Blog API built with ASP.NET Core 9, featuring JWT authentication, role-based authorization, comprehensive testing, and clean architecture.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Authentication](#authentication)
- [Testing](#testing)
- [Project Structure](#project-structure)
- [License](#license)
- [Author](#author)

---

## 🎯 Overview

A fully-featured **Blog API** designed for modern web applications. Built with enterprise-level patterns including JWT authentication, role-based access control, unit and integration testing, and comprehensive API documentation via Swagger.

**Perfect for:** Learning modern ASP.NET Core API development, building blog platforms, or as a foundation for content management systems.

---

## ✨ Features

### 🔐 Authentication & Security
- **JWT Authentication** - Secure token-based authentication
- **Role-Based Authorization** - Three-tier access control (Admin, Author, Reader)
- **BCrypt Password Hashing** - Secure password storage
- **User Secrets** - Safe configuration management
- **Input Validation** - Comprehensive request validation

### 📚 Blog System
- **Posts Management** - Create, read, update, delete blog posts
- **Comments System** - Nested comments with full CRUD
- **Likes/Reactions** - User engagement tracking
- **Categories** - Organize posts by category
- **Tags** - Flexible post tagging system
- **Author Profiles** - User information and post history

### ⚡ Performance & Quality
- **Pagination** - Efficient data retrieval
- **Filtering & Sorting** - Flexible query options
- **EF Core Optimization** - Query performance tuning
- **DTOs** - Clean separation between models and API responses
- **AutoMapper** - Automated object mapping

### 🧪 Testing
- **Unit Tests** - Service layer and business logic testing
- **Integration Tests** - Full API endpoint testing with WebApplicationFactory
- **Test Coverage** - Comprehensive test suite

### 📖 Documentation
- **Swagger/OpenAPI** - Interactive API documentation
- **XML Comments** - Detailed endpoint descriptions
- **Example Requests** - Sample payloads for testing

---

## 🛠️ Tech Stack

### Backend
| Technology | Version | Purpose |
|------------|---------|---------|
| [.NET](https://dotnet.microsoft.com/) | 9.0 | Application framework |
| [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/) | 9.0 | RESTful API framework |
| [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) | 9.0 | ORM & data access |
| [SQL Server](https://www.microsoft.com/sql-server) | 2019+ | Database |
| [JWT Bearer](https://jwt.io/) | - | Authentication tokens |
| [BCrypt.Net](https://github.com/BcryptNet/bcrypt.net) | - | Password hashing |
| [AutoMapper](https://automapper.org/) | 12.0+ | Object mapping |

### Testing
| Technology | Purpose |
|------------|---------|
| [xUnit](https://xunit.net/) | Test framework |
| [Moq](https://github.com/moq/moq4) | Mocking library |
| [FluentAssertions](https://fluentassertions.com/) | Assertion library |
| [WebApplicationFactory](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests) | Integration testing |

### Tools
- **Swagger/OpenAPI** - API documentation
- **Visual Studio 2022** / VS Code / Rider
- **SQL Server Management Studio (SSMS)**
- **Postman** - API testing
- **Git** - Version control

---

## 🏗️ Architecture

This API follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────────┐
│              BlogAPI                        │
│         (Controllers, DTOs)                 │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│           Services Layer                    │
│    (Business Logic, Validation)             │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│          Data Access Layer                  │
│   (DbContext, Repositories, Models)         │
└─────────────────────────────────────────────┘
```

### Design Patterns
- ✅ **Repository Pattern** - Data access abstraction
- ✅ **Service Layer Pattern** - Business logic separation
- ✅ **DTO Pattern** - API request/response objects
- ✅ **Dependency Injection** - Loose coupling
- ✅ **Unit of Work Pattern** - Transaction management

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server 2019+](https://www.microsoft.com/sql-server) (Express or LocalDB is fine)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) / [VS Code](https://code.visualstudio.com/) / [Rider](https://www.jetbrains.com/rider/)
- [Git](https://git-scm.com/)
- [Postman](https://www.postman.com/) (optional, for API testing)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/hanagouda/Blog.git
   cd Blog
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Update database connection string**
   
   Configure User Secrets for the BlogAPI project:
   ```bash
   cd BlogAPI
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER;Database=BlogDB;Trusted_Connection=True;TrustServerCertificate=True;"
   ```
   
   > 💡 **Tip:** For SQL Server Express, use `Server=localhost\\SQLEXPRESS`

4. **Configure JWT settings**
   ```bash
   dotnet user-secrets set "Jwt:Key" "YourSuperSecretKeyHere_AtLeast32Characters!"
   dotnet user-secrets set "Jwt:Issuer" "BlogAPI"
   dotnet user-secrets set "Jwt:Audience" "BlogAPIUsers"
   ```

5. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

6. **Run the API**
   ```bash
   dotnet run --project BlogAPI
   ```

7. **Access Swagger documentation**
   
   Navigate to: `https://localhost:5001/swagger`

---

## 📚 API Endpoints

### Authentication
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register new user | No |
| POST | `/api/auth/login` | Login and get JWT token | No |

### Posts
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/posts` | Get all posts (paginated) | No |
| GET | `/api/posts/{id}` | Get post by ID | No |
| POST | `/api/posts` | Create new post | Yes (Author/Admin) |
| PUT | `/api/posts/{id}` | Update post | Yes (Author/Admin) |
| DELETE | `/api/posts/{id}` | Delete post | Yes (Author/Admin) |

### Comments
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/posts/{postId}/comments` | Get post comments | No |
| POST | `/api/posts/{postId}/comments` | Add comment | Yes |
| PUT | `/api/comments/{id}` | Update comment | Yes (Owner) |
| DELETE | `/api/comments/{id}` | Delete comment | Yes (Owner/Admin) |

### Likes
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/posts/{postId}/like` | Like/unlike post | Yes |
| GET | `/api/posts/{postId}/likes` | Get post likes | No |

### Categories
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/categories` | Get all categories | No |
| POST | `/api/categories` | Create category | Yes (Admin) |
| PUT | `/api/categories/{id}` | Update category | Yes (Admin) |
| DELETE | `/api/categories/{id}` | Delete category | Yes (Admin) |

### Tags
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/tags` | Get all tags | No |
| POST | `/api/tags` | Create tag | Yes (Author/Admin) |

---

## 🔐 Authentication

### Register a New User

**Request:**
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "SecurePassword123!",
  "role": "Author"
}
```

**Response:**
```json
{
  "success": true,
  "message": "User registered successfully"
}
```

### Login

**Request:**
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2025-12-31T23:59:59Z",
  "username": "johndoe",
  "role": "Author"
}
```

### Using the JWT Token

Include the token in the Authorization header:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Roles & Permissions

| Role | Permissions |
|------|-------------|
| **Reader** | View posts, comments, categories, tags |
| **Author** | All Reader permissions + Create/Edit own posts, Create tags, Add comments |
| **Admin** | All permissions + Delete any content, Manage categories, Manage users |

---

## 🧪 Testing

### Run Unit Tests

```bash
dotnet test BlogUnitTests
```

### Run Integration Tests

```bash
dotnet test BlogIntegrationTests
```

### Run All Tests with Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Test Structure

**Unit Tests:**
- Service layer business logic
- Validation rules
- Authorization checks
- DTOs mapping

**Integration Tests:**
- Full API endpoint workflows
- Database operations
- Authentication flows
- Error handling

---

## 📁 Project Structure

```
Blog/
│
├── BlogAPI/                          # Main API Project
│   ├── Controllers/                  # API Controllers
│   │   ├── AuthController.cs
│   │   ├── PostsController.cs
│   │   ├── CommentsController.cs
│   │   ├── CategoriesController.cs
│   │   └── TagsController.cs
│   ├── Services/                     # Business Logic
│   │   ├── IAuthService.cs
│   │   ├── AuthService.cs
│   │   ├── IPostService.cs
│   │   ├── PostService.cs
│   │   └── ...
│   ├── Data/                         # Database Context
│   │   ├── ApplicationDbContext.cs
│   │   └── Migrations/
│   ├── Models/                       # Domain Entities
│   │   ├── User.cs
│   │   ├── Post.cs
│   │   ├── Comment.cs
│   │   ├── Category.cs
│   │   └── Tag.cs
│   ├── DTOs/                         # Data Transfer Objects
│   │   ├── UserDto.cs
│   │   ├── PostDto.cs
│   │   ├── CommentDto.cs
│   │   └── ...
│   ├── Helpers/                      # Utility Classes
│   │   ├── AutoMapperProfiles.cs
│   │   └── PaginationHelper.cs
│   └── Program.cs                    # Application Entry Point
│
├── BlogUnitTests/                    # Unit Test Project
│   ├── Services/
│   │   ├── AuthServiceTests.cs
│   │   ├── PostServiceTests.cs
│   │   └── ...
│   └── Controllers/
│       └── ...
│
├── BlogIntegrationTests/             # Integration Test Project
│   ├── Controllers/
│   │   ├── AuthControllerTests.cs
│   │   ├── PostsControllerTests.cs
│   │   └── ...
│   └── TestFixtures/
│       └── CustomWebApplicationFactory.cs
│
├── .gitignore
├── LICENSE
├── README.md
└── Blog.sln
```

---

## 🔒 Security Best Practices

- ✅ **Password Hashing** - BCrypt with salt
- ✅ **JWT Tokens** - Secure token-based authentication
- ✅ **User Secrets** - Sensitive data not in source control
- ✅ **Input Validation** - Data annotations and FluentValidation
- ✅ **SQL Injection Prevention** - Parameterized queries via EF Core
- ✅ **HTTPS** - Enforced in production
- ✅ **CORS** - Configured for specific origins
- ✅ **Rate Limiting** - API throttling (recommended for production)

---

## 📖 API Documentation

Interactive API documentation is available via Swagger:

**Development:** `https://localhost:5001/swagger`

**Features:**
- Try out endpoints directly from the browser
- View request/response models
- Authentication support
- Example payloads

---

## 🚀 Deployment

### Azure App Service

```bash
# Publish
dotnet publish -c Release -o ./publish

# Deploy via Azure CLI
az webapp up --name your-blog-api --resource-group your-rg
```

### Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "BlogAPI.dll"]
```

### Production Checklist

- ✅ Update connection strings
- ✅ Set JWT secret key (strong, random)
- ✅ Configure CORS for your domain
- ✅ Enable HTTPS
- ✅ Set up logging (Serilog/Application Insights)
- ✅ Configure rate limiting
- ✅ Set up database backups
- ✅ Enable health checks

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Hana Gouda**

- GitHub: [@hanagouda](https://github.com/hanagouda)
- LinkedIn: [Hana Gouda](https://linkedin.com/in/hana-gouda)
- Email: hhanagouda@gmail.com

---

## 🙏 Acknowledgments

- Built as a demonstration of modern ASP.NET Core Web API development
- Implements industry best practices for authentication and testing
- Showcases clean architecture and SOLID principles

---

## 🎯 Learning Outcomes

This project demonstrates proficiency in:
- ✅ ASP.NET Core Web API development
- ✅ JWT authentication & authorization
- ✅ Entity Framework Core & SQL Server
- ✅ RESTful API design
- ✅ Unit & Integration testing
- ✅ Clean Architecture patterns
- ✅ Swagger/OpenAPI documentation
- ✅ Secure coding practices

---

<div align="center">

**⭐ If you find this project helpful, please give it a star!**

Made with ❤️ by Hana Gouda

</div>