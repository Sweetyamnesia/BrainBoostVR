using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using BrainBoostVR_API.Services;
using System;

namespace BrainBoostVR_API.Middleware
{
    public class FirebaseAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public FirebaseAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, FirebaseService firebaseService)
        {
            try
            {
                // Vérifie la présence du header Authorization
                if (!context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Authorization header manquant");
                    return;
                }

                var tokenHeader = context.Request.Headers["Authorization"].ToString();
                if (!tokenHeader.StartsWith("Bearer "))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Format Authorization invalide");
                    return;
                }

                var token = tokenHeader.Substring("Bearer ".Length).Trim();

                // Vérification du token via Firebase
                var uid = await firebaseService.VerifyTokenAsync(token);
                context.Items["FirebaseUid"] = uid;

                Console.WriteLine($"[Middleware] Token Firebase validé ✅ UID : {uid}");

                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(ex.Message);
                Console.WriteLine($"[Middleware] Token invalide ❌ : {ex.Message}");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Erreur interne du serveur");
                Console.WriteLine($"[Middleware] Erreur interne : {ex}");
            }
        }
    }
}
