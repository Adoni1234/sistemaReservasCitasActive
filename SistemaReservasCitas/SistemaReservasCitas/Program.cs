using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SistemaReservasCitas.Application.Interfaces;
using SistemaReservasCitas.Application.Services;
using SistemaReservasCitas.Application.ValidacionesServices;
using SistemaReservasCitas.Domain.Entities;
using SistemaReservasCitas.Domain.Repositories;
using SistemaReservasCitas.Infrastructure.Data;
using SistemaReservasCitasApi.Application.Interfaces;
using SistemaReservasCitasApi.Application.Services;
using SistemaReservasCitasApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));

builder.Services.AddScoped<ReservaValidations>();
builder.Services.AddScoped<ICitaService, CitaService>();

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
    c.RoutePrefix = "swagger"; // Hace que esté en https://localhost:7205/
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
