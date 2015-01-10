namespace PartyService.Providers
{
    public static class KeywordProviderFactory
    {
        public static IKeywordProvider Create( )
        {
            return new KeywordProvider();
        }
    }
}