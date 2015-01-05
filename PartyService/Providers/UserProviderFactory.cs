namespace PartyService.Providers
{
    public static class UserProviderFactory
    {
        public static IUserProvider Create( ProviderMode mode, ApplicationUserManager userManager )
        {
            return new DbUserProvider { UserManager = userManager };
        }
    }
}