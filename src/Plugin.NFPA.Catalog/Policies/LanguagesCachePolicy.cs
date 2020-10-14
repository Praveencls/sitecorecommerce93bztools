namespace Plugin.NFPA.Catalog.Policies
{
    using Sitecore.Commerce.Core;
    public class LanguagesCachePolicy : CachePolicy
    {
        public LanguagesCachePolicy()
        {
            this.CacheName = "Languages";
        }
    }
}
