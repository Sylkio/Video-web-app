using Microsoft.EntityFrameworkCore;
using VideoWebAppApi.Data; 
using VideoWebAppApi.Interface;
using VideoWebAppApi.Service; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register AzureService (You need to implement this according to your Azure Blob Storage setup)
builder.Services.AddScoped<IAzureService, AzureService>();

// Register AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

try
{
    app.Run();
    Log.Information("API is now ready to serve files to and from Azure Cloud Storage...");
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    Log.Fatal(ex, "Unhandled Exception");
}
finally
{
    Log.Information("Azure Storage API Shutting Down...");
    Log.CloseAndFlush();
}