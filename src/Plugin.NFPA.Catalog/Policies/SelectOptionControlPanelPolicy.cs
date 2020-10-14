namespace Plugin.NFPA.Catalog.Policies
{
    using Sitecore.Commerce.Core;

    public class SelectOptionControlPanelPolicy : Policy
    {
        public string StoreFrontPath { get; set; } =
            "/sitecore/Commerce/Commerce Control Panel/Storefront Settings/Storefronts";

        public string CommerceTermsPath { get; set; } = "Commerce Terms";
        public string LanguagesPath { get; set; } = "Languages";
        public string FormatsPath { get; set; } = "Formats";
    }
}
