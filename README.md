# Buildline Platform Backend

Buildline Platform Backend is the Sprint 3 Web Services implementation for the Buildline construction logistics platform. The solution follows the course learning-center structure: bounded contexts, CQRS-style application services, repository/unit-of-work persistence, REST resources and assemblers, shared Problem Details handling, JWT authentication, ACL facades and EF Core migrations.

## Architecture

- `Iam`: authentication, JWT token issuing, users management and identity ACL.
- `Profiles`: company profile lookup/update and profile ACL.
- `Projects`: shared project reference data and project ACL.
- `Materials`: material catalog commands and queries for requisitions/inventory.
- `Categories`: read-only material category reference data.
- `Shared`: Result model, repositories, unit of work, audit interceptor, route conventions, middleware, Problem Details and localization resources.

Public REST contracts use `/api/v1`. Operational controllers require JWT Bearer authentication. `POST /api/v1/auth/sign-in`, `POST /api/v1/auth/sign-up` and `GET /api/v1/health` remain public.

## Verification

```powershell
dotnet build Backend.sln
```

The project is configured to generate XML documentation and include it in Swagger. A clean build must remain at `0 warnings / 0 errors`.

## Database

The backend uses EF Core with MySQL:

```powershell
.\.tools\dotnet-ef database update --project Backend --startup-project Backend
```

The initial migration creates `users`, `profiles`, `projects`, `materials` and `categories`, including seed data aligned with the Sprint 2 frontend mock contracts.

## Docker

```powershell
docker build -t buildline-platform-api .
docker run --rm -p 8080:8080 `
  -e MYSQL_HOST=host.docker.internal `
  -e MYSQL_PORT=3306 `
  -e MYSQL_DATABASE=buildline_platform `
  -e MYSQL_USER=root `
  -e MYSQL_PASSWORD=your-password `
  -e BUILDLINE_JWT_SECRET=replace-with-at-least-32-characters `
  buildline-platform-api
```

Health check:

```http
GET http://localhost:8080/api/v1/health
```
