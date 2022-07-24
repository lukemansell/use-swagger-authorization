using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SwaggerAuthorization.Constants;

namespace SwaggerAuthorization.Middleware;

public class SwaggerAuthorizationMiddleware
{
    private readonly RequestDelegate _requestDelegate;
    private readonly IConfiguration _configuration;
    
    public SwaggerAuthorizationMiddleware(RequestDelegate requestDelegate, IConfiguration configuration)
    {
        _requestDelegate = requestDelegate;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (!_configuration.GetValue<bool>(ConfigurationConstants.SwaggerAuthorizationDisable) 
            && httpContext.Request.Path.StartsWithSegments("/swagger"))
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
                
                if (username.Equals(_configuration.GetValue<string>(ConfigurationConstants.SwaggerAuthorizationUsername)) 
                    && password.Equals(_configuration.GetValue<string>(ConfigurationConstants.SwaggerAuthorizationPassword))) {
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