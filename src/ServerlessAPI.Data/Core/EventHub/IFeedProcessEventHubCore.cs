using System.Threading.Tasks;

namespace ServerlessAPI.Data.Core.EventHub
{
    public interface IFeedProcessEventHubCore
    {
        Task PushEvent(string inputMessage);
        Task<string> PopEvent();
    }
}
