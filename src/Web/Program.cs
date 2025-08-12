using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// DbContext (MySQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("CooperativaApiBearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Acá pegar el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "CooperativaApiBearerAuth"
                }
            },
            new List<string>()
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    setupAction.IncludeXmlComments(xmlPath);
});

// Auth (JWT)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AuthenticationService:Issuer"],
            ValidAudience = builder.Configuration["AuthenticationService:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["AuthenticationService:SecretForKey"])
            )
        };
    });

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBombaRepository, BombaRepository>();
builder.Services.AddScoped<ITanqueRepository, TanqueRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBombaService, BombaService>();
builder.Services.AddScoped<ITanqueService, TanqueService>();
builder.Services.AddScoped<IBombaRedundanciaService, BombaRedundanciaService>();
builder.Services.AddScoped<ICustomAuthenticationService, AuthenticationService>();

builder.Services.Configure<AuthenticationService.AuthenticationServiceOptions>(
    builder.Configuration.GetSection(AuthenticationService.AuthenticationServiceOptions.AuthenticationService)
);

// Middlewares
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

// CORS para conexión con el front (Vite)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
        // .AllowCredentials(); // descomentar si usás cookies/credenciales
    });
});

var app = builder.Build();

// Swagger (puede quedar siempre habilitado para dev)
app.UseSwagger();
app.UseSwaggerUI();


// app.UseHttpsRedirection();
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


// ⬇️ ORDEN CORRECTO CON ENDPOINT ROUTING
app.UseRouting();                   // 1) Routing
app.UseCors("AllowFrontendDev");    // 2) CORS (antes de auth y antes de cualquier write)

// Middleware global de excepciones (después de CORS)
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthentication();            // 3) Auth
app.UseAuthorization();             // 4) AuthZ

app.MapControllers();               // 5) Endpoints

// Inicialización de redundancia
using (var scope = app.Services.CreateScope())
{
    var redundanciaService = scope.ServiceProvider.GetRequiredService<IBombaRedundanciaService>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        var resultado = await redundanciaService.VerificarYMantenerRedundanciaAsync();
        logger.LogInformation("Sistema de redundancia inicializado: {Descripcion}", resultado.Descripcion);

        if (resultado.EstadoCritico)
        {
            logger.LogCritical("ATENCIÓN: Sistema iniciado en estado crítico - {Descripcion}", resultado.Descripcion);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al inicializar el sistema de redundancia");
    }
}

app.Run();
