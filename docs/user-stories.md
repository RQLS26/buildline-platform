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

