using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaReservasCitas.Application.Interfaces;
using SistemaReservasCitas.Application.Services;
using SistemaReservasCitas.Application.ValidacionesServices;
using SistemaReservasCitas.Domain.Entities;
using SistemaReservasCitas.Domain.Repositories;
using SistemaReservasCitas.Infrastructure.Data;
using SistemaReservasCitasApi.Application.Interfaces;
using SistemaReservasCitasApi.Application.Services;
using SistemaReservasCitasApi.Infrastructure.Repositories;
using Serilog.Filters;
using Serilog;
using SistemaReservasCitas.Authorizations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();  // Si no usas cookies o auth, puedes eliminar esta línea
    });
});


// Agregar servicios al contenedor
builder.Services.AddControllers();
//// Agregar servicios al contenedor
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//    });


builder.Services.AddDbContext<SistemaReservasCitasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddScoped<IRepository<Cita>, SqlRepository<Cita>>();
builder.Services.AddScoped<IRepository<Turno>, SqlRepository<Turno>>();
builder.Services.AddScoped<IRepository<Usuario>, SqlRepository<Usuario>>();
builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IEmailService, EmailService>(provider =>
    new EmailService(builder.Configuration));
builder.Services.AddScoped<IConfiguracionService, ConfiguracionService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));


builder.Services.AddScoped<ReservaValidations>();
builder.Services.AddScoped<ICitaService, CitaService>();
Log.Logger = new LoggerConfiguration()
.MinimumLevel.Information()
    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
    .Filter.ByExcluding(Matching.FromSource("System"))
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// Agrego el JWT configuration:
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IInicioSesion, InicioInicioSesionService>();
builder.Services.AddScoped<IJwt, JwtProvider>(); // <- REGISTRA EL JWT

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Agregar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline de HTTP

app.UseDeveloperExceptionPage();

// ?? Swagger SIEMPRE disponible
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Reservas Citas API V1");
    c.RoutePrefix = "swagger"; // Hace que est� en https://localhost:7205/
});
app.UseCors("AllowAngularDev");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

app.Run();
