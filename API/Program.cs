using API.Middleware;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// configure app DB context
builder.Services.AddDbContext<AppDbContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Scoped only to HTTP Request
//// Repos
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//// Identity services
builder.Services.AddAuthorization();
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// on unathorised - 401, do not respond with NotFound/404
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = ctx =>
    {
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});


//// services
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();

// CORS support
// CORS configuration below
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "LMS API", Version = "v1" });

    // File upload support
    options.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware code below
// is software that executes in the request/response pipeline,
// either before the request reaches the application logic,
// or after the response leaves the application.
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

var allowedOrigins = builder.Configuration
    .GetSection("AllowedCorsClients")
    .Get<string[]>() ?? Array.Empty<string>();

app.UseCors(x =>
    x.AllowAnyHeader()
     .AllowAnyMethod()
     .AllowCredentials()                                // accepting identity cookie from these clients
     .WithOrigins(allowedOrigins));

app.MapControllers();

// seeding
try
{
    //  service locator pattern
    // manualy creating an object of AppDbContext outside of DI container
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();          // create DB if not availbale, and apply pending migrations
    await LmsContextSeed.SeedAsync(context);

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await LmsContextSeed.SeedIdentiyAsync(roleManager);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.UseStaticFiles();     // expose wwwroot content - https://localhost:5001/images/booksImgs/7154e61d-19af-49ee-afc8-1b2c8df19bd4.png

app.Run();
