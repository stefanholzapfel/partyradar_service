using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PartyService.ControllerModels;
using PartyService.Models;

namespace PartyService.Providers
{
    public interface ILocationProvider
    {
        ResultSet<LocationDetail> AddLocation( AddLocation addLocation, string userId );
        Result ChangeLocation( UpdateLocation updateLocation );
        ResultSet<LocationDetail> GetAll(string userId );
    }
}