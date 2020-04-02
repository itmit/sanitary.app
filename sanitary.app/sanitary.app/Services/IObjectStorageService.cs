using sanitary.app.Models;
using System.Collections.Generic;
using Realms;
using System.Threading.Tasks;

namespace sanitary.app.Services
{
    public interface IObjectStorageService
    {
        Realm Realm { get; }

        Task<List<Object>> GetUserObjectsAsync();
        Task<Object> GetObjectFullInfo(string ObjectUuid);
        Task<List<Node>> GetObjectNodesAsync(string ObjectUuid);
        Task<List<Material>> GetObjectMaterialsAsync(string ObjectUuid);
        Task<List<Node>> GetAllUserNodesAsync();
        Task<string> GetEstimatePdfUrlAsync(string ObjectUuid);

        Task<bool> SendNewObjectAsync(Models.Object CreatedObject);
        Task<bool> UpdateObjectAsync(Models.Object CreatedObject);
        Task<bool> AddMaterialToNode(Material MaterialToAdd, string NodeId);

        void SendCopyNodeRequestAsync(string nodeUuid, string objectUuid);

        Task<bool> DeleteUserObjectsAsync(Models.Object objectToDelete);
        Task<bool> DeleteNodeAsync(string NodeUuid);
        Task<bool> DeleteMaterialFromNodeAsync(string materialUuid);
    }
}
