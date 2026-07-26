# ToDoList Backend — Completion Plan (A → Z)

## 1. Cleanup & Housekeeping
- [ ] Delete placeholder files `Api/Middlewares/a.cs` and `Application/Validators/a.cs`
- [ ] Fix `Program.cs`: replace `if (true || app.Environment.IsDevelopment())` with a proper environment check
- [ ] Add `.gitignore` to exclude `bin/`, `obj/`, and other build artifacts
- [ ] Standardize namespaces (project is `ToDoList.Persistence` but assembly/namespaces use `ToDoList.Infrastructure` — pick one)
- [ ] Add a `README.md` with setup, run, and migration instructions

## 2. ToDoItem CRUD (core feature — currently empty)
- [ ] Create `ToDoItemCreateDto` and `ToDoItemUpdateDto`
- [ ] Add a `UserConverter`-style converter/mapper for ToDoItem ↔ DTOs
- [ ] Define full `IToDoItemService` interface (Create, GetById, GetAll, Update, Delete, ToggleComplete)
- [ ] Implement `ToDoItemService` with repository + `ICurrentUserService` (scope items to the logged-in user)
- [ ] Implement soft delete (use existing `IsDeleted` / `DeletedAt` fields)
- [ ] Set `CompletedAt` when an item is marked complete
- [ ] Build out `ToDoItemsController`: POST, GET (list), GET by id, PUT, DELETE, PATCH complete
- [ ] Add `[Authorize]` to the controller and enforce ownership on every action

## 3. Querying, Filtering & Pagination
- [ ] Add pagination (page/pageSize) to the ToDoItem list endpoint
- [ ] Add filtering (by IsCompleted, Priority, DueDate range)
- [ ] Add sorting (by CreatedAt, DueDate, Priority)
- [ ] Create a reusable `PagedResult<T>` DTO

## 4. Validation
- [ ] Add FluentValidation NuGet package
- [ ] Validators for `RegisterDto` (email format, password strength, username rules)
- [ ] Validators for `LoginDto`, `RefreshTokenRequestDto`
- [ ] Validators for `ToDoItemCreateDto` / `ToDoItemUpdateDto`
- [ ] Register validators in DI and enable automatic validation

## 5. Error Handling
- [ ] Implement a global exception-handling middleware
- [ ] Map custom exceptions (`NotFoundException`, `UnauthorizedException`, `ValidationException`, `EmailAlreadyExistsException`, `UserNotFoundException`) to proper HTTP status codes
- [ ] Return a consistent error response model (ProblemDetails)
- [ ] Replace raw `throw new Exception(...)` in `RegisterAsync` with `EmailAlreadyExistsException`
- [ ] Standardize on `UnauthorizedException` in `LoginAsync` (currently uses `UnauthorizedAccessException`)
- [ ] Register the middleware in `Program.cs`

## 6. API Response Consistency
- [ ] Wrap controller returns in `ActionResult<T>` with correct status codes (201 on create, 204 on delete, etc.)
- [ ] Standardize auth endpoint responses (Register currently returns a raw `long`)

## 7. Notifications (currently empty stub)
- [ ] Define the `INotificationService` interface (SendEmailAsync, etc.)
- [ ] Implement `EmailNotificationService` (SMTP config from appsettings)
- [ ] Add email settings section to `appsettings.json`
- [ ] Register the notification service in DI

## 8. Email Confirmation Flow (field exists, unused)
- [ ] Generate an email-confirmation token on register
- [ ] Add a "confirm email" endpoint that sets `EmailConfirmed = true`
- [ ] Send the confirmation email via the notification service
- [ ] Optionally block login until email is confirmed

## 9. Reminders / Background Jobs (ReminderAt field exists, unused)
- [ ] Add a background worker (HostedService / Hangfire) to scan due `ReminderAt` items
- [ ] Send reminder notifications for tasks that are due
- [ ] Make reminder scheduling configurable

## 10. AI Features (services are empty stubs)
- [ ] Define the `IAIService` interface (e.g. suggest tasks, summarize, prioritize)
- [ ] Implement `OpenAIService` (call OpenAI API, read key from config)
- [ ] Implement `AntropicService` (Anthropic API) — fix typo "Antropic" → "Anthropic"
- [ ] Choose the active provider via configuration
- [ ] Add AI API keys/settings to `appsettings.json` + user-secrets
- [ ] Register the chosen AI service in DI
- [ ] Add controller endpoint(s) exposing the AI features

## 11. Caching (Redis configured, unused)
- [ ] Wire up the Redis connection from `appsettings.json`
- [ ] Add distributed caching for read-heavy endpoints (e.g. task lists)
- [ ] Add cache invalidation on create/update/delete

## 12. Authorization & Security
- [ ] Apply role-based authorization using the existing `UserRole` enum
- [ ] Move the JWT `SecurityKey` and all secrets out of `appsettings.json` into user-secrets / environment variables
- [ ] Use `DateTime.UtcNow` consistently (AuthService mixes `DateTime.Now` and `UtcNow`)
- [ ] Add a repository method to purge expired/revoked refresh tokens
- [ ] Configure CORS policy
- [ ] Add rate limiting on auth endpoints

## 13. Repository Enhancements
- [ ] Add `GetByIdAsync` and async helpers to `IBaseRepository` / `BaseRepository`
- [ ] Add an optional global query filter for soft-deleted entities

## 14. Configuration & Documentation
- [ ] Configure Swagger with JWT Bearer auth support (Authorize button)
- [ ] Add XML comments / operation summaries to endpoints
- [ ] Verify all settings bind correctly (`JwtSettings`, `DatabaseSettings`)
- [ ] Add health check endpoint(s)

## 15. Logging & Observability
- [ ] Add structured logging (Serilog)
- [ ] Log requests, errors, and key auth events

## 16. Testing
- [ ] Add a unit test project (xUnit)
- [ ] Unit tests for `AuthService` and `ToDoItemService`
- [ ] Add an integration test project (WebApplicationFactory + in-memory/test DB)
- [ ] Integration tests for auth and ToDoItem endpoints

## 17. Database & Migrations
- [ ] Verify migrations apply cleanly against a fresh database
- [ ] Add a seed for a default admin user / roles
- [ ] Confirm indexes on frequently queried columns (UserId, Token, Email)

## 18. Deployment
- [ ] Add a `Dockerfile`
- [ ] Add `docker-compose.yml` (API + SQL Server + Redis)
- [ ] Externalize all environment-specific config
- [ ] Set up a CI pipeline (build + test)

## 19. Final Verification
- [ ] `dotnet build` the full solution with zero warnings/errors
- [ ] Run all tests green
- [ ] Manual smoke test of every endpoint via Swagger / `.http` file
