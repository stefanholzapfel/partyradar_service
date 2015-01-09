namespace PartyService.Providers
{
    public static class EventProviderFactory
    {
        public static IAppEventProvider Create( )
        {
            return new DbEventProvider();
        }

        public static IWebEventProvider CreateWebEventProvider(string userId, ApplicationUserManager userManager =null )
        {
            return new DbWebEventProvider {UserId = userId, UserManager = userManager };
        }
    }
}