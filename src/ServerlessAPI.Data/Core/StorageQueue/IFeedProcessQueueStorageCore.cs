using ServerlessAPI.Data.SimpleModels;
using System.Threading.Tasks;

namespace ServerlessAPI.Data.Core.StorageQueue
{
    public interface IFeedProcessQueueStorageCore
    {
        Task<string> SendMessage(string inputMessage);
    }
}
