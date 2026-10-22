# SmartTaskReminder

A task management RESTful API built with **ASP.NET Core Web API (.NET 10)**.

## Features

- User authentication using JWT and refresh tokens.
- Task management (Create, Read, Update, Delete).
- Task filtering by date range, priority, and status.
- Pagination for filtered task results.
- Scheduled task reminders using Hangfire.
- Admin dashboard with user and task statistics.
- User profile updates and soft account deletion.
- Consistent API responses and error handling.
- Application logging using Serilog.

## Technologies

- C# / .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication & Refresh Tokens
- Hangfire (Background Jobs)
- Serilog (Logging)
- Swagger / OpenAPI

## Architecture

The project follows a layered architecture:

- **API:** HTTP endpoints and request handling.
- **Application:** Services, interfaces, and DTOs.
- **Core:** Entities and business models.
- **Infrastructure:** Database access and repository implementations.

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server

### Setup

1. Clone the repository.
2. Configure the SQL Server connection string in the application configuration.
3. Configure the required JWT settings.
4. Apply the database migrations using the appropriate Entity Framework Core command for your project setup.
5. Run the API
6. Open Swagger at `/swagger` using the application URL.

## Task Filtering & Pagination

Tasks can be filtered by:

- **Date Range:** Retrieve tasks within a specified start and end date.
- **Priority:** Retrieve tasks based on their priority level.
- **Status:** Retrieve tasks based on their current status.

Filtered results support pagination using page number and page size parameters.

## Admin Dashboard & Statistics

The API provides an administrative dashboard with the following statistics:

- **Total Users:** Total number of registered users.
- **Active Users:** Number of users active in the last 30 days.
- **New Users:** Number of users registered in the last 30 days.
- **Total Tasks:** Total number of tasks.
- **Created Tasks:** Number of tasks created within a specified date range.
- **Completed Tasks:** Number of tasks completed within a specified date range.

The dashboard accepts a start date and an end date to retrieve task statistics for a specific period.

## Background Jobs & Reminders

- Hangfire schedules task reminder jobs based on task due dates and configured reminder times.
- Reminder jobs are scheduled or updated when task reminders are created or modified.
- Existing reminder jobs are cancelled when tasks are deleted or reminders are no longer needed.
- The Hangfire Dashboard is available at `/dashboard` when enabled.

**Note:** Actual reminder delivery depends on the implementation of the reminder job.

## Authentication

To test protected endpoints in Swagger, use the JWT access token with the authorization format expected by the Swagger configuration. If Swagger automatically adds the `Bearer` prefix, enter the token without that prefix.

## Admin Account

The admin account credentials are configured in appsettings.json.

## License

No license specified.