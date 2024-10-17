using pizzalandClient;
using pizzalandClient.Interfaces;
using pizzalandClient.Services;

var serverUri = new Uri("http://localhost:5000");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
builder.Services.AddGrpcClient<UserService.UserServiceClient>(o =>
{
    o.Address = serverUri;
}).AddCallCredentials(async (context, metadata, serviceProvider) =>
    {
        var provider = serviceProvider.GetRequiredService<ITokenProvider>();
        var token = provider.GetToken(context.CancellationToken);
        metadata.Add("Authorization", $"Bearer {token}");
    })
    .ConfigureChannel(o => o.UnsafeUseInsecureChannelCallCredentials = true);

builder.Services.AddGrpcClient<OrderService.OrderServiceClient>(o =>
{
    o.Address = serverUri;
}).AddCallCredentials(async (context, metadata, serviceProvider) =>
    {
        var provider = serviceProvider.GetRequiredService<ITokenProvider>();
        var token = provider.GetToken(context.CancellationToken);
        metadata.Add("Authorization", $"Bearer {token}");
    })
    .ConfigureChannel(o => o.UnsafeUseInsecureChannelCallCredentials = true);

builder.Services.AddGrpcClient<PizzaService.PizzaServiceClient>(o =>
{
    o.Address = serverUri;
}).AddCallCredentials(async (context, metadata, serviceProvider) =>
    {
        var provider = serviceProvider.GetRequiredService<ITokenProvider>();
        var token = provider.GetToken(context.CancellationToken);
        metadata.Add("Authorization", $"Bearer {token}");
    })
    .ConfigureChannel(o => o.UnsafeUseInsecureChannelCallCredentials = true);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
