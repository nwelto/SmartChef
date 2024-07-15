using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

public class FirebaseAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public FirebaseAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                    context.Items["Uid"] = decodedToken.Uid;
                }
                catch
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            }
        }

        await _next(context);
    }
}

