using PizzalandCore.Db;
using PizzalandCore.Db.Repositories;
using PizzalandCore.Interfaces;
using PizzalandCore.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSqlite<PizzalandContext>(builder.Configuration.GetConnectionString("SQLITE"));
builder.Services.AddScoped<IPizzaRepository, PizzaRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<PizzasService>();
app.MapGrpcService<UsersService>();
app.MapGrpcService<OrdersService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
