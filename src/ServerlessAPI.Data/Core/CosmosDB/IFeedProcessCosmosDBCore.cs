using ServerlessAPI.Data.SimpleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessAPI.Data.Core.CosmosDB
{
    public interface IFeedProcessCosmosDBCore
    {
        Task AddRecord(OrderSampleModel order);
    }
}
