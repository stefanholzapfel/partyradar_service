namespace PartyService.Providers
{
    public static class LocationProviderFactory
    {
        public static ILocationProvider Create( ApplicationUserManager userManager = null )
        {
            return new DbLocationProvider() { UserManager = userManager };
        }
    }
}