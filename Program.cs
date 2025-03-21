using AutoMapper;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Astravon.Configuration.Context;
using Astravon.HUb;
using Astravon.Mapping;
using Astravon.Modules.Posts.Application.Adapter;
using Astravon.Modules.User.Application.Adapter;
using Astravon.Modules.User.Application.Port;
using Astravon.Modules.User.Domain.IRepository;
using Astravon.Modules.User.Infraestructure.Presenter;
using Astravon.Modules.User.Infraestructure.Repository;

// Asegúrate de incluir el espacio de nombres de AutoMapper

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR(); 
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

builder.Services.AddScoped<IPostInputPort, PostAdapter>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostOutPort, PostPresenter>();

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

app.MapHub<PostHub>("/postHub");


app.Run();