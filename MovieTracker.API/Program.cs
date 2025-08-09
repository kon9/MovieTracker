using AutoMapper;
using MovieTracker.Core.Repositories;
using MovieTracker.Core.Services;
using MovieTracker.Infrastructure.Data;
using MovieTracker.Infrastructure.Mapping;
using MovieTracker.Infrastructure.Migrations;
using MovieTracker.Infrastructure.Repositories;
using MovieTracker.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper
builder.Services.AddSingleton<IMapper>(provider =>
{
    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<AutoMapperProfile>();
    }, new LoggerFactory());
    return config.CreateMapper();
});

// Add PostgreSQL Database Connection
builder.Services.AddSingleton<IDatabaseConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new PostgresConnection(connectionString);
});

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IQueueRepository, QueueRepository>();
builder.Services.AddScoped<IQueueItemRepository, QueueItemRepository>();
builder.Services.AddScoped<IQueueMemberRepository, QueueMemberRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();

// Add Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IRatingService, RatingService>();

// Add Migration Runner
builder.Services.AddScoped<IMigrationRunner, DapperMigrationRunner>();

var app = builder.Build();

// Run migrations
using (var scope = app.Services.CreateScope())
{
    var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    await migrationRunner.RunMigrationsAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
});

app.Run();
