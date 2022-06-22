using PostsService.Data;
using PostsService.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services
    .AddControllers()
    .AddDapr()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<IStatsRepository, StatsRepository>();
builder.Services.AddSingleton<IUrlService, UrlService>();
builder.Services.AddSingleton<IPostsPublishService, DaprPostsPublishService>();

// EF Core
if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("[INFO] Using In-Memory-Database");
    builder.Services.AddDbContext<AppDbContext>(
        options => options.UseInMemoryDatabase(
            databaseName: "TestDB"
        )
    );
}
else
{
    Console.WriteLine("[INFO] Using Postgres-Database");
    var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
    builder.Services.AddDbContext<AppDbContext>(
        options => options.UseNpgsql(connectionString)
    );
}

// PostgreSQL Container:
// docker run -it --env POSTGRES_USER=root --env POSTGRES_PASSWORD=root --env POSTGRES_DB=posts_db -p 5432:5432 postgres:latest

var app = builder.Build();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Run migration in production mode
if (!app.Environment.IsDevelopment())
{
    Console.WriteLine("Migrating database...");

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCloudEvents();
app.MapControllers();
app.MapSubscribeHandler();

app.Run();
