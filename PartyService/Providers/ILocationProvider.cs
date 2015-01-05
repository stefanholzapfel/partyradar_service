using System;
using System.Threading.Tasks;
using PartyService.ControllerModels;
using PartyService.Models;

namespace PartyService.Providers
{
    public interface ILocationProvider
    {
        Task<ResultSet<LocationDetail>> AddLocationAsync( AddLocation addLocation, string userId );
        Task<Result> ChangeLocationAsync( UpdateLocation u );
        Task<ResultSet<LocationDetail[]>> GetAllAsync( string userId );
        Task RemoveAsync( string userId, Guid locationId );
        Task<bool> LocationExistAsync( string userId, Guid locationId );
        Task AddOwnerAsync( Guid locationId, string userId );
        Task RemoveOwnerAsync(Guid locationId, string userId);
    }
}