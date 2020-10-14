using System.Collections.Generic;
using System.Linq;

namespace Plugin.NFPA.Catalog.Pipelines.Arguments
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;
    public class LocalizedTermArgument : PipelineArgument
    {
        public LocalizedTermArgument() { }
        public LocalizedTermArgument(string key, string localizationPath, object [ ] args = null)
        {
            Condition.Requires<string>(localizationPath, nameof(localizationPath)).IsNotNullOrEmpty();
            this.LocalizationPath = localizationPath;
            this.Key = key;
            if (args == null)
            {
                return;
            }

            this.Arguments = ((IEnumerable<object>) args).ToList<object>();
        }

        public string Key { get; set; }

        public string LocalizationPath { get; set; }

        public List<object> Arguments { get; }
    }
}
