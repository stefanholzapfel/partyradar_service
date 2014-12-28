using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartyService.Providers
{
    public class VisitResult
    {
        public System.Net.HttpStatusCode HttpCode { get; set; }
        public DateTime Timestamp { get; set; }
    }
}