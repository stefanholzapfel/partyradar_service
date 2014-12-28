using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartyService.Providers
{
    public static class EventProviderFactory
    {
        public static IEventProvider Create(ProviderMode content)
        {
            if (content == ProviderMode.DbContent)
                return new DbEventProvider();

            return new MockEventProvider();
        }
    }
}