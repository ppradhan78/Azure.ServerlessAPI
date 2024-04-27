using ServerlessAPI.Data.SimpleModels;
using System.IO;
using System;
using System.Threading.Tasks;

namespace ServerlessAPI.Data.Core.Blob
{
    public interface IFeedProcessBlobStorageCore
    {
        Task<Uri> UploadFileBlobAsync(Stream content, string fileName);
    }
}
