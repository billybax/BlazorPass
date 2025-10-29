using Microsoft.AspNetCore.SignalR;

namespace BlazorPass.Hubs
{
    public class TableUpdateHub : Hub
    {
        // Этот метод будет использоваться для отправки уведомлений об обновлении таблицы
        public async Task SendTableUpdate(string message)
        {
            // Отправляет сообщение всем подключенным клиентам
            await Clients.All.SendAsync("ReceiveTableUpdate", message);
        }

        // Вы можете добавить более специфичные методы, если хотите отправлять конкретные данные
        public async Task SendPasswordEntryUpdate(int entryId, string action)
        {
            await Clients.All.SendAsync("ReceivePasswordEntryUpdate", entryId, action);
        }
    }
}