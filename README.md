# Buildline Platform Backend

Buildline Platform API is the Sprint 3 Web Services implementation for the Buildline construction logistics platform. The solution follows the course learning-center structure: bounded contexts, CQRS-style application services, repository/unit-of-work persistence, REST resources and assemblers, shared Problem Details handling, JWT authentication, ACL facades and EF Core migrations.

## Architecture

- `Iam`: authentication, JWT token issuing, users management and identity ACL.
- `Profiles`: company profile lookup/update and profile ACL.
- `Catalog`: reference data for projects, materials and categories used by requisition, procurement, inventory and analytics workflows.
- `Requisition`: field material requests, priority, approval status and requested delivery date.
- `Procurement`: purchase orders, quotations and approval-status transitions.
- `Inventory`: project stock, minimum/maximum thresholds and stock update dates.
- `Delivery`: purchase-order delivery tracking, dispatch state, origin, destination and ETA.
- `Suppliers`: supplier directory, operational incidents and supplier performance fields.
- `Analytics`: project budgets, spent amount, allocations and overrun status.
- `Communication`: inbox messages, alerts, read state and starred messages.
- `Shared`: Result model, repositories, unit of work, audit interceptor, route conventions, middleware, Problem Details and localization resources.

Public REST contracts use `/api/v1`. Operational controllers require JWT Bearer authentication. `POST /api/v1/auth/sign-in`, `POST /api/v1/auth/sign-up` and `GET /api/v1/health` remain public.

## Frontend Contract Coverage

The API currently covers the Sprint 2 Vue frontend mock resources with versioned endpoints:

| Frontend resource | Backend endpoint | Bounded context |
| --- | --- | --- |
| `users` | `/api/v1/users` | IAM |
| `profiles` | `/api/v1/profiles` | Profiles |
| `projects` | `/api/v1/projects` | Catalog reference data |
| `materials` | `/api/v1/materials` | Catalog reference data |
| `categories` | `/api/v1/categories` | Catalog reference data |
| `requisitions` | `/api/v1/requisitions` | Requisition |
| `purchaseOrders` | `/api/v1/purchaseOrders` | Procurement |
| `quotations` | `/api/v1/quotations` | Procurement |
| `inventory` | `/api/v1/inventory` | Inventory |
| `deliveries` | `/api/v1/deliveries` | Delivery |
| `suppliers` | `/api/v1/suppliers` | Suppliers |
| `incidents` | `/api/v1/incidents` | Suppliers |
| `budgets` | `/api/v1/budgets` | Analytics |
| `messages` | `/api/v1/messages` | Communication |

## Verification

```powershell
dotnet build Buildline.sln
```

The project is configured to generate XML documentation and include it in Swagger. A clean build must remain at `0 warnings / 0 errors`.

## Database

The backend uses EF Core with MySQL:

```powershell
.\.tools\dotnet-ef database update --project Buildline\Platform\Buildline.Platform.csproj --startup-project Buildline\Platform\Buildline.Platform.csproj
```

The migrations create `users`, `profiles`, `projects`, `materials`, `categories`, `requisitions`, `purchase_orders`, `quotations`, `inventory_items`, `deliveries`, `suppliers`, `supplier_incidents`, `budgets` and `messages`, including seed data aligned with the Sprint 2 frontend mock contracts.

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

