# Buildline Platform Backend

Buildline Platform API is the Sprint 3 Web Services implementation for the Buildline construction logistics platform. It exposes frontend-aligned bounded contexts through versioned REST resources, application command/query services, repository/unit-of-work persistence, Problem Details responses, ACL facades and EF Core migrations.

## Architecture

- `Iam`: authentication, JWT token issuing, users management and identity ACL.
- `Profiles`: company profile lookup/update and profile ACL.
- `Requisition` owns material reference data used by field material requests.
- `Inventory` owns category reference data used by stock filters and material classification.
- `Analytics` owns project reference data used by budget and dashboard views.
- `Requisition`: field material requests, priority, approval status and requested delivery date.
- `Procurement`: purchase orders, quotations and approval-status transitions.
- `Inventory`: project stock, minimum/maximum thresholds and stock update dates.
- `Delivery`: purchase-order delivery tracking, dispatch state, origin, destination and ETA.
- `Suppliers`: supplier directory, operational incidents and supplier performance fields.
- `Analytics`: project budgets, spent amount, allocations and overrun status.
- `Communication`: inbox messages, alerts, read state and starred messages.
- `Shared`: Result model, repositories, unit of work, audit interceptor, route conventions, middleware, Problem Details and localization resources.

Public REST contracts use `/api/v1`. Operational controllers require JWT Bearer authentication. `POST /api/v1/auth/sign-in`, `POST /api/v1/auth/sign-up` and `GET /api/v1/health` remain public.

## Production Deployment

Production is deployed on Railway:

- Base URL: `https://buildline-platform.up.railway.app`
- Health check: `https://buildline-platform.up.railway.app/api/v1/health`
- Swagger UI: `https://buildline-platform.up.railway.app/swagger/index.html`
- Container port: `8080`
- Runtime environment: `Production`

Railway must define these variables in the Backend service, not in the MySQL service:

| Variable | Expected value |
| --- | --- |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ASPNETCORE_URLS` | `http://+:8080` |
| `MYSQL_HOST` | Railway MySQL host from the Connect tab |
| `MYSQL_PORT` | `3306` |
| `MYSQL_DATABASE` | Railway MySQL database name |
| `MYSQL_USER` | Railway MySQL username |
| `MYSQL_PASSWORD` | Railway MySQL password |
| `BUILDLINE_JWT_SECRET` | Long signing key, at least 32 characters |
| `ENABLE_SWAGGER` | `true` for Sprint Review Swagger evidence; set `false` to hide public Swagger later |

Deployment startup behavior:

- The container starts `Buildline.Platform.dll` from the published ASP.NET Core output.
- The app binds to `http://+:8080`; Railway terminates HTTPS at the platform edge.
- Startup attempts EF Core migrations through `Database.MigrateAsync()`.
- If the Railway database already contains application tables without EF migration history, startup preserves the existing schema and data instead of recreating tables.
- After schema initialization, the JSON demo seeder runs.
- If target tables already contain records, seeding is skipped. This means existing production data is preserved.

The current Railway logs show the database already has records because the seeder reports that demo data was skipped. That is expected once the production database has been initialized.

## Swagger Usage

Swagger is available in production for Sprint Review evidence. Public endpoints can be executed directly:

```http
GET https://buildline-platform.up.railway.app/api/v1/health
POST https://buildline-platform.up.railway.app/api/v1/auth/sign-in
POST https://buildline-platform.up.railway.app/api/v1/auth/sign-up
```

Most business endpoints are protected. If Swagger returns `401 Unauthorized` for endpoints such as `/api/v1/companies/{companyId}/messages` or `/api/v1/companies/{companyId}/purchase-orders`, the API is behaving correctly: the request did not include a valid JWT Bearer token.

To execute protected endpoints from Swagger:

1. Call `POST /api/v1/auth/sign-in` or `POST /api/v1/auth/sign-up` to obtain a token.
2. Click the `Authorize` button in Swagger.
3. Paste the JWT token in the Bearer authorization dialog.
4. Execute protected endpoints again.

For raw HTTP clients, send the token as:

```http
Authorization: Bearer <jwt-token>
```

## Frontend Contract Coverage

The API currently covers the Sprint 2 Vue frontend mock resources with versioned endpoints:

| Frontend resource | Backend endpoint | Bounded context | Auth |
| --- | --- | --- | --- |
| `users` | `/api/v1/users` | IAM | Bearer |
| `profiles` | `/api/v1/profiles` | Profiles | Bearer |
| `projects` | `/api/v1/projects` | Analytics reference data | Bearer |
| `materials` | `/api/v1/materials` | Requisition reference data | Bearer |
| `categories` | `/api/v1/categories` | Inventory reference data | Bearer |
| `requisitions` | `/api/v1/requisitions` | Requisition | Bearer |
| `purchase-orders` | `/api/v1/companies/{companyId}/purchase-orders` | Procurement | Bearer |
| `quotations` | `/api/v1/quotations` | Procurement | Bearer |
| `inventory` | `/api/v1/inventory` | Inventory | Bearer |
| `deliveries` | `/api/v1/deliveries` | Delivery | Bearer |
| `suppliers` | `/api/v1/suppliers` | Suppliers | Bearer |
| `incidents` | `/api/v1/incidents` | Suppliers | Bearer |
| `budgets` | `/api/v1/budgets` | Analytics | Bearer |
| `messages` | `/api/v1/messages` | Communication | Bearer |

Company-owned operational routes use `/api/v1/companies/{companyId}/...`; multi-word route segments use kebab-case, for example `/api/v1/companies/{companyId}/purchase-orders`.

## Verification

Local build verification:

```powershell
dotnet build Buildline.sln
```

Production health verification:

```powershell
Invoke-WebRequest -Uri 'https://buildline-platform.up.railway.app/api/v1/health' -UseBasicParsing
```

Expected health response:

```json
{"status":"Healthy","service":"Buildline Platform API"}
```

Swagger verification:

```powershell
Invoke-WebRequest -Uri 'https://buildline-platform.up.railway.app/swagger/index.html' -UseBasicParsing
```

A clean build must remain at `0 warnings / 0 errors`.

## Database

The backend uses EF Core with MySQL. For local manual migration execution:

```powershell
.\.tools\dotnet-ef database update --project Buildline\Platform\Buildline.Platform.csproj --startup-project Buildline\Platform\Buildline.Platform.csproj
```

The migrations create `users`, `profiles`, `projects`, `materials`, `categories`, `requisitions`, `purchase_orders`, `quotations`, `inventory_items`, `deliveries`, `suppliers`, `supplier_incidents`, `budgets` and `messages`. Local and production demonstration data is loaded by the JSON-backed seeder instead of model-builder seed configuration.

## Docker

Build the production image:

```powershell
docker build -t buildline-platform-api .
```

Run locally against a local or host-provided MySQL instance:

```powershell
docker run --rm -p 8080:8080 `
  -e ASPNETCORE_ENVIRONMENT=Production `
  -e ASPNETCORE_URLS=http://+:8080 `
  -e MYSQL_HOST=host.docker.internal `
  -e MYSQL_PORT=3306 `
  -e MYSQL_DATABASE=buildline_platform `
  -e MYSQL_USER=root `
  -e MYSQL_PASSWORD=your-password `
  -e BUILDLINE_JWT_SECRET=replace-with-at-least-32-characters `
  -e ENABLE_SWAGGER=true `
  buildline-platform-api
```

Local container health check:

```http
GET http://localhost:8080/api/v1/health
```
