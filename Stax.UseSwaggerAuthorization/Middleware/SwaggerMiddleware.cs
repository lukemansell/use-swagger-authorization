using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using UseSwaggerAuthorization.Models;

namespace UseSwaggerAuthorization.Middleware;

public class SwaggerMiddleware
{
    private readonly RequestDelegate _requestDelegate;
    private readonly SwaggerBasicAuthorizationOptions _options;
    
    public SwaggerMiddleware(RequestDelegate requestDelegate, IOptions<SwaggerBasicAuthorizationOptions> options)
    {
        _requestDelegate = requestDelegate;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Path.StartsWithSegments("/swagger"))
        {
            var request = httpContext.Request;
            var authorization = request.Headers["Authorization"].ToString();
            if (authorization != null && authorization.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                var header = AuthenticationHeaderValue.Parse(authorization);
                var credentials = Encoding.UTF8.GetString(
                    Convert.FromBase64String(header.Parameter ?? throw new InvalidOperationException())
                    ).Split(':');
                var username = credentials[0];
                var password = credentials[1];
                
                if (username.Equals(_options.Username) && password.Equals(_options.Password)) {
                    await _requestDelegate.Invoke(httpContext).ConfigureAwait(false);
                    return;
                }
            }
            httpContext.Response.Headers["WWW-Authenticate"] = "Basic";
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        else
        {
            await _requestDelegate.Invoke(httpContext).ConfigureAwait(false);
        }
    }

}