using ServerlessAPI.Data.SimpleModels;
using System.Threading.Tasks;

namespace ServerlessAPI.Data.Core.API
{
    public interface IOrdersCore
    {
        Task<OrderOutputModel>  GetAllOrdersDetails(int OrdersId);
    }
}
