using BadogASP_razor_pages_.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(); // Razor Pages
builder.Services.AddControllers(); // Add support for APIs (e.g., ProductController)
builder.Services.AddEndpointsApiExplorer(); // Enable Swagger support for API documentation
builder.Services.AddSwaggerGen(); // Add Swagger for API testing
builder.Services.AddMemoryCache();

// Add database context with PostgreSQL
builder.Services.AddDbContext<BadogContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // Ensure Npgsql is installed

// Add dependency injection for services (if needed)
// builder.Services.AddScoped<IYourService, YourServiceImplementation>();

// Configure CORS policy (if needed for cross-origin requests)
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});

builder.Services.AddDbContext<BadogContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null
            );
        }));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts(); // Use HSTS in non-development environments
}
else
{
	app.UseSwagger(); // Enable Swagger in development
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
	});
}

app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseStaticFiles(); // Serve static files from wwwroot by default

// Serve files from the Public folder
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		Path.Combine(Directory.GetCurrentDirectory(), "Public")),
	RequestPath = "/Public"
});

app.UseRouting(); // Configure routing

app.UseCors("AllowAll"); // Apply CORS policy

app.UseAuthorization(); // Authorization middleware (can be extended with authentication)

app.MapRazorPages(); // Map Razor Pages endpoints
app.MapControllers(); // Map API controllers (e.g., ProductController)

// Add health checks (optional)
app.MapGet("/health", () => Results.Ok("Healthy"));

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
    await next();
});


app.Run();
