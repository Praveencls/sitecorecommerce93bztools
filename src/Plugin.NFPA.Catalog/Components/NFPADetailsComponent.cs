using System;
using Sitecore.Commerce.Core;

namespace Plugin.NFPA.Catalog.Components
{
    public class NFPADetailsComponent : Component
    {
        /// <summary>
        /// Last date this product was modified from the source
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// The date this product was introduced
        /// </summary>
        public DateTime IntroductionDate { get; set; }
    }
}