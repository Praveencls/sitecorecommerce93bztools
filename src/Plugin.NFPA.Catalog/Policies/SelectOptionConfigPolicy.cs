using System.Collections.Generic;

namespace Plugin.NFPA.Catalog.Policies
{
    using Sitecore.Commerce.Core;
    public class SelectOptionConfigPolicy : Policy
    {
        public SelectOptionConfigPolicy()
        {
            this.Options = new System.Collections.Generic.List<Selection>();
        }

        // Available options to select from
        public List<Selection> Options { get; set; }
    }
}
