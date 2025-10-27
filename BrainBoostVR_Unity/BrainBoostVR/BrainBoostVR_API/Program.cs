using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BrainBoostVR_API.Data;
using BrainBoostVR_API.Services;
using BrainBoostVR_API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Ajout du DbContext (MySQL)
builder.Services.AddDbContext<BrainBoostDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Ajouter FirebaseService en singleton
builder.Services.AddSingleton<FirebaseService>();

// Ajouter controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware Firebase
app.UseMiddleware<FirebaseAuthMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();
