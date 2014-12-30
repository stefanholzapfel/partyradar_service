using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace PartyService.Providers
{
    public static class UserProviderFactory
    {
        public static IUserProvider Create( ProviderMode mode )
        {
            if ( mode == ProviderMode.DbContent )
                return new DbUserProvider();

            return new MockUserProvider();
        }
    }
}