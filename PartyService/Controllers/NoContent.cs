using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace PartyService.Controllers
{
    public class NoContent: IHttpActionResult
    {
        public async Task<HttpResponseMessage> ExecuteAsync( CancellationToken cancellationToken )
        {
            return await Task.FromResult( new HttpResponseMessage( HttpStatusCode.NoContent ) );
        }
    }

    public class NoAuthorized : IHttpActionResult
    {
        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
        }
    }
}