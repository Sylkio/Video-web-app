using Microsoft.EntityFrameworkCore;
using VideoWebAppApi.Data;
using VideoWebAppApi.Interface;
using VideoWebAppApi.Service;
using Microsoft.OpenApi.Models;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VideoWebApp API",
        Version = "v1",
        Description = "API for Video Web Application"
    });
    // Add more Swagger configuration if needed
});

// Register AzureService
builder.Services.AddScoped<IAzureService, AzureService>();

// Set up CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policyBuilder => policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Register AppDbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register BlobServiceClient for Azure Blob Storage
var blobStorageConnectionString = builder.Configuration.GetValue<string>("BlobConnectionString");
builder.Services.AddSingleton(new BlobServiceClient(blobStorageConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VideoWebApp API V1");
        c.RoutePrefix = string.Empty; // Swagger UI at the app's root in development
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
