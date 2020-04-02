using sanitary.app.Models;
using System.Collections.Generic;
using Realms;
using System.Threading.Tasks;

namespace sanitary.app.Services
{
    public interface IDirectoryStorageService
    {
        Realm Realm { get; }

        Directory GetDirectory(string id);
        Task<List<Directory>> GetAllDirectoriesAsync();
        Task<List<Directory>> GetSubDirectoriesAsync(string directoryUuid);
        Task<List<Directory>> GetPositionsAsync(string directoryUuid);
        Task<Position> GetSinglePositionAsync(string postitionUuid);

        Task<List<Directory>> SearchDirectoriesAsync(string searchText);

        bool DoesDirectoryExist(Directory directory);

    }
}