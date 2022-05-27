using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles()
    .UseStaticFiles();

app.Run();
