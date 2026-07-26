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

## 7. Authorization & Security
- [ ] Apply role-based authorization using the existing `UserRole` enum
- [ ] Move the JWT `SecurityKey` and all secrets out of `appsettings.json` into user-secrets / environment variables
- [ ] Use `DateTime.UtcNow` consistently (AuthService mixes `DateTime.Now` and `UtcNow`)
- [ ] Add a repository method to purge expired/revoked refresh tokens
- [ ] Configure CORS policy
- [ ] Add rate limiting on auth endpoints

## 8. Repository Enhancements
- [ ] Add `GetByIdAsync` and async helpers to `IBaseRepository` / `BaseRepository`
- [ ] Add an optional global query filter for soft-deleted entities

## 9. Logging & Observability
- [ ] Add structured logging (Serilog)
- [ ] Log requests, errors, and key auth events

## 10. Database & Migrations
- [ ] Verify migrations apply cleanly against a fresh database
- [ ] Add a seed for a default admin user / roles
- [ ] Confirm indexes on frequently queried columns (UserId, Token, Email)

## 11. Final Verification
- [ ] `dotnet build` the full solution with zero warnings/errors
