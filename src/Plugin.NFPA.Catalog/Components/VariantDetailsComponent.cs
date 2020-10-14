namespace Plugin.NFPA.Catalog.Components
{
    using Sitecore.Commerce.Core;

    public class VariantDetailsComponent : Component
    {
        public bool ERPManaged { get; set; }
        public string PIN { get; set; }
        public string Language { get; set; }
        public string Format { get; set; }
        public bool CanBundle { get; set; }
        public string Year { get; set; }
        public string SortOrder { get; set; }
    }
}
