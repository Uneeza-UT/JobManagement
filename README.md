# Job/Internship Management System API

A RESTful ASP.NET Core Web API for managing job and internship postings, companies, students, applications, and company join requests. The API includes role-based authorization, JWT authentication, validation, pagination, search, sorting, email notifications, and global exception handling.



## Features

- User registration and login
- JWT authentication
- Role-based authorization
  - Administrator
  - Company
  - Student
- Job/internship CRUD operations
- Company CRUD operations
- Job Applications CRUD operations
- Company Join Request CRUD operations
- Companies can create, update and delete jobs
- Admin approval and rejection of job postings
- Students can apply for jobs
- Companies can view applicants for their jobs
- Job application status tracking
- Users with company role can create, update or delete a company
- Company join requests
- Users can send a request to the company they want to join
- Companies can accept or reject the join request
- Search
- Sorting
- Pagination
- Input validation using FluentValidation
- Global exception handling
- Custom logging
- Email notifications using Resend
- Automatic job expiration based on application deadline
- Swagger/OpenAPI documentation
- SQL Server database with Entity Framework Core
- ASP.NET Core Identity
- Password reset and password change functionality
- Resume/application document upload using Supabase Storage



## User Roles

### Administrator

- Approve or reject job postings
- Delete job postings
- View jobs and companies


### Company

- Create and manage their company profile
- Create job/internship postings
- Manage their own job postings
- View applications submitted to their jobs
- Update application statuses
- Change the status of applictaions
- Accept or reject company join requests
- Search and sort applicants

### Student

- Browse approved job/internship postings
- Apply for jobs
- View their own applications
- Track application status
- Send requests to join companies
- View their own company join requests



## Technologies

- ASP.NET Core Web API
- .NET 10
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Authentication
- FluentValidation
- AutoMapper
- Swagger / OpenAPI
- Resend
- Supabase Storage
- Clean Architecture



## Architecture

The project follows Clean Architecture principles:


```text
JobManagement
│
├── API
│   ├── Controllers
│   ├── Middleware
│   ├── Models
│   └── Program.cs
│
├── Core
│   ├── Application
│   │   ├── DTOs
│   │   ├── Contracts
│   │          ├── Email
│   │          ├── Identity
│   │          ├── Persistence
│   │          ├── Services
│   │   ├── IServices
│   │   └── Enums
│   │   └── Exceptions
│   │   └── Logging
│   │   └── Mapping Profiles
│   │   └── Models
│   │   ├── Validations
│   │
│   └── Domain
│       ├── Entities
│       ├── Shared
│       └── Enums
│
└── Infrastructure
    ├── Identity
    │   ├── Configurations
    │   ├── DbContext
    │   └── Models
    │   └── Services
    │
    └── Infrastructure       
    │   ├── Email Service
    │   ├── Entity Services
    │   └── Logging
    │
    └── Persistence
        ├── DbContext
        ├── Repositories
        └── Configurations

```




## Authentication & Authorization

The API uses ASP.NET Core Identity for user management and JWT Bearer tokens for authentication.

After logging in, the client receives a JWT token which must be included in authenticated requests:

Authorization: Bearer <token>



Authorization is role-based and resource ownership is also enforced where required.

For example:

- Students can only view their own applications.
- Companies can only view applications belonging to their own jobs.
- Company owners can manage their own company and company join requests.
- Administrators have access to administrative operations.






## Prerequisites

- .NET 10 SDK
- SQL Server
- GitHub
- A Supabase account for document storage
- A Resend account/API key for email functionality



## Configuration

Before running the application, configure the following settings:

- SQL Server connection string
- JWT settings
- Resend API key
- Supabase URL
- Supabase API key


```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-sql-server-connection-string"
  },
  "JwtSettings": {
    "Key": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  },
  "Resend": {
    "ApiKey": "your-resend-api-key"
  },
  "Supabase": {
    "Url": "your-supabase-url",
    "ApiKey": "your-supabase-api-key"
  }
}
```

Sensitive values such as API keys should be configured using .NET User Secrets or environment variables rather than committed to the repository.
In this project, the Supabase API key is stored outside `appsettings.json` using .NET configuration secrets.



## Database

The application uses SQL Server with Entity Framework Core.

The database contains entities for:

- Users and roles
- Companies
- Jobs
- Job applications
- Company join requests




## Database Setup

This project uses SQL Server and Entity Framework Core.
The repository includes EF Core migrations required to create the database.

1. Create a SQL Server database or configure a SQL Server instance.
2. Update the connection string in `appsettings.json` or user secrets.
3. Open the project in Visual Studio.
4. Migrations are committed to GitHub.
5. After configuring the SQL Server connection string, run:

Select Persistence project as default project:
update-database -Context ApplicationDbContext

Select Identity project as default project:
update-database -Context ApplicationIdentityDbContext

5. Start the application.




## Running the Application

Clone the repository:

git clone <https://github.com/Uneeza-UT/JobManagement.git>

Navigate to the API project:

cd JobManagement.API

Restore dependencies:

dotnet restore

Apply database migrations:

dotnet ef database update

Run the application:

dotnet run



## Swagger/ API Documentation

The API is documented using Swagger/OpenAPI.

Once the application is running, open the Swagger UI at:

`https://localhost:<port>/swagger`

The port is determined by the application's launch settings/environment.







## Testing the API

The API can be tested using Swagger UI.
The Administrator, Company and Student test accounts are seeded during database initialization.

Recommended testing flow:

1. Clone repository
2. Configure SQL Server connection string
3. Configure secrets
4. Update database for both dbContexts
5. Run the API
6. Open Swagger
7. Use the seeded accounts or create a new one using the Register endpoint
8. Log in to obtain a JWT token.
9. Click **Authorize** in Swagger.
10. Enter the JWT token.
11. Use the endpoints according to the user's role.
12. Create and manage jobs, applications, and company join requests.
13. Test endpoints


 





## Email Notifications

Email notifications are implemented using Resend.

Emails are sent for events such as:

- Account registration
- Company join requests
- Join request acceptance/rejection
- Job application status updates
- Password reset requests



## File Storage

Job application documents are uploaded to Supabase Storage.

Only the unique object key/filename is stored in the database.




## Background Job

The application includes a background service that periodically checks job application deadlines.

When a job's application deadline has passed, its status is automatically changed to `Expired`.

The service runs automatically when the application starts.




## Error Handling

The API uses global exception handling middleware to provide consistent error responses.

Common HTTP status codes include:

- 200 OK
- 201 Created
- 400 Bad Request
- 401 Unauthorized
- 403 Forbidden
- 404 Not Found
- 409 Conflict




## Project Status

Backend API completed.

The API is fully documented through Swagger/OpenAPI and is ready to be consumed by a frontend application.




## Implementation Highlights

- Clean Architecture for separation of concerns
- Repository and service patterns
- ASP.NET Core Identity with JWT authentication
- Role-based and resource-based authorization
- FluentValidation for request validation
- Entity Framework Core with SQL Server
- Global exception handling middleware
- Email notifications using Resend
- Supabase Storage for application documents
- Background service for automatic job expiration
- Swagger/OpenAPI documentation
- Pagination, searching, and sorting