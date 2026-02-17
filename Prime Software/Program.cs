using BusinessLogicLayer.Interface;
using BusinessLogicLayer.Interface.Customer_Club;
using BusinessLogicLayer.Interface.Fund;
using BusinessLogicLayer.Interface.Invoices;
using BusinessLogicLayer.Interface.People;
using BusinessLogicLayer.Interface.Producr;
using BusinessLogicLayer.Interface.Settings;
using BusinessLogicLayer.Repository;
using BusinessLogicLayer.Repository.Customer_Club;
using BusinessLogicLayer.Repository.Fund;
using BusinessLogicLayer.Repository.Invoices;
using BusinessLogicLayer.Repository.People;
using BusinessLogicLayer.Repository.Product;
using BusinessLogicLayer.Repository.Settings;
using DataAccessLayer;
using DataAccessLayer.Interface;
using DataAccessLayer.Interface.Customer_Club;          // ???? IUnitOfWork ? ?????????????? ??????
using DataAccessLayer.Interface.Fund;
using DataAccessLayer.Interface.Fund_and_Bank;
using DataAccessLayer.Interface.Product;
using DataAccessLayer.Interface.Settings;
using DataAccessLayer.Repository;
using DataAccessLayer.Repository.Bank;
using DataAccessLayer.Repository.Customer_Club;        // ???? UnitOfWork ? ?????????????? ??????
using DataAccessLayer.Repository.Fund;
using DataAccessLayer.Repository.Product;
using DataAccessLayer.Repository.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prime_Software;
using Prime_Software.Controllers;
using Prime_Software.DTO;
using Serilog;
using Serilog.Filters;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ================= Logger =================
ConfigureLogger();

var connectionString = configuration.GetConnectionString("DefaultConnection");

// ================= Database =================
builder.Services.AddDbContext<Database>(options =>
    options.UseNpgsql(connectionString));

// DbInitializer 
builder.Services.AddScoped<DbInitializer>();

// ================= JWT Auth =================
ConfigureJwtAuthentication();

// ================= DI =================
ConfigureDependencies();

// ================= API Services =================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
ConfigureSwagger();

// ================= Health Check =================
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "Database Health");

// ================= Rate Limiting =================
ConfigureRateLimiting();

// ================= CORS =================
ConfigureCors();

// ================= AutoMapper =================
ConfigureAutoMapper();

builder.Services.AddAuthorization();

var app = builder.Build();

// ================== Seed Database ==================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Database>();
    db.Database.Migrate();

    var seeder = scope.ServiceProvider.GetRequiredService<DbInitializer>();

    // ??? ?? ???? Development seed ???? ???
    if (app.Environment.IsDevelopment())
    {
        await seeder.SeedAsync();
    }
}
// ================= Middleware Pipeline =================
ConfigureMiddlewarePipeline();

app.Run();

// ================= Methods =================
void ConfigureLogger()
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(Matching.FromSource("Prime_Software.Controllers"))
            .WriteTo.File("Logs/WebApi/log-.txt", rollingInterval: RollingInterval.Day))
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(Matching.FromSource("BusinessLogicLayer"))
            .WriteTo.File("Logs/Business/log-.txt", rollingInterval: RollingInterval.Day))
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(Matching.FromSource("DataAccessLayer"))
            .WriteTo.File("Logs/DataAccess/log-.txt", rollingInterval: RollingInterval.Day))
        .CreateLogger();

    builder.Host.UseSerilog();
}

void ConfigureJwtAuthentication()
{
    var jwtSection = configuration.GetSection("Jwt");
    var secretKey = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    });
}

void ConfigureDependencies()
{
    // ================= Repository (Generic) =================
    // ? ??? ?????????? ????? ???? (???? ???? ???? LogService ? ...)
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

    // ================= UnitOfWork =================
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();
    builder.Services.AddScoped<ISalesReturnService, SalesReturnService>();
    builder.Services.AddScoped<IPurchaseReturnService, PurchaseReturnService>();
    // ================= Generic Service (BLL) =================
    builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

    // ================= Log Service =================
    builder.Services.AddScoped<ILogService, LogService>();

    // ================= User & Auth Services =================
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddHttpContextAccessor();

    // ================= Product Services =================
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IProductService, ProductService>();

    // ================= File Upload Services =================
    builder.Services.AddScoped<IFileStorageService, FileStorageService>();

    // ================= People Services =================
    builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();
    builder.Services.AddScoped<IPeopleService, PeopleService>();

    // ================= Fund Services =================
    builder.Services.AddScoped<IFundRepository, FundRepository>();
    builder.Services.AddScoped<IFundService, FundService>();

    // ================= Bank Services =================
    builder.Services.AddScoped<IDefinitionBankService, DefinitionBankService>();
    builder.Services.AddScoped<IDefinitionBankAccountRepository, DefinitionBankAccountRepository>();
    builder.Services.AddScoped<IDefinitionBankAccountService, DefinitionBankAccountService>();

    // ================= Settings & Definition Services =================
    builder.Services.AddScoped<BusinessLogicLayer.Interface.Product.IGroupProductService, GroupProductService>();
    builder.Services.AddScoped<IGroupPeopleService, GroupPeopelService>();
    builder.Services.AddScoped<IGroupUserService, GroupUserService>();
    builder.Services.AddScoped<ITypePeopleService, TypePeopelService>();
    builder.Services.AddScoped<ITypeProductService, TypeProdudtService>();
    builder.Services.AddScoped<ISectionProductService, SectionProductService>();
    builder.Services.AddScoped<IUnitProductService, UnitProdudtService>();
    builder.Services.AddScoped<BusinessLogicLayer.Interface.Product.IPriceLevelsService, PricelevelService>();

    // ================= Customer Club Services =================
    builder.Services.AddScoped<ICustomerService, CustomerService>();
    builder.Services.AddScoped<IWalletService, WalletService>();
    builder.Services.AddScoped<IClubDiscountService, ClubDiscountService>();
    builder.Services.AddScoped<IPublicDiscountService, PublicDiscountService>();
    builder.Services.AddScoped<IInvoiceService, InvoiceService>();

    // ================= Reminder Service =================
    builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
    // builder.Services.AddScoped<IReminderService, ReminderService>();

    builder.Services.AddMemoryCache(); // ????? MemoryCache ?? ????? ????
    builder.Services.AddScoped<ITempInvoiceService, TempInvoiceService>();
    // ================= Storeroom Product Service =================
    builder.Services.AddScoped<IStoreroomProductRepository, StoreroomProductRepository>();
    builder.Services.AddScoped<IStoreroomProductService, StoreroomProductService>();

    builder.Services.AddSignalR();
    // ================= Form Options for File Upload =================
    builder.Services.Configure<FormOptions>(options =>
    {
        options.ValueLengthLimit = int.MaxValue;
        options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50MB
        options.MultipartHeadersLengthLimit = int.MaxValue;
    });
}

void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Prime Software API",
            Version = "v1",
            Description = "API for Prime Software - ERP System"
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.\n\nEnter 'Bearer' [space] and then your token.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });
}

void ConfigureRateLimiting()
{
    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        options.AddPolicy("api", context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "global",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 10
                }));
    });
}

void ConfigureCors()
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:3000",
                    "http://localhost:5173",
                    "https://yourdomain.com"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });
}

void ConfigureAutoMapper()
{
    builder.Services.AddAutoMapper(typeof(ProductProfile));
}

void ConfigureMiddlewarePipeline()
{
    app.UseStaticFiles();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prime Software API v1");
            c.RoutePrefix = "api-docs";
        });

        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("AllowFrontend");
    app.UseRateLimiter();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(
                System.Text.Json.JsonSerializer.Serialize(new
                {
                    Status = report.Status.ToString(),
                    Timestamp = DateTime.UtcNow,
                    Checks = report.Entries.Select(e => new
                    {
                        Component = e.Key,
                        Status = e.Value.Status.ToString(),
                        Description = e.Value.Description,
                        Duration = e.Value.Duration.TotalMilliseconds
                    })
                })
            );
        }
    });

    app.Use(async (context, next) =>
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Global error occurred");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = "An internal server error occurred",
                Details = app.Environment.IsDevelopment() ? ex.Message : null
            });
        }
    });

    app.MapControllers();
    app.MapFallback(() => Results.Problem("Endpoint not found", statusCode: 404));
}