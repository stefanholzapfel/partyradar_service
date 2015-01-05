﻿using System;
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
        Task<Result> RemoveAsync( string userId, Guid locationId );
    }
}