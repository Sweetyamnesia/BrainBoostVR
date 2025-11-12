using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BrainBoostVR_API.Data;
using BrainBoostVR_API.Services;
using BrainBoostVR_API.Middleware;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Initialisation Firebase Admin SDK
if (FirebaseApp.DefaultInstance == null)
{
    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile("Config/brainboostvr-firebase-adminsdk-fbsvc-841d9189b7.json")
    });
    Console.WriteLine("[Program] Firebase Admin initialisé ✅");
}

// 🔹 Ajout du DbContext
builder.Services.AddDbContext<BrainBoostDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// 🔹 Ajouter FirebaseService en singleton
builder.Services.AddSingleton<FirebaseService>();

// 🔹 Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 Authentification JWT Firebase
var firebaseProjectId = "brainboostvr";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
            ValidateAudience = true,
            ValidAudience = firebaseProjectId,
            ValidateLifetime = true
        };
    });

// 🔹 Autorisation
builder.Services.AddAuthorization();

var app = builder.Build();

// 🔹 Swagger uniquement en dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 Pipeline
app.UseAuthentication();
app.UseAuthorization();

// 🔹 Middleware Firebase personnalisé
app.UseMiddleware<FirebaseAuthMiddleware>();

// 🔹 Map controllers
app.MapControllers();

// 🔹 URL API
app.Urls.Add("http://0.0.0.0:5286");

app.Run();
