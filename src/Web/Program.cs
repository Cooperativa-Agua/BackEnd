using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Web.Middlewares;


var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext con MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("CooperativaApiBearerAuth", new OpenApiSecurityScheme() //Esto va a permitir usar swagger con el token.
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
                    Id = "CooperativaApiBearerAuth" } //Tiene que coincidir con el id seteado arriba en la definición
                }, new List<string>() }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    setupAction.IncludeXmlComments(xmlPath);

});

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AuthenticationService:SecretForKey"]))
        };
    });


#region Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBombaRepository, BombaRepository>();
builder.Services.AddScoped<ITanqueRepository, TanqueRepository>();
#endregion



#region services

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBombaService, BombaService>();
builder.Services.AddScoped<ITanqueService, TanqueService>();
builder.Services.AddScoped<IBombaRedundanciaService, BombaRedundanciaService>();
builder.Services.AddScoped<ICustomAuthenticationService, AuthenticationService>();

// Configuración de las opciones de autenticación
// Configuración de las opciones de autenticación
builder.Services.Configure<AuthenticationService.AuthenticationServiceOptions>(
    builder.Configuration.GetSection(AuthenticationService.AuthenticationServiceOptions.AuthenticationService));


// Registro de servicios


//MERCADOPAGO


builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
#endregion

//CORS para conexion con el front
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendDev", policy =>
    {
        policy
          .WithOrigins("http://localhost:5173", "https://localhost:5173")
          .AllowAnyHeader()
          .AllowAnyMethod();
        // .AllowCredentials(); // solo si usas cookies
    });
});


var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}


// app.UseStaticFiles();  no lo necesito mas

app.UseHttpsRedirection();

app.UseCors("AllowFrontendDev");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

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
