using Microsoft.EntityFrameworkCore;
using WebApiFristProject;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ? Configure Serilog to log into Console and File
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Logs to console
    .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day) // Logs to file (new file every day)
    .MinimumLevel.Information() // Set minimum log level
    .CreateLogger();

// ? Replace default logging with Serilog
builder.Host.UseSerilog();

// ? Add services to the container
builder.Services.AddControllers();

// ? Configure Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

// ? Enable Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ? Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
