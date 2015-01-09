namespace PartyService.Providers
{
    public static class EventProviderFactory
    {
        public static IEventProvider Create( )
        {
            return new DbEventProvider();
        }
    }
}