# 1. Dasturni ishga tushirish uchun asosiy .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# 2. Loyihani build qilish uchun .NET 8 SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Csproj faylini nusxalash va kutubxonalarni yuklab olish (restore)
COPY ["src/FirstDockerProj/FirstDockerProj.csproj", "src/FirstDockerProj/"]
RUN dotnet restore "./src/FirstDockerProj/FirstDockerProj.csproj"

# Qolgan barcha fayllarni nusxalash va build qilish
COPY . .
WORKDIR "/src/src/FirstDockerProj"
RUN dotnet build "./FirstDockerProj.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Dasturni publish qilish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FirstDockerProj.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Yakuniy image'ni yig'ish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FirstDockerProj.dll"]







-----------------------------------------------------------------------------------------------------------------------------------












Agar sening arxitekturang:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
Database
```

bo'lsa, **Redis'ni Controller yoki Repository ichida ishlatish tavsiya etilmaydi**.

Eng to'g'ri joy — **Service** qatlami.

Sababi:

* **Repository** faqat database bilan ishlaydi.
* **Service** business logic va cache ni boshqaradi.
* **Controller** faqat request/response bilan shug'ullanadi.

---

# Misol

Faraz qilaylik `Course` entity bor.

## Repository

```csharp
public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);
}
```

```csharp
public class CourseRepository : ICourseRepository
{
    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _context.Courses
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
```

Repository Redis haqida **hech narsa bilmaydi**.

---

# Service

```csharp
public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;
    private readonly IDistributedCache _cache;

    public CourseService(
        ICourseRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<CourseDto?> GetByIdAsync(Guid id)
    {
        var cacheKey = $"course:{id}";

        // 1. Redisdan o'qish
        var cached = await _cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonSerializer.Deserialize<CourseDto>(cached);
        }

        // 2. Databasega borish
        var course = await _repository.GetByIdAsync(id);

        if (course == null)
            return null;

        var dto = new CourseDto
        {
            Id = course.Id,
            Name = course.Name
        };

        // 3. Redisga yozish

        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(dto),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(10)
            });

        return dto;
    }
}
```

---

# Controller

Controller umuman o'zgarmaydi.

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> Get(Guid id)
{
    var course = await _courseService.GetByIdAsync(id);

    if (course == null)
        return NotFound();

    return Ok(course);
}
```

---

# Request Flow

### Birinchi request

```text
Browser

↓

Controller

↓

Service

↓

Redis ❌ (topilmadi)

↓

Repository

↓

SQL Server

↓

Service

↓

Redisga saqlaydi

↓

Controller

↓

Browser
```

---

### Ikkinchi request

```text
Browser

↓

Controller

↓

Service

↓

Redis ✅

↓

Controller

↓

Browser
```

Bu safar SQL Serverga umuman bormaydi.

---

# Professional loyiha

Katta loyihalarda odatda `IDistributedCache` ni to'g'ridan-to'g'ri Service ichida ishlatishmaydi. Buning o'rniga alohida wrapper yoziladi:

```text
Controller

↓

Service

↓

ICacheService

↓

Redis

↓

Repository

↓

Database
```

Masalan:

```csharp
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);

    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan expiration);

    Task RemoveAsync(string key);
}
```

Shunda `CourseService` ichida kod juda toza bo'ladi:

```csharp
var course = await _cacheService.GetAsync<CourseDto>(cacheKey);

if (course != null)
    return course;

course = await _repository.GetByIdAsync(id);

// ...

await _cacheService.SetAsync(cacheKey, course, TimeSpan.FromMinutes(10));
```

Bu usul test qilishni osonlashtiradi va Redis implementatsiyasini keyinchalik almashtirishni ham yengillashtiradi.

**Men aynan production darajasidagi `ICacheService` + `RedisCacheService` implementatsiyasini yozib bera olaman.** Bu ko'p .NET loyihalarida qo'llaniladigan standart yondashuv hisoblanadi.



------------------------------------------------------------------------------------------------
































