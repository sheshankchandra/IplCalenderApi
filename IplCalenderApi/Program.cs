var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()  // Allows requests from any domain
              .AllowAnyMethod()  // Allows GET, POST, etc.
              .AllowAnyHeader(); // Allows all headers
    });
});

var app = builder.Build();

// Enable CORS before serving static files
app.UseCors();

app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true, // Allows unknown file types
    DefaultContentType = "text/calendar" // Ensures correct MIME type
});

app.MapControllers();
app.Run();