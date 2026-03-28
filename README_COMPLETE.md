# FlowDesk Task Board API

A lightweight Task Board backend service for FlowDesk, built with ASP.NET Core 10.0. Provides comprehensive REST APIs for managing tasks, projects, and users with validation, error handling, and structured logging.

## Quick Start

```bash
cd c:\Users\Asus\Desktop\assignment

cd FlowDesk.Data
dotnet ef database update --startup-project ../FlowDesk.Api

cd ../FlowDesk.Api
dotnet run
```

API: https://localhost:5001 | Swagger: https://localhost:5001/swagger | Health: https://localhost:5001/api/health

## Project Structure

```
FlowDesk/
├── FlowDesk.Api/              # ASP.NET Core REST API & Controllers
├── FlowDesk.Domain/           # Domain entities, enums, interfaces, exceptions
├── FlowDesk.Data/             # EF Core DbContext, migrations, repositories
├── FlowDesk.Services/         # Business logic, DTOs, validators
└── FlowDesk.Tests/            # Unit and integration tests
```

## Architecture

Layered architecture with clear separation of concerns:

```
Controllers (REST Endpoints)
    ↓
Validation Filters (Input Validation)
    ↓
Services (Business Logic Rules)
    ↓
Repositories (Data Access Pattern)
    ↓
EF Core DbContext
    ↓
SQL Server Database
```

- **Presentation**: REST API controllers with OpenAPI/Swagger documentation
- **Business Logic**: Services enforcing rules, validators for input
- **Data Access**: Generic repository pattern with unit of work
- **Domain**: Entities, enums, interfaces, custom exceptions
- **Cross-cutting**: Global exception middleware, structured logging (Serilog)

## Prerequisites

- **.NET 10.0+** - Download from https://dotnet.microsoft.com
- **SQL Server 2019+ or SQL Server Express** (LocalDB)

## Getting Started

### Database Setup (First Time)

```bash
cd FlowDesk.Data
dotnet ef database update --startup-project ../FlowDesk.Api
```

Creates schema with 3 tables: Users, Projects, Tasks with proper indices and constraints.

### Run API Server

```bash
cd FlowDesk.Api
dotnet run
```

- **REST API**: https://localhost:5001
- **Swagger/OpenAPI**: https://localhost:5001/swagger
- **Health Check**: https://localhost:5001/api/health
- **API Info**: https://localhost:5001/api

### Test with Postman

1. Import `FlowDesk_API_Collection.postman_collection.json`
2. Set variable `baseUrl` to `https://localhost:5001`
3. Start testing endpoints

## Core Features

### Task Management ✅

- CRUD operations with full validation
- Status workflow: Todo → InProgress → Done → Archived
- Priority levels 1-5 with sorting
- Due date validation (cannot be in past)
- Assign/unassign to users
- Archive for soft delete
- Filter by project, user, status

### Project Management ✅

- CRUD operations
- View all project tasks
- Track active task count
- Created by tracking

### User Management ✅

- Registration with strong password validation
- Roles: TeamMember, TeamLead, Admin
- Activate/deactivate users
- Email uniqueness validation
- Password hashing with HMACSHA512

### Data Integrity ✅

- Foreign key validation
- Cascading deletes for projects → tasks
- Unique email constraint
- Strong password requirements (8+ chars, uppercase, lowercase, digit)
- Status transition rules enforcement

### Error Handling ✅

- Global exception middleware
- Structured error responses with codes
- Field-level validation errors
- Proper HTTP status codes (400, 403, 404, 409, 500)

### Logging & Monitoring ✅

- Serilog structured logging
- File rolling by day in `logs/` folder
- Console and file output
- Request logging with timestamps

### API Discovery ✅

- OpenAPI 3.0 / Swagger UI
- Health check endpoint
- API info with endpoint discovery

## API Endpoints (26 Total)

### Health & Discovery (2)

- `GET /api` - API info and endpoint list
- `GET /api/health` - Health check

### Users (8)

- `POST /api/users` - Register user
- `GET /api/users` - List all users
- `GET /api/users/{id}` - Get user
- `GET /api/users/email/{email}` - Get by email
- `GET /api/users/active/list` - List active users
- `PUT /api/users/{id}` - Update user
- `PATCH /api/users/{id}/activate` - Activate
- `PATCH /api/users/{id}/deactivate` - Deactivate

### Projects (6)

- `POST /api/projects` - Create project
- `GET /api/projects` - List all
- `GET /api/projects/{id}` - Get project with task count
- `GET /api/projects/user/{userId}` - User's projects
- `PUT /api/projects/{id}` - Update
- `DELETE /api/projects/{id}` - Delete

### Tasks (12)

- `POST /api/tasks` - Create task
- `GET /api/tasks/{id}` - Get task
- `GET /api/tasks/project/{projectId}` - Project tasks
- `GET /api/tasks/user/{userId}` - User tasks
- `GET /api/tasks/status/{status}` - By status
- `GET /api/tasks/project/{projectId}/archived` - Archived tasks
- `PUT /api/tasks/{id}` - Update task
- `PATCH /api/tasks/{id}/status?status={status}` - Change status
- `PATCH /api/tasks/{id}/assign/{userId}` - Assign
- `PATCH /api/tasks/{id}/unassign` - Unassign
- `PATCH /api/tasks/{id}/archive` - Archive
- `DELETE /api/tasks/{id}` - Delete

## Example Usage

### Create User

```bash
curl -X POST https://localhost:5001/api/users \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john@example.com",
    "fullName": "John Doe",
    "password": "SecurePass123"
  }'
```

### Create Project

```bash
curl -X POST https://localhost:5001/api/projects \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Website Redesign",
    "description": "Q1 initiative"
  }'
```

### Create Task

```bash
curl -X POST https://localhost:5001/api/tasks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Design Homepage",
    "priority": 3,
    "projectId": 1,
    "dueDate": "2026-04-15T00:00:00Z",
    "assignedToUserId": 1
  }'
```

### Update Task Status

```bash
curl -X PATCH "https://localhost:5001/api/tasks/1/status?status=InProgress"
```

## Validation Rules

### User Registration

- Email: Required, valid format, max 255 chars, unique
- Full Name: Required, 1-255 chars
- Password: Min 8 chars, requires uppercase, lowercase, digit

### Task Creation

- Title: Required, 1-255 chars
- Description: Max 2000 chars
- Priority: 1-5 required
- DueDate: Must be future date if provided
- ProjectId: Must exist
- AssignedToUserId: Must exist and be active if provided

### Project Creation

- Name: Required, 1-255 chars
- Description: Max 2000 chars

## Status Transitions

Valid task status flows:

- **Todo** → InProgress, Archived
- **InProgress** → Done, Todo, Archived
- **Done** → Archived
- **Archived** → ✗ (No transitions from archived)

## Data Models

### User

```json
{
  "id": 1,
  "email": "user@example.com",
  "fullName": "John Doe",
  "role": "TeamMember",
  "isActive": true,
  "createdAt": "2026-03-28T10:00:00Z",
  "updatedAt": "2026-03-28T10:00:00Z"
}
```

### Project

```json
{
  "id": 1,
  "name": "Website Redesign",
  "description": "Q1 initiative",
  "createdByUserId": 1,
  "createdByUserName": "John Doe",
  "taskCount": 5,
  "createdAt": "2026-03-28T10:00:00Z",
  "updatedAt": "2026-03-28T10:00:00Z"
}
```

### Task

```json
{
  "id": 1,
  "title": "Design Homepage",
  "description": "Create mockups",
  "priority": 3,
  "status": "Todo",
  "dueDate": "2026-04-15T00:00:00Z",
  "projectId": 1,
  "assignedToUserId": 1,
  "assignedToUserName": "Jane Smith",
  "isArchived": false,
  "createdAt": "2026-03-28T10:00:00Z",
  "updatedAt": "2026-03-28T10:00:00Z"
}
```

## Design Decisions

### Repository Pattern

- Generic `IRepository<T>` base interface with 10 CRUD/query methods
- Specific repositories with domain-specific queries
- Enables testability through mocking

### Unit of Work

- Coordinates multiple repositories
- Transaction support for atomic operations
- Scoped lifetime per HTTP request

### Validation Strategy

- Input validation via FluentValidation (automatic filter)
- Business rule validation in services
- Early validation before database operations

### Status Machine

- Enforced valid transitions to prevent invalid states
- Prevents transitions from archived tasks
- Supports transitions to archived from any state

### Password Security

- HMACSHA512 with random salt
- 8+ characters, uppercase, lowercase, digit requirements
- Passwords never logged

### Soft Deletes

- IsArchived flag instead of hard delete
- History preservation for audit
- Excluded from default queries

## Known Limitations

1. **Authentication** - Hardcoded current user (userId=1) for demo
2. **Authorization** - No role-based checks on endpoints yet
3. **Pagination** - No limit/offset on list endpoints
4. **Filtering** - Basic filters only (status, project, user)
5. **Audit Trail** - No detailed change history tracking
6. **Task Dependencies** - No blocking/subtask relationships

## Future Improvements

- JWT authentication and role-based authorization
- Pagination and advanced filtering
- Task dependencies and subtasks
- File attachments
- Comments and discussions
- Real-time updates via SignalR
- Complete audit logging
- Bulk operations
- CSV/PDF exports
- Rate limiting
- API versioning

## Git Commit History

The project has a clean commit history with meaningful, focused commits:

1. Initial project setup with layered architecture
2. Domain models and initial EF Core migration
3. Repository pattern and unit of work
4. Global exception handling and input validation
5. Business logic services with comprehensive rules
6. REST API controllers and Swagger documentation

View full history: `git log --oneline`

## Troubleshooting

### Database Issues

```bash
# Verify SQL Server running
sqlcmd -S (localdb)\mssqllocaldb

# Recreate database from scratch
dotnet ef database drop --force --project FlowDesk.Data
dotnet ef database update --project FlowDesk.Data
```

### Build Errors

```bash
dotnet clean
dotnet build
```

### Port Already in Use

Modify `FlowDesk.Api/Properties/launchSettings.json` or close other applications using port 5001

## Performance Considerations

- Repositories include indices on frequently queried columns
- Eager loading of related entities where needed
- Set null cascading for optional relationships
- Cascading delete for dependent entities
- Lazy repository instantiation in unit of work

## Security Notes

- Passwords hashed with HMACSHA512 + random salt
- SQL injection prevented via parameterized EF Core queries
- Input validation prevents malformed data
- CORS configured for development (restrict in production)
- Exception details not exposed to clients

## Testing

Run unit tests:

```bash
dotnet test --project FlowDesk.Tests
```

Run specific test:

```bash
dotnet test --project FlowDesk.Tests --filter "TestName"
```

## Deployment

For production:

1. Set up database in managed SQL instance
2. Update connection string in appsettings.Production.json
3. Enable authentication/authorization
4. Configure CORS for specific origins
5. Set up monitoring and alerting
6. Use cloud provider (Azure App Service, AWS Lambda, etc.)

## License & Support

Internal FlowDesk project. For questions, refer to assignment documentation.
