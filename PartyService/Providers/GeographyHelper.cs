using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PartyService.Providers
{
    public static class GeographyHelper
    {
        public static DbGeography CreatePoint( double latitude, double longitude )
        {
            //Contract.Requires<ArgumentOutOfRangeException>(latitude >= -90 && latitude <= 90);
            //Contract.Requires<ArgumentOutOfRangeException>(longitude >= -180 && longitude <= 180);

            return DbGeography.PointFromText(string.Format(CultureInfo.InvariantCulture,"POINT({0} {1})", longitude, latitude), 4326);
        }
    }
}