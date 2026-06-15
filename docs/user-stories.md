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

