# Blog

A production-ready RESTful Blog API built with ASP.NET Core 9, featuring JWT authentication, role-based authorization, and comprehensive testing.

## 🚀 Features

- **Authentication & Authorization**: JWT-based auth with role management (Admin, Author, Reader)
- **Complete Blog System**: Posts, Comments, Likes, Categories, Tags
- **Security**: BCrypt password hashing, User Secrets, input validation
- **Performance**: Pagination, filtering, sorting with EF Core optimization
- **Testing**: Unit tests + Integration tests with WebApplicationFactory
- **Clean Architecture**: Controllers → Services → Data layers with DTOs
- **API Documentation**: Swagger/OpenAPI integration

## 📋 Tech Stack

| Technology | Purpose |
|-----------|---------|
| ASP.NET Core 9 | Web API framework |
| Entity Framework Core 9 | ORM for database access |
| SQL Server | Database |
| JWT | Authentication tokens |
| BCrypt | Password hashing |
| AutoMapper | Object mapping |
| xUnit | Unit testing |
| Swagger | API documentation |

## 🛠️ Getting Started

### Prerequisites

Before you begin, ensure you have installed:
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (Express or LocalDB is fine)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation Steps

#### 1. Clone the repository
```bash
git clone [https://github.com/hhanagouda/Blog.git](https://github.com/hhanagouda/Blog.git)
cd Blog