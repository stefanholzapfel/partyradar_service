using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PartyService.DatabaseModels;
using PartyService.Models;
using Keyword = PartyService.ControllerModels.Keyword;

namespace PartyService.Providers
{
    public interface IKeywordProvider
    {
        Task<ResultSet<Keyword[]>> GetKeywordsAsync();
        Task<ResultSet<Keyword>> GetKeywordAsync( Guid id );
        Task<Result> ChangeLabel( Keyword changeKeyword );
        Task<ResultSet<Keyword>> AddKeywordAsync( string addLabel );
        Task<bool> KeywordExistAsync( Guid id );
        Task<Result> RemoveKeywordAsync( Guid id );
    }

    public class KeywordProvider : IKeywordProvider
    {
        public async Task<ResultSet<Keyword[]>> GetKeywordsAsync()
        {
            using ( var db = new ApplicationDbContext() )
            {
                try
                {
                    var keywords = await db.Keywords.Where(x => !x.IsInactive)
                        .Select(x => new Keyword { Id = x.Id, Label = x.Label })
                        .OrderBy( x=>x.Label )
                        .ToArrayAsync();
                    return new ResultSet<Keyword[]>( true ) { Result = keywords };
                }
                catch ( Exception exception)
                {
                    return new ResultSet<Keyword[]>( false, exception.Message );
                }
            }
        }

        public async Task<ResultSet<Keyword>> GetKeywordAsync( Guid id )
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var keyword = await GetAllActiveKeywords( db ).SingleOrDefaultAsync( x => x.Id == id );
                    if ( keyword == null )
                        return new ResultSet<Keyword>( false, "Keyword not found for given Id!" );
                    
                    return new ResultSet<Keyword>( true ) { Result = Convert( keyword ) };
                }
            }
            catch ( Exception exc)
            {
                return new ResultSet<Keyword>( false, exc.Message );
            }
        }

        public async Task<Result> ChangeLabel( Keyword changeKeyword )
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var labelExist = db.Keywords.ToArray().SingleOrDefault(x => x.Label.ToUpper(CultureInfo.CurrentCulture) == changeKeyword.Label.ToUpper(CultureInfo.CurrentCulture));
                    if (labelExist != null)
                        return new Result(false,
                            string.Format("Label allready exist! See Keyword(id:{0},label: {1}, IsInactive: {2}", labelExist.Id, labelExist.Label, labelExist.IsInactive));

                    var keyword = await GetAllActiveKeywords(db).SingleOrDefaultAsync(x => x.Id == changeKeyword.Id);
                    if (keyword == null)
                        return new Result(false, "Keyword not found for given Id!");

                    keyword.Label = changeKeyword.Label;
                    db.Keywords.AddOrUpdate(keyword);
                    await db.SaveChangesAsync();
                }
            }
            catch ( Exception exc)
            {
                return new Result( false, exc.Message );
            }
            return new Result(true);
        }

        public async Task<ResultSet<Keyword>> AddKeywordAsync( string addLabel )
        {
            var id = Guid.NewGuid();
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var keyword = db.Keywords.ToArray().SingleOrDefault(x => x.Label.ToUpper(CultureInfo.CurrentCulture) == addLabel.ToUpper(CultureInfo.CurrentCulture));
                    if (keyword != null)
                        return new ResultSet<Keyword>(false,
                            string.Format("Label allready exist! See Keyword(id:{0},label: {1}, IsInactive: {2}", keyword.Id, keyword.Label, keyword.IsInactive));

                    db.Keywords.Add(new DatabaseModels.Keyword { Id = id, Label = addLabel });
                    await db.SaveChangesAsync();
                }
            }
            catch ( Exception exc )
            {
                return new ResultSet<Keyword>( false, exc.Message );
            }
            return await GetKeywordAsync(id);
        }

        public async Task<bool> KeywordExistAsync( Guid id )
        {
            using ( var db = new ApplicationDbContext() )
            {
                return await KeywordExists( db, id );
            }
        }

        public async Task<Result> RemoveKeywordAsync( Guid id )
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var keyword = await GetAllActiveKeywords(db).SingleOrDefaultAsync(x => x.Id == id);
                    if (keyword == null)
                        return new Result(false, "Keyword not found for given ID!");

                    keyword.IsInactive = true;
                    db.Keywords.AddOrUpdate(keyword);
                    await db.SaveChangesAsync();
                }
            }
            catch ( Exception exc )
            {
                return new Result( false, exc.Message );
            }

            return new Result( true );
        }

        private static async Task<bool> KeywordExists(ApplicationDbContext db, Guid id )
        {
            return await GetAllActiveKeywords( db ).AnyAsync( x => x.Id == id );
        }

        private static IQueryable<DatabaseModels.Keyword> GetAllActiveKeywords( ApplicationDbContext db )
        {
            return db.Keywords.Where( x => !x.IsInactive );
        }

        public static Keyword[] GetKeywords( ApplicationDbContext db )
        {
            return GetAllActiveKeywords( db ).Select( x => new Keyword { Id = x.Id, Label = x.Label } ).ToArray();
        }

        private static ControllerModels.Keyword Convert( DatabaseModels.Keyword keyword )
        {
            return new Keyword
            {
                Id = keyword.Id,
                Label = keyword.Label
            };
        }
    }
}