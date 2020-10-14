using System.Collections.Generic;
using System.Linq;

namespace Plugin.NFPA.Catalog.Pipelines.Arguments
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;

    public class LocalizedFormatArgument : PipelineArgument
    {
        public LocalizedFormatArgument() {}
        public LocalizedFormatArgument(string localizationKey, object[] args = null)
        {
            Condition.Requires<string>(localizationKey, nameof(localizationKey)).IsNotNullOrEmpty();
            this.LocalizationKey = localizationKey;
            if (args == null)
            {
                return;
            }

            this.Arguments = ((IEnumerable<object>)args).ToList<object>();
        }

        public LocalizedFormatArgument(string localizationKey, List<object> args)
        {
            Condition.Requires<string>(localizationKey, nameof(localizationKey)).IsNotNullOrEmpty();
            this.LocalizationKey = localizationKey;
            if (args == null)
            {
                return;
            }

            this.Arguments = args;
        }

        public string LocalizationKey { get; set; }

        public List<object> Arguments { get; }
    }
}
