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

