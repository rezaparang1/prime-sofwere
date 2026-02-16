using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Prime_Software.Hubs
{
    public class InvoiceHub : Hub
    {
        // هنگام باز شدن فاکتور، کلاینت در گروه مخصوص آن فاکتور عضو می‌شود
        public async Task SubscribeToInvoice(int invoiceId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"invoice-{invoiceId}");
        }

        // هنگام بسته شدن فاکتور، از گروه خارج می‌شود
        public async Task UnsubscribeFromInvoice(int invoiceId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"invoice-{invoiceId}");
        }
    }
}
