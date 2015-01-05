namespace PartyService.Providers
{
    public static class UserProviderFactory
    {
        public static IUserProvider Create(ApplicationUserManager userManager = null )
        {
            return new DbUserProvider { UserManager = userManager };
        }
    }
}