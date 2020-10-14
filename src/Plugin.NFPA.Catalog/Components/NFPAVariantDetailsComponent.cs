using Sitecore.Commerce.Core;

namespace Plugin.NFPA.Catalog.Components
{
    public class NFPAVariantDetailsComponent : Component
    {
        public string SKU { get; set; }
        public string VariantName { get; set; }
        public string Details { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
    }
}