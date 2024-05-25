using Repository;
using API.Filters;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(x => x.Filters.Add<ValidationFilter>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Add services to the container.

// DI(Dependency Injection) Container framework
// IoC ( Inversion Of Container)  framework
//  - Dependency Inversion / Inversion Of Control Principles
//  - Dependency Injection Design Pattern

// 1. AddSingleton
// 2. AddScoped (*)
// 3. AddTransient

builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddRepository(builder.Configuration);
builder.Services.AddServices();


var app = builder.Build();

app.SeedDatabase();

// Configure the HTTP request pipeline.
app.AddMidddlewares();

app.Run();
