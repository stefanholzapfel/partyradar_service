using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartyService.Providers
{
    public static class PictureProviderFactory
    {
        public static IPictureProvider Create(ProviderMode mode)
        {
            if (mode == ProviderMode.DbContent)
                return new DbPictureProvider();

            return new MockPictureProvider();
        }
    }
}