using IndacoProject.Corso.Core;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Api.Hubs
{
    public class NotifyHub : Hub<INotify>
    {

    }
}