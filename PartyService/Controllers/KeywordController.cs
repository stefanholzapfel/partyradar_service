using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Http;
using PartyService.ControllerModels;
using PartyService.Providers;

namespace PartyService.Controllers
{
    public class KeywordController : ApiController
    {
        // GET: api/Keyword
        public async Task<IHttpActionResult> Get()
        {
            var result = await KeywordProviderFactory.Create()
                .GetKeywordsAsync();

            if ( result.Succeeded )
                return Ok( result.Result );

            return BadRequest( result.ErrorMessage );
        }

        // GET: api/Keyword/5
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var provider = KeywordProviderFactory.Create();
            if ( ! await provider.KeywordExistAsync( id ) )
                return NotFound();

            var result = await provider.GetKeywordAsync( id );
            if ( result.Succeeded )
                return Ok( result.Result );

            return BadRequest( result.ErrorMessage );
        }

        // POST: api/Keyword
        public async Task<IHttpActionResult> Post(AddLabel addLabel)
        {
            if (addLabel == null)
                return BadRequest("Label attribute is required!");

            if ( !ModelState.IsValid )
                return BadRequest( ModelState );

            var result = await KeywordProviderFactory.Create().AddKeywordAsync( addLabel.Label );
            if ( result.Succeeded )
                return Ok( result.Result );

            return BadRequest( result.ErrorMessage );
        }

        // PUT: api/Keyword/5
        public async Task<IHttpActionResult> Put(Guid id, ChangeLabel model)
        {
            if ( model == null )
                return BadRequest( "Label attribute is required!" );

            if ( !ModelState.IsValid )
                return BadRequest( ModelState );
           
             
            var provider = KeywordProviderFactory.Create();

            if ( !await provider.KeywordExistAsync( id ) )
                return NotFound();

            var result = await provider.ChangeLabel( new Keyword { Id = id, Label = model.Label } );
            if ( result.Succeeded )
                return new NoContent();

            return BadRequest( result.ErrorMessage );
        }

        // DELETE: api/Keyword/5
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var provider = KeywordProviderFactory.Create();
            if ( ! await provider.KeywordExistAsync( id ) )
                return NotFound();

            var result = await provider.RemoveKeywordAsync( id );
            if ( result.Succeeded )
                return new NoContent();

            return BadRequest( result.ErrorMessage );
        }
    }
}
