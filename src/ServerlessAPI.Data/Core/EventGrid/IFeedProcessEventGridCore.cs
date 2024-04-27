using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerlessAPI.Data.Core.EventGrid
{
    public interface IFeedProcessEventGridCore
    {
        Task  PushEvent(string inputMessage);
        //Task<List<string>> PopEvent();
    }
}
