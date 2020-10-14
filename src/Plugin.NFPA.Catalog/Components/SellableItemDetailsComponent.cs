namespace Plugin.NFPA.Catalog.Components
{
    using System;
    using Sitecore.Commerce.Core;
    public class SellableItemDetailsComponent : Component
    {
        public DateTimeOffset FeaturedDate { get; set; }
        public bool ERPManaged { get; set; }
        public string MaxQuantity { get; set; }
        public string MinQuantity { get; set; }
    }
}
