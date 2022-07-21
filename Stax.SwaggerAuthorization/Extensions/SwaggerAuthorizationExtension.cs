using System;
using Microsoft.AspNetCore.Builder;
using SwaggerAuthorization.Middleware;

namespace SwaggerAuthorization.Extensions;

public static class SwaggerAuthorizationExtension
{
    /// <summary>
    /// A basic authorization middleware, which allows you to protect your swagger endpoint by a username and password.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <param name="authorizationOptions">SwaggerBasicAuthorizationOptions which contains the username and password which
    /// you want to use to protect the swagger endpoint.</param>
    /// <returns></returns>
    public static IApplicationBuilder UseSwaggerBasicAuthorization
        (this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<SwaggerAuthorizationMiddleware>();
    }

    private static IApplicationBuilder UseSwaggerIAPAuthorization
        (this IApplicationBuilder applicationBuilder)
    {
        throw new NotImplementedException();
    }
}