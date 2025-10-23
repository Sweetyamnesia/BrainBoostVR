using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BrainBoostVR_API.Data;

var builder = WebApplication.CreateBuilder(args);

// Connexion à la base de données MySQL
builder.Services.AddDbContext<BrainBoostDbContext>(options => options.UseMySql(
builder.Configuration.GetConnectionString("DefaultConnection"),
ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
)
);

// Ajouter les contrôleurs + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
//app.MapGet("/", () => "Hello World!");
//app.MapGet("/api/hello", () => "Hello from /api/hello");

app.Run();
