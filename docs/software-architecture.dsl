workspace "Buildline Platform" "C4 model for the Buildline backend web services and their integration with the deployed web application." {

  model {
    projectManager = person "Project Manager" "Approves purchases, monitors budget and reviews logistics indicators."
    logisticsAnalyst = person "Logistics Analyst" "Creates requisitions, quotations, purchase orders, suppliers and delivery records."
    residentEngineer = person "Resident Engineer" "Requests materials and follows delivery status from construction sites."
    companyOwner = person "Company Owner" "Creates the company account, manages users, roles and settings."

    buildline = softwareSystem "Buildline" "Digital construction logistics platform for requisitions, procurement, inventory, delivery tracking, suppliers, budgets and communication." {
      landing = container "Landing Page" "Public marketing site that communicates the product value proposition." "Static web app / Vercel"
      frontend = container "Frontend Web Application" "Vue.js SPA used by authenticated users to operate Buildline workflows." "Vue 3, Vite, Pinia, PrimeVue / Vercel"
      api = container "Backend Web Services" "Company-scoped REST API that exposes Buildline bounded context contracts." "ASP.NET Core, C#, EF Core / Railway" {
        iam = component "IAM Context" "Authenticates users, issues JWTs, stores roles, membership status, 2FA flag and last login."
        profiles = component "Profiles Context" "Stores company profile information used to scope tenant data."
        requisition = component "Requisition Context" "Manages material requests and material reference data."
        procurement = component "Procurement Context" "Manages quotations and purchase orders."
        inventory = component "Inventory Context" "Manages categories, stock and inventory item state."
        delivery = component "Delivery Context" "Registers delivery tracking information and delivery status transitions."
        suppliers = component "Suppliers Context" "Manages supplier directory and supplier incidents."
        analytics = component "Analytics Budgeting Context" "Provides project references, budget indicators and spending state."
        communication = component "Communication Context" "Manages messages, read state, starred state, archive state and membership notifications."
        shared = component "Shared Infrastructure" "Provides persistence, Unit of Work, Problem Details, Swagger, middleware, CORS, JWT configuration, migrations and seed data."
      }
      database = container "Buildline Database" "Persists users, profiles, projects, materials, categories, requisitions, quotations, purchase orders, inventory, deliveries, suppliers, incidents, budgets and messages." "MySQL / Railway"
    }

    projectManager -> frontend "Uses dashboards, purchase approvals, budgets and reports"
    logisticsAnalyst -> frontend "Operates requisitions, procurement, suppliers and deliveries"
    residentEngineer -> frontend "Creates material requests and follows deliveries"
    companyOwner -> frontend "Creates company, invites users and manages roles"
    projectManager -> landing "Reviews public product information"

    frontend -> api "Consumes REST/JSON endpoints with JWT Bearer" "HTTPS"
    api -> database "Reads and writes company-scoped aggregates" "EF Core / MySQL"

    iam -> profiles "Assigns users to company profiles"
    requisition -> analytics "Uses projects as reference data"
    requisition -> inventory "Uses materials and categories as reference data"
    procurement -> requisition "Turns approved requisitions into quotations and orders"
    delivery -> procurement "Tracks purchase order delivery status"
    suppliers -> procurement "Provides supplier data for quotations and purchase orders"
    analytics -> procurement "Calculates budget impact from purchase orders"
    communication -> iam "Targets users and company memberships"
    iam -> shared "Uses hashing, JWT, repositories and Unit of Work"
    profiles -> shared "Uses repositories and Unit of Work"
    requisition -> shared "Uses repositories and Unit of Work"
    procurement -> shared "Uses repositories and Unit of Work"
    inventory -> shared "Uses repositories and Unit of Work"
    delivery -> shared "Uses repositories and Unit of Work"
    suppliers -> shared "Uses repositories and Unit of Work"
    analytics -> shared "Uses repositories and Unit of Work"
    communication -> shared "Uses repositories and Unit of Work"
  }

  views {
    systemContext buildline "SystemContext" {
      include *
      autolayout lr
    }

    container buildline "Containers" {
      include *
      autolayout lr
    }

    component api "BackendComponents" {
      include *
      autolayout lr
    }

    styles {
      element "Person" {
        shape person
        background #08427b
        color #ffffff
      }
      element "Software System" {
        background #1168bd
        color #ffffff
      }
      element "Container" {
        background #438dd5
        color #ffffff
      }
      element "Component" {
        background #85bbf0
        color #000000
      }
      element "Database" {
        shape cylinder
        background #f5da81
        color #000000
      }
    }
  }
}
