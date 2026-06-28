# Buildline Platform - REST API Technical Stories

This document contains API-focused technical stories and backend improvements for the Buildline Platform REST API. It follows the same endpoint-oriented style used by the course reference project while preserving Buildline bounded contexts and frontend contracts.

Common conventions:
- Base path: `/api/v1`.
- Company-scoped operational base path: `/api/v1/companies/{companyId}`.
- API implementation: ASP.NET Core / C#.
- Frontend contexts: `iam`, `profiles`, `shared`, `requisition`, `procurement`, `inventory`, `delivery`, `suppliers`, `analytics-budgeting` and `communication`.
- `Materials`, `Categories` and `Projects` are implemented as reference data inside existing frontend-aligned bounded contexts: `requisition`, `inventory` and `analytics-budgeting` respectively; they are not independent bounded contexts and are not part of the technical `Shared` kernel.
- Authentication and the current-user projection remain global (`/api/v1/auth/*`, `/api/v1/users/me`), while operational resources are scoped by company to prevent cross-company data leakage.

---

### IMP-BE-001 - Backend foundations

**Branch:** `feature/backend-foundations`
**Context / module:** shared / platform
**Endpoint(s):** `GET /api/v1/health, Swagger/OpenAPI, JWT Bearer, Problem Details`

As a frontend developer, I want to configure the ASP.NET Core platform foundation so every bounded context exposes secure, documented, versioned contracts.

Acceptance criteria:
- Given the API is running, when GET /api/v1/health is requested, then the service responds 200 OK.
- Given Swagger is opened, when the API document is generated, then XML summaries, parameters, response metadata and bearer authentication are visible.

---

### IMP-BE-002 - Persistence, migrations and seed data

**Branch:** `feature/backend-persistence-migrations`
**Context / module:** shared persistence
**Endpoint(s):** `EF Core DbContext, migrations, seed data`

As a frontend developer, I want to configure transactional persistence, audit metadata, constraints and initial data compatible with the frontend mock contracts.

Acceptance criteria:
- Given the database is empty, when migrations are applied, then operational tables for the prioritized contexts are created.
- Given the frontend replaces json-server, when reference endpoints are queried, then initial data is available for smoke testing.

---

### TS-IAM-001 - Sign-in API

**Branch:** `feature/TS-IAM-001-sign-in-api`
**Context / module:** iam
**Endpoint(s):** `POST /api/v1/auth/sign-in`

As a frontend developer, I want to authenticate users, obtain a JWT token and initialize the client session.

Acceptance criteria:
- Given valid credentials, when the sign-in endpoint is called, then the API returns 200 OK with user data and token.
- Given invalid credentials, when the endpoint is called, then the API returns a controlled error without issuing a token.

---

### TS-IAM-002 - Sign-up API

**Branch:** `feature/TS-IAM-002-sign-up-api`
**Context / module:** iam
**Endpoint(s):** `POST /api/v1/auth/sign-up`

As a frontend developer, I want to register new users for controlled platform access.

Acceptance criteria:
- Given a valid registration payload, when the endpoint is called, then the API creates the user and returns confirmation data.
- Given missing or duplicated data, when the command is validated, then the API returns 400 Bad Request.

---

### TS-IAM-003 - Users directory API

**Branch:** `feature/TS-IAM-003-users-directory-api`
**Context / module:** iam
**Endpoint(s):** `GET /api/v1/companies/{companyId}/users, POST /api/v1/companies/{companyId}/users`

As a frontend developer, I want to list and create administrative users from the web application.

Acceptance criteria:
- Given users exist, when GET /api/v1/companies/{companyId}/users is called, then the API returns a user collection.
- Given a valid user payload, when POST /api/v1/companies/{companyId}/users is called, then the API persists and returns the created resource.

---

### TS-IAM-004 - User detail and update API

**Branch:** `feature/TS-IAM-004-user-detail-update-api`
**Context / module:** iam
**Endpoint(s):** `GET /api/v1/companies/{companyId}/users/{id}, PATCH /api/v1/companies/{companyId}/users/{id}`

As a frontend developer, I want to show user details and update editable role or status information.

Acceptance criteria:
- Given an existing user id, when the detail endpoint is called, then the API returns 200 OK with the user resource.
- Given valid changes, when PATCH is executed, then the API persists the update and returns the modified user.

---

### TS-PROF-001 - Profile read and update API

**Branch:** `feature/TS-PROF-001-profile-api`
**Context / module:** profiles
**Endpoint(s):** `GET /api/v1/profiles, GET /api/v1/profiles/{id}, PUT/PATCH /api/v1/profiles/{id}`

As a frontend developer, I want to display and update company or user profile information.

Acceptance criteria:
- Given a profile exists, when it is requested by id, then the API returns ProfileResource.
- Given valid profile changes, when PUT or PATCH is executed, then the API returns the updated profile.

---

### TS-ANB-003 - Projects reference API

**Branch:** `feature/TS-ANB-003-projects-reference-api`
**Context / module:** analytics-budgeting / projects reference data
**Endpoint(s):** `GET /api/v1/companies/{companyId}/projects, GET /api/v1/companies/{companyId}/projects/{id}`

As a frontend developer, I want to provide project reference data for requisitions, inventory and dashboards.

Acceptance criteria:
- Given seeded projects exist, when GET /api/v1/companies/{companyId}/projects is called, then the API returns a collection usable by frontend filters.
- Given an existing project id, when the detail endpoint is called, then the API returns the project reference resource.

---

### TS-REQ-004 - Materials reference API

**Branch:** `feature/TS-REQ-004-materials-reference-api`
**Context / module:** requisition / materials reference data
**Endpoint(s):** `GET/POST /api/v1/companies/{companyId}/materials, GET/PUT/PATCH/DELETE /api/v1/companies/{companyId}/materials/{id}`

As a frontend developer, I want to provide material reference data for requisitions, procurement and inventory.

Acceptance criteria:
- Given materials exist, when material references are queried, then the API returns name, unit and category data.
- Given valid reference data changes, when create/update/delete operations are executed, then the API persists the material reference state.

---

### TS-INV-003 - Categories reference API

**Branch:** `feature/TS-INV-003-categories-reference-api`
**Context / module:** inventory / categories reference data
**Endpoint(s):** `GET /api/v1/categories, GET /api/v1/categories/{id}`

As a frontend developer, I want to provide category reference data for material classification and inventory filters.

Acceptance criteria:
- Given categories exist, when GET /api/v1/categories is called, then the API returns stable reference data.
- Given an existing category id, when the detail endpoint is called, then the API returns the category resource.

---

### TS-REQ-001 - Create requisition API

**Branch:** `feature/TS-REQ-001-create-requisition-api`
**Context / module:** requisition
**Endpoint(s):** `POST /api/v1/companies/{companyId}/requisitions`

As a frontend developer, I want to register material requests from construction sites.

Acceptance criteria:
- Given material, project, quantity, unit, priority and requested date, when POST is executed, then the API returns 201 Created.
- Given missing required data, when the payload is validated, then the API returns a validation error.

---

### TS-REQ-002 - List and detail requisitions API

**Branch:** `feature/TS-REQ-002-list-detail-requisitions-api`
**Context / module:** requisition
**Endpoint(s):** `GET /api/v1/companies/{companyId}/requisitions, GET /api/v1/companies/{companyId}/requisitions/{id}`

As a frontend developer, I want to implement requisition list and detail screens.

Acceptance criteria:
- Given requisitions exist, when the list endpoint is called, then the API returns status, priority, requester and project data.
- Given a non-existent id, when the detail endpoint is called, then the API returns 404 Not Found.

---

### TS-REQ-003 - Update requisition API

**Branch:** `feature/TS-REQ-003-update-requisition-api`
**Context / module:** requisition
**Endpoint(s):** `PATCH /api/v1/companies/{companyId}/requisitions/{id}`

As a frontend developer, I want to reflect status, priority and operational updates for material requests.

Acceptance criteria:
- Given an existing requisition and valid changes, when PATCH is executed, then the API returns the updated resource.
- Given an invalid id, when PATCH is executed, then the API returns 404 Not Found.

---

### TS-PROC-001 - Quotations list and create API

**Branch:** `feature/TS-PROC-001-quotations-api`
**Context / module:** procurement
**Endpoint(s):** `GET /api/v1/companies/{companyId}/quotations, POST /api/v1/companies/{companyId}/quotations`

As a frontend developer, I want to register and compare supplier quotations.

Acceptance criteria:
- Given a valid quotation payload, when POST is executed, then the API returns 201 Created.
- Given quotations exist, when GET is called, then the API returns comparable supplier offer data.

---

### TS-PROC-002 - Quotation detail and update API

**Branch:** `feature/TS-PROC-002-quotation-detail-update-api`
**Context / module:** procurement
**Endpoint(s):** `GET /api/v1/companies/{companyId}/quotations/{id}, PATCH /api/v1/companies/{companyId}/quotations/{id}`

As a frontend developer, I want to review quotation details and update quotation state.

Acceptance criteria:
- Given an existing quotation id, when GET is called, then the API returns supplier, material and amount data.
- Given a valid state change, when PATCH is executed, then the API persists the decision.

---

### TS-PROC-003 - Purchase orders list and create API

**Branch:** `feature/TS-PROC-003-purchase-orders-api`
**Context / module:** procurement
**Endpoint(s):** `GET /api/v1/companies/{companyId}/purchase-orders, POST /api/v1/companies/{companyId}/purchase-orders`

As a frontend developer, I want to formalize approved purchases and show purchase history.

Acceptance criteria:
- Given supplier, project, material and amount data, when POST is executed, then the API creates a purchase order.
- Given purchase orders exist, when GET is called, then the API returns traceable purchase history.

---

### TS-PROC-004 - Purchase order detail and update API

**Branch:** `feature/TS-PROC-004-purchase-order-update-api`
**Context / module:** procurement
**Endpoint(s):** `GET /api/v1/companies/{companyId}/purchase-orders/{id}, PATCH /api/v1/companies/{companyId}/purchase-orders/{id}`

As a frontend developer, I want to query purchase order details and approve or reject orders.

Acceptance criteria:
- Given an existing purchase order id, when GET is called, then the API returns supplier, amount and state.
- Given a valid transition, when PATCH is executed, then the API updates state without losing traceability.

---

### TS-INV-001 - Inventory list and create API

**Branch:** `feature/TS-INV-001-inventory-api`
**Context / module:** inventory
**Endpoint(s):** `GET /api/v1/companies/{companyId}/inventory, POST /api/v1/companies/{companyId}/inventory`

As a frontend developer, I want to visualize and register stock per project.

Acceptance criteria:
- Given inventory items exist, when GET is called, then the API returns current, minimum and maximum stock.
- Given a valid item payload, when POST is executed, then the API creates the inventory item.

---

### TS-INV-002 - Inventory stock update API

**Branch:** `feature/TS-INV-002-inventory-stock-update-api`
**Context / module:** inventory
**Endpoint(s):** `PATCH /api/v1/companies/{companyId}/inventory/{id}`

As a frontend developer, I want to reflect warehouse movements in stock values.

Acceptance criteria:
- Given a valid stock update, when PATCH is executed, then the API updates stock and last updated date.
- Given an invalid inventory id, when PATCH is called, then the API returns 404 Not Found.

---

### TS-DEL-001 - Delivery tracking list and create API

**Branch:** `feature/TS-DEL-001-delivery-tracking-api`
**Context / module:** delivery
**Endpoint(s):** `GET /api/v1/companies/{companyId}/deliveries, POST /api/v1/companies/{companyId}/deliveries`

As a frontend developer, I want to register and track deliveries linked to purchase orders.

Acceptance criteria:
- Given purchase order, supplier, origin, destination and ETA, when POST is executed, then the API creates a delivery with tracking id.
- Given deliveries exist, when GET is called, then the API returns tracking information for field and logistics users.

---

### TS-DEL-002 - Delivery status update API

**Branch:** `feature/TS-DEL-002-delivery-status-update-api`
**Context / module:** delivery
**Endpoint(s):** `PATCH /api/v1/companies/{companyId}/deliveries/{id}`

As a frontend developer, I want to reflect dispatched, delayed, in-transit or delivered states.

Acceptance criteria:
- Given a valid delivery state transition, when PATCH is executed, then the API returns the updated delivery.
- Given a non-existent delivery id, when PATCH is called, then the API returns 404 Not Found.

---

### TS-SUP-001 - Suppliers directory API

**Branch:** `feature/TS-SUP-001-suppliers-directory-api`
**Context / module:** suppliers
**Endpoint(s):** `GET /api/v1/companies/{companyId}/suppliers, POST /api/v1/companies/{companyId}/suppliers`

As a frontend developer, I want to maintain the supplier directory used by procurement and incidents.

Acceptance criteria:
- Given a valid supplier payload, when POST is executed, then the API returns the created supplier.
- Given suppliers exist, when GET is called, then the API returns rating, category and delivery performance data.

---

### TS-SUP-002 - Supplier update and delete API

**Branch:** `feature/TS-SUP-002-supplier-update-delete-api`
**Context / module:** suppliers
**Endpoint(s):** `PATCH /api/v1/companies/{companyId}/suppliers/{id}, DELETE /api/v1/companies/{companyId}/suppliers/{id}`

As a frontend developer, I want to update supplier information and remove inactive suppliers from operational views.

Acceptance criteria:
- Given valid supplier changes, when PATCH is executed, then the API returns the updated supplier.
- Given an existing supplier, when DELETE is executed, then the supplier is removed or deactivated from active listings.

---

### TS-SUP-003 - Incidents list and create API

**Branch:** `feature/TS-SUP-003-incidents-api`
**Context / module:** suppliers
**Endpoint(s):** `GET /api/v1/companies/{companyId}/incidents, POST /api/v1/companies/{companyId}/incidents`

As a frontend developer, I want to record operational problems related to suppliers or deliveries.

Acceptance criteria:
- Given supplier, order, severity and description, when POST is executed, then the API creates an open incident.
- Given incidents exist, when GET is called, then the API returns severity, supplier and state data.

---

### TS-SUP-004 - Incident status update API

**Branch:** `feature/TS-SUP-004-incident-status-update-api`
**Context / module:** suppliers
**Endpoint(s):** `PATCH /api/v1/companies/{companyId}/incidents/{id}`

As a frontend developer, I want to close, escalate or update incident status.

Acceptance criteria:
- Given an existing incident and valid changes, when PATCH is executed, then the API returns the updated incident.
- Given an invalid id, when PATCH is called, then the API returns 404 Not Found.

---

### TS-ANB-001 - Budget dashboard API

**Branch:** `feature/TS-ANB-001-budget-dashboard-api`
**Context / module:** analytics-budgeting
**Endpoint(s):** `GET /api/v1/companies/{companyId}/budgets, POST /api/v1/companies/{companyId}/budgets`

As a frontend developer, I want to feed analytics-budgeting dashboards with budget and spending indicators.

Acceptance criteria:
- Given project budgets exist, when GET is called, then the API returns totalBudget, spent, allocated and status.
- Given a valid budget payload, when POST is executed, then the API creates the budget record for dashboard consumption.

---

### TS-ANB-002 - Budget update API

**Branch:** `feature/TS-ANB-002-budget-update-api`
**Context / module:** analytics-budgeting
**Endpoint(s):** `PATCH /api/v1/companies/{companyId}/budgets/{id}`

As a frontend developer, I want to update financial values used to calculate budget deviation states.

Acceptance criteria:
- Given valid spent or allocated changes, when PATCH is executed, then the API returns the updated budget.
- Given a non-existent budget id, when PATCH is called, then the API returns 404 Not Found.

---

### TS-COM-001 - Messages inbox API

**Branch:** `feature/TS-COM-001-messages-inbox-api`
**Context / module:** communication
**Endpoint(s):** `GET /api/v1/companies/{companyId}/messages, POST /api/v1/companies/{companyId}/messages`

As a frontend developer, I want to show the internal inbox and create operational notifications.

Acceptance criteria:
- Given messages exist, when GET is called, then the API returns sender, subject, category, read and starred data.
- Given a valid message payload, when POST is executed, then the API returns the created message.

---

### TS-COM-002 - Message state and delete API

**Branch:** `feature/TS-COM-002-message-state-delete-api`
**Context / module:** communication
**Endpoint(s):** `PATCH /api/v1/companies/{companyId}/messages/{id}, DELETE /api/v1/companies/{companyId}/messages/{id}`

As a frontend developer, I want to mark messages as read/starred or remove them from the inbox.

Acceptance criteria:
- Given an existing message, when PATCH is executed, then the API updates read or starred state.
- Given an existing message, when DELETE is executed, then the message no longer appears in the inbox.

---

### IMP-BE-003 - Deployment and integration readiness

**Branch:** `feature/backend-deployment-readiness`
**Context / module:** deployment
**Endpoint(s):** `Dockerfile, appsettings.Production.json, Buildline.Platform.http smoke requests`

As a frontend developer, I want to prepare the Web Services for deployment and frontend integration smoke testing.

Acceptance criteria:
- Given production variables are provided, when the API is built or containerized, then it starts with production-safe configuration.
- Given the frontend replaces json-server, when smoke requests are executed, then the prioritized endpoints respond with the documented contracts.
