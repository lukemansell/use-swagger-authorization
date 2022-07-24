# Swagger Authorization

## Summary
This NuGet library allows you to easily secure your swagger endpoints with a basic username and password. It reads a username and password from your appsettings.json file, which you can then use to access your swagger endpoints. 

This helps you expose them on production, without having to setup firewall rules so people can't access them for example.

## How to use

This relies on you adding a username and password in your `appsettings.json` which is picked up by this NuGet library, which then secures anything which starts with `/swagger` as an endpoint.

Step 1: Add something like this to your appsettings.json, or setup environment variables which your application then loads into your `appsettings.json`


#### appsettings.json:
```json
"SwaggerAuthorization": {
    "Username": "uid",
    "Password": "pwd"
}
```

You can also disable having this login screen occur by adding: "Disabled: true" - which can be useful if you want to add this to your `appsettings.Development.json file for example`. Eg:

#### appsettings.Development.json:
```json
"SwaggerAuthorization": {
    "Username": "uid",
    "Password": "pwd",
    "Disabled": true
}
```

#### Program.cs/startup

On default projects, your `app.UseSwagger();` is in a `if (env.IsDevelopment())` statement. You can move this out of this if statement.

Once you have done this, you want to put `app.UseSwaggerBasicAuthorization();` above `app.UseSwagger();`. Your Configure method may now look something like this.

```c#
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwaggerBasicAuthorization(); <--- newly added
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
    
app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
```

If you run your API project, you should now see a popup screen when you try to access your swagger endpoint.