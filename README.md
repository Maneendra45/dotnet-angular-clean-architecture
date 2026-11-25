# Enterprise Rule Engine & Tax Calculation Reference Architecture (.NET 8 + Angular)

A clean-architecture full-stack reference implementation demonstrating enterprise design patterns, decoupled business rules, RESTful API design, global exception handling, unit testing, and reactive Angular integration.

> **Note:** Production enterprise systems built during my tenure at EY GDS, Bureau Veritas, and Wipro reside in private corporate repositories. This repository serves as a clean-room architectural sample illustrating my standard engineering practices.

---

## Architecture & Design Patterns

```text
┌──────────────────────────────────────────────────────────────────────┐
│                        Angular Frontend (SPA)                        │
│   Standalone Components • Reactive Forms • Typed HttpClient • RxJS   │
└──────────────────────────────────┬───────────────────────────────────┘
                                   │ HTTPS / JSON (REST)
                                   ▼
┌──────────────────────────────────────────────────────────────────────┐
│                     ASP.NET Core 8 Web API Layer                     │
│   Controllers • Global Exception Middleware (ProblemDetails) • DI    │
└──────────────────────────────────┬───────────────────────────────────┘
                                   │
                                   ▼
┌──────────────────────────────────────────────────────────────────────┐
│                          Application Layer                           │
│     Strategy Pattern (Rule Evaluators) • Factory Pattern • DTOs      │
└───────────────────┬──────────────────────────────┬───────────────────┘
                    │                              │
                    ▼                              ▼
┌───────────────────────────────────┐  ┌───────────────────────────────┐
│           Domain Layer            │  │     Infrastructure Layer      │
│   Entities • Core Interfaces      │  │   EF Core • Repository        │
│   Domain Exceptions               │  │   SQL Server / In-Memory DB   │
└───────────────────────────────────┘  └───────────────────────────────┘
```

### Key Patterns Demonstrated
* **Strategy & Factory Patterns:** Decouples jurisdiction-specific tax/compliance calculation rules (`ProgressiveSlabStrategy`, `FlatCorporateTaxStrategy`) from API controllers so new rules can be added without modifying existing orchestration logic (Open/Closed Principle).
* **Repository Pattern & EF Core:** Abstracts persistence behind `IRuleRepository`, using `AsNoTracking()` for read queries and clean entity-to-DTO mapping.
* **Global Exception Handling Middleware:** Centralizes error trapping and returns standardized RFC 7807 `ProblemDetails` responses.
* **Unit Testing (xUnit):** Isolates business rule verification with deterministic test cases.
* **Reactive Angular Integration:** Uses strict TypeScript interfaces, `ReactiveFormsModule`, and RxJS error handling (`catchError`).

---

## Project Structure

```text
dotnet-angular-clean-architecture/
├── backend/
│   ├── src/
│   │   ├── TaxRulesEngine.Domain/          # Entities, Interfaces, Domain Exceptions
│   │   ├── TaxRulesEngine.Application/     # DTOs, Strategy & Factory Implementations
│   │   ├── TaxRulesEngine.Infrastructure/  # EF Core DbContext & Repository
│   │   └── TaxRulesEngine.Api/             # Controllers, Middleware, Program.cs
│   └── tests/
│       └── TaxRulesEngine.Tests/           # xUnit Unit Tests
└── frontend/
    └── tax-rules-ui/                       # Angular Standalone UI & Typed API Service
```

---

## API Endpoints

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/v1/taxcalculations/rules` | Retrieves active jurisdiction rule configurations |
| `POST` | `/api/v1/taxcalculations/evaluate` | Evaluates taxable income against the selected strategy |

### Sample Request (`POST /api/v1/taxcalculations/evaluate`)
```json
{
  "jurisdictionCode": "US-FED",
  "grossIncome": 125000,
  "deductions": 15000
}
```

---

## Quick Start

### 1. Run the .NET 8 Web API & Unit Tests
```bash
cd backend
dotnet test
dotnet run --project src/TaxRulesEngine.Api
```
Swagger UI is available at `http://localhost:5000/swagger`.

### 2. Run the Angular Frontend
```bash
cd frontend/tax-rules-ui
npm install
ng serve
```
Navigate to `http://localhost:4200`.
