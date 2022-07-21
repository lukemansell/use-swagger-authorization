namespace UseSwaggerAuthorization.Models;

/// <summary>
/// Class which contains a username and password which can be used to protect a swagger endpoint
/// </summary>
public class SwaggerBasicAuthorizationOptions
{
    // Username value which you wish to use
    public string Username { get; set; }

    // Password value which you wish to use
    public string Password { get; set; }
}