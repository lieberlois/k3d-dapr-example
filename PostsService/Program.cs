using PostsService.Data;
using PostsService.DaprServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<IPostsPublishService, PostsPublishService>();
builder.Services.AddScoped<IUrlService, UrlService>();

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
    builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
}

// PostgreSQL Container:
// docker run -it --env POSTGRES_USER=root --env POSTGRES_PASSWORD=root --env POSTGRES_DB=posts_db -p 5432:5432 postgres:latest

var app = builder.Build();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
