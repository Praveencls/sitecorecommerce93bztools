namespace Plugin.NFPA.Catalog.Policies
{
    using Sitecore.Commerce.Core;
    public class FormatsCachePolicy : CachePolicy
    {
        public FormatsCachePolicy()
        {
            this.CacheName = "Formats";
        }
    }
}
