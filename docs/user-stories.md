# Buildline Platform - REST API Technical Stories

This document contains API-focused technical stories and backend improvements for the Buildline Platform REST API. It follows the same endpoint-oriented style used by the Learning Center Platform reference project while preserving Buildline bounded contexts and frontend contracts.

Common conventions:
- Base path: /api/v1.
- API implementation: ASP.NET Core / C#.
- Frontend contexts: iam, profiles, shared, requisition, procurement, inventory, delivery, suppliers, analytics-budgeting and communication.
- Projects, materials and categories are documented as reference catalog modules, not as independent frontend bounded contexts.

---

### IMP-BE-001 - Backend foundations

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** shared / platform  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to configure the ASP.NET Core platform foundation so every bounded context exposes secure, documented, versioned contracts.

Acceptance criteria:
- Given the API is running, when GET /api/v1/health is requested, then the service responds 200 OK.
- Given Swagger is opened, when the API document is generated, then XML summaries, parameters, response metadata and bearer authentication are visible.

---

### IMP-BE-002 - Persistence, migrations and seed data

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** shared persistence  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to configure transactional persistence, audit metadata, constraints and initial data compatible with the frontend mock contracts.

Acceptance criteria:
- Given the database is empty, when migrations are applied, then operational tables for the prioritized contexts are created.
- Given the frontend replaces json-server, when reference endpoints are queried, then initial data is available for smoke testing.

---

### TS-IAM-001 - Sign-in API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** iam  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to authenticate users, obtain a JWT token and initialize the client session.

Acceptance criteria:
- Given valid credentials, when the sign-in endpoint is called, then the API returns 200 OK with user data and token.
- Given invalid credentials, when the endpoint is called, then the API returns a controlled error without issuing a token.

---

### TS-IAM-002 - Sign-up API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** iam  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to register new users for controlled platform access.

Acceptance criteria:
- Given a valid registration payload, when the endpoint is called, then the API creates the user and returns confirmation data.
- Given missing or duplicated data, when the command is validated, then the API returns 400 Bad Request.

---

### TS-IAM-003 - Users directory API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** iam  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to list and create administrative users from the web application.

Acceptance criteria:
- Given users exist, when GET /api/v1/users is called, then the API returns a user collection.
- Given a valid user payload, when POST /api/v1/users is called, then the API persists and returns the created resource.

---

### TS-IAM-004 - User detail and update API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** iam  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to show user details and update editable role or status information.

Acceptance criteria:
- Given an existing user id, when the detail endpoint is called, then the API returns 200 OK with the user resource.
- Given valid changes, when PATCH is executed, then the API persists the update and returns the modified user.

---

### TS-PROF-001 - Profile read and update API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** profiles  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to display and update company or user profile information.

Acceptance criteria:
- Given a profile exists, when it is requested by id, then the API returns ProfileResource.
- Given valid profile changes, when PUT or PATCH is executed, then the API returns the updated profile.

---

### TS-SHARED-001 - Projects reference API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** reference catalog  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to provide project reference data for requisitions, inventory and dashboards.

Acceptance criteria:
- Given seeded projects exist, when GET /api/v1/projects is called, then the API returns a collection usable by frontend filters.
- Given an existing project id, when the detail endpoint is called, then the API returns the project reference resource.

---

### TS-SHARED-002 - Materials catalog API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** reference catalog  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to provide material catalog data for requisitions, procurement and inventory.

Acceptance criteria:
- Given materials exist, when the catalog is queried, then the API returns name, unit and category data.
- Given valid catalog changes, when create/update/delete operations are executed, then the API persists the material catalog state.

---

### TS-SHARED-003 - Categories reference API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** reference catalog  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to provide category reference data for material classification and inventory filters.

Acceptance criteria:
- Given categories exist, when GET /api/v1/categories is called, then the API returns stable reference data.
- Given an existing category id, when the detail endpoint is called, then the API returns the category resource.

---

### TS-REQ-001 - Create requisition API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** requisition  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to register material requests from construction sites.

Acceptance criteria:
- Given material, project, quantity, unit, priority and requested date, when POST is executed, then the API returns 201 Created.
- Given missing required data, when the payload is validated, then the API returns a validation error.

---

### TS-REQ-002 - List and detail requisitions API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** requisition  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to implement requisition list and detail screens.

Acceptance criteria:
- Given requisitions exist, when the list endpoint is called, then the API returns status, priority, requester and project data.
- Given a non-existent id, when the detail endpoint is called, then the API returns 404 Not Found.

---

### TS-REQ-003 - Update requisition API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** requisition  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to reflect status, priority and operational updates for material requests.

Acceptance criteria:
- Given an existing requisition and valid changes, when PATCH is executed, then the API returns the updated resource.
- Given an invalid id, when PATCH is executed, then the API returns 404 Not Found.

---

### TS-PROC-001 - Quotations list and create API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** procurement  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to register and compare supplier quotations.

Acceptance criteria:
- Given a valid quotation payload, when POST is executed, then the API returns 201 Created.
- Given quotations exist, when GET is called, then the API returns comparable supplier offer data.

---

### TS-PROC-002 - Quotation detail and update API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** procurement  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to review quotation details and update quotation state.

Acceptance criteria:
- Given an existing quotation id, when GET is called, then the API returns supplier, material and amount data.
- Given a valid state change, when PATCH is executed, then the API persists the decision.

---

### TS-PROC-003 - Purchase orders list and create API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** procurement  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to formalize approved purchases and show purchase history.

Acceptance criteria:
- Given supplier, project, material and amount data, when POST is executed, then the API creates a purchase order.
- Given purchase orders exist, when GET is called, then the API returns traceable purchase history.

---

### TS-PROC-004 - Purchase order detail and update API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** procurement  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to query purchase order details and approve or reject orders.

Acceptance criteria:
- Given an existing purchase order id, when GET is called, then the API returns supplier, amount and state.
- Given a valid transition, when PATCH is executed, then the API updates state without losing traceability.

---

### TS-INV-001 - Inventory list and create API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** inventory  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to visualize and register stock per project.

Acceptance criteria:
- Given inventory items exist, when GET is called, then the API returns current, minimum and maximum stock.
- Given a valid item payload, when POST is executed, then the API creates the inventory item.

---

### TS-INV-002 - Inventory stock update API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** inventory  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to reflect warehouse movements in stock values.

Acceptance criteria:
- Given a valid stock update, when PATCH is executed, then the API updates stock and last updated date.
- Given an invalid inventory id, when PATCH is called, then the API returns 404 Not Found.

---

### TS-DEL-001 - Delivery tracking list and create API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** delivery  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to register and track deliveries linked to purchase orders.

Acceptance criteria:
- Given purchase order, supplier, origin, destination and ETA, when POST is executed, then the API creates a delivery with tracking id.
- Given deliveries exist, when GET is called, then the API returns tracking information for field and logistics users.

---

### TS-DEL-002 - Delivery status update API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** delivery  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to reflect dispatched, delayed, in-transit or delivered states.

Acceptance criteria:
- Given a valid delivery state transition, when PATCH is executed, then the API returns the updated delivery.
- Given a non-existent delivery id, when PATCH is called, then the API returns 404 Not Found.

---

### TS-SUP-001 - Suppliers directory API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** suppliers  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to maintain the supplier directory used by procurement and incidents.

Acceptance criteria:
- Given a valid supplier payload, when POST is executed, then the API returns the created supplier.
- Given suppliers exist, when GET is called, then the API returns rating, category and delivery performance data.

---

### TS-SUP-002 - Supplier update and delete API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** suppliers  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to update supplier information and remove inactive suppliers from operational views.

Acceptance criteria:
- Given valid supplier changes, when PATCH is executed, then the API returns the updated supplier.
- Given an existing supplier, when DELETE is executed, then the supplier is removed or deactivated from active listings.

---

### TS-SUP-003 - Incidents list and create API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** suppliers  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to record operational problems related to suppliers or deliveries.

Acceptance criteria:
- Given supplier, order, severity and description, when POST is executed, then the API creates an open incident.
- Given incidents exist, when GET is called, then the API returns severity, supplier and state data.

---

### TS-SUP-004 - Incident status update API

**Branch:** $(System.Collections.Hashtable.Branch)  
**Context / module:** suppliers  
**Endpoint(s):** $(System.Collections.Hashtable.Endpoints)

As a frontend developer, I want to close, escalate or update incident status.

Acceptance criteria:
- Given an existing incident and valid changes, when PATCH is executed, then the API returns the updated incident.
- Given an invalid id, when PATCH is called, then the API returns 404 Not Found.

