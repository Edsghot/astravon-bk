using AutoMapper;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Astravon.Configuration.Context;
using Astravon.HUb;
using Astravon.Mapping;
using Astravon.Modules.User.Application.Adapter;
using Astravon.Modules.User.Application.Port;
using Astravon.Modules.User.Domain.IRepository;
using Astravon.Modules.User.Infraestructure.Presenter;
using Astravon.Modules.User.Infraestructure.Repository;

// Asegúrate de incluir el espacio de nombres de AutoMapper

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MySqlContext>();
builder.Services.AddMapster();
MappingConfig.RegisterMappings();

builder.Services.AddScoped<IUserInputPort, UserAdapter>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserOutPort, UserPresenter>();

// Configuración de CORS para permitir cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSignalR();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar CORS para todos los orígenes
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.MapHub<AstravonHub>("/astravonHub");

app.Run();