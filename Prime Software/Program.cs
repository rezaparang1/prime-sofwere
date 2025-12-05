using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Interface;
using BusinessLogicLayer.Repository;
using DataAccessLayer;
using DataAccessLayer.Repository;
using HealthChecks.NpgSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prime_Software;
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
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
ConfigureSwagger();

// ================= Health Check =================
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "Database Health");

// ================= Rate Limiting =================
ConfigureRateLimiting();

// ================= CORS =================
ConfigureCors();
builder.Services.AddAuthorization();

var app = builder.Build();

// ================== Seed Database ==================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Database>();
    db.Database.Migrate(); 

    var seeder = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await seeder.SeedAsync(); 
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
            .Filter.ByIncludingOnly(Matching.FromSource("WebApi1.Controllers"))
            .WriteTo.File("Logs/WebApi/log-.txt", rollingInterval: RollingInterval.Day))
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(Matching.FromSource("BusinessLogicLayer2"))
            .WriteTo.File("Logs/Business/log-.txt", rollingInterval: RollingInterval.Day))
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(Matching.FromSource("DataAccessLayer2"))
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
    //Product
    builder.Services.AddScoped<BusinessLogicLayer.Interface.Producr.IGroupProductService, BusinessLogicLayer.Repository.Product.GroupProductService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Product.IGroupProductRepository, DataAccessLayer.Repository.Product.GroupProductRepository>();

    builder.Services.AddScoped<BusinessLogicLayer.Interface.Producr.IPriceLevelsService, BusinessLogicLayer.Repository.Product.PriceLevelsService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Product.IPriceLevelsRepository, DataAccessLayer.Repository.Product.PriceLevelRepository>();

    builder.Services.AddScoped<BusinessLogicLayer.Interface.Producr.IProductService, BusinessLogicLayer.Repository.Product.ProductService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Product.IProductRepository, DataAccessLayer.Repository.Product.ProductRepository>();

    builder.Services.AddScoped<BusinessLogicLayer.Interface.Producr.ISectionProductService, BusinessLogicLayer.Repository.Product.SectionProductService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Product.ISectionProductRepository, DataAccessLayer.Repository.Product.SectionProductRepository>();

    builder.Services.AddScoped<BusinessLogicLayer.Interface.Producr.IStoreroomProductService, BusinessLogicLayer.Repository.Product.StoreroomProductService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Product.IStoreroomProductRepository, DataAccessLayer.Repository.Product.StoreroomProductRepository>();

    builder.Services.AddScoped<BusinessLogicLayer.Interface.Producr.ITypeProductService, BusinessLogicLayer.Repository.Product.TypeProductService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Product.ITypeProductRepository, DataAccessLayer.Repository.Product.TypeProductRepository>();

    builder.Services.AddScoped<BusinessLogicLayer.Interface.Producr.IUnitProductService, BusinessLogicLayer.Repository.Product.UnitProductService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Product.IUnitProductRepository, DataAccessLayer.Repository.Product.UnitProductRepository>();

    //Fund
    builder.Services.AddScoped<BusinessLogicLayer.Interface.Fund.IFundService, BusinessLogicLayer.Repository.Fund.FundService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Fund.IFundRepository, DataAccessLayer.Repository.Fund.FundRepository>();

    builder.Services.AddScoped<BusinessLogicLayer.Interface.Fund.ICashRegisterToTheUserService, BusinessLogicLayer.Repository.Fund.CashRegisterToUserService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Fund.ICashRegisterToTheUserRepository, DataAccessLayer.Repository.Fund.CashRegisterToUserRepository>();

    builder.Services.AddScoped<BusinessLogicLayer.Interface.Fund.IWorkShiftService, BusinessLogicLayer.Repository.Fund.WorkShiftService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Fund.IWorkShiftRepository, DataAccessLayer.Repository.Fund.WorkShiftRepository>();
    //People
    builder.Services.AddScoped<BusinessLogicLayer.Interface.People.IGroupPeopleService, BusinessLogicLayer.Repository.People.GroupPeopleService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.People.IGroupPeopleRepository, DataAccessLayer.Repository.People.GroupPeopelRepository>();

    builder.Services.AddScoped<BusinessLogicLayer.Interface.People.IPeopleService, BusinessLogicLayer.Repository.People.PeopleService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.People.IPeopleRepository, DataAccessLayer.Repository.People.PeopleRepository>();

    builder.Services.AddScoped<BusinessLogicLayer.Interface.People.ITypePeopleService, BusinessLogicLayer.Repository.People.TypePeopleService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.People.ITypePeopleRepository, DataAccessLayer.Repository.People.TypePeopelRepository>();
    //Bank
    builder.Services.AddScoped<BusinessLogicLayer.Interface.Bank.IDefinitionBankService, BusinessLogicLayer.Repository.Bank.DefinitionBankService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Bank.IDefinitionBankRepository, DataAccessLayer.Repository.Bank.DefinitionBankRepository>();

    builder.Services.AddScoped<BusinessLogicLayer.Interface.Bank.IDefinitionBankAccountService, BusinessLogicLayer.Repository.Bank.DefinitionBankAccountService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Bank.IDefinitionBankAccountRepository, DataAccessLayer.Repository.Bank.DefinitionBankAccountRepository>();
    //Invoices
    builder.Services.AddScoped<BusinessLogicLayer.Interface.Invoices.IInvoicesService, BusinessLogicLayer.Repository.Invoices.InvoicesService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Invoices.IInvoicesRepository, DataAccessLayer.Repository.Invoices.InvoicesRepository>();
    //Setting
    builder.Services.AddScoped<BusinessLogicLayer.Interface.Settings.IUserService, BusinessLogicLayer.Repository.Settings.UserService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Settings.IUserRepository, DataAccessLayer.Repository.Settings.UserRepository>();

    builder.Services.AddScoped<BusinessLogicLayer.Interface.Settings.IGroupUserService, BusinessLogicLayer.Repository.Settings.GroupUserService>();
    builder.Services.AddScoped<DataAccessLayer.Interface.Settings.IGroupUserRepository, DataAccessLayer.Repository.Settings.GroupUserRepository>();

    builder.Services.Configure<DatabaseBackupSettings>(
    builder.Configuration.GetSection("DatabaseBackup")
);
    builder.Services.AddScoped<IDatabaseService, DatabaseService>();

    builder.Services.AddAutoMapper(typeof(ProductProfile));
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
}

void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
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
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}

void ConfigureMiddlewarePipeline()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        });
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
                    Checks = report.Entries.Select(e => new
                    {
                        Component = e.Key,
                        Status = e.Value.Status.ToString(),
                        Description = e.Value.Description
                    })
                })
            );
        }
    });

    app.MapControllers();
}
