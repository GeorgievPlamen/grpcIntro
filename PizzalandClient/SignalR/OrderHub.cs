namespace pizzalandClient.SignalR;

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class OrderHub : Hub
{
    public async Task SendOrderStatus(string orderId, string status)
    {
        await Clients.All.SendAsync("ReceiveOrderStatus", orderId, status);
    }
}
