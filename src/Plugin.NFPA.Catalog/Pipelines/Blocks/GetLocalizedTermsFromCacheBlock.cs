using Microsoft.Extensions.Logging;
using Plugin.NFPA.Catalog.Pipelines.Arguments;
using Plugin.NFPA.Catalog.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.NFPA.Catalog.Pipelines.Blocks
{
    public class GetLocalizedTermsFromCacheBlock : PipelineBlock<LocalizedTermArgument, LocalizedTerm, CommercePipelineExecutionContext>
    {
        /// <summary>
        /// Gets or sets the commander.
        /// </summary>
        /// <value>
        /// The commander.
        /// </value>
        protected CommerceCommander Commander { get; set; }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:Sitecore.Framework.Pipelines.PipelineBlock" /> class.</summary>
        /// <param name="commander">The commerce commander.</param>
        public GetLocalizedTermsFromCacheBlock(CommerceCommander commander)
            : base(null) => this.Commander = commander;

        public override async Task<LocalizedTerm> Run(LocalizedTermArgument arg, CommercePipelineExecutionContext context)
        {
            var languagesCacheBlock = this;
            // ISSUE: explicit non-virtual call
            Condition.Requires<LocalizedTermArgument>(arg).IsNotNull<LocalizedTermArgument>(languagesCacheBlock.Name + ": pipeline block argument can not be null");
            // ISSUE: explicit non-virtual call
            Condition.Requires<string>(arg.LocalizationPath).IsNotNullOrEmpty(languagesCacheBlock.Name + ": pipeline block argument LocalizationKey cannot be null or empty");
            context.CommerceContext.AddUniqueObjectByType((object)arg);
            var policy = context.GetPolicy<LanguagesCachePolicy>();
            if (!policy.AllowCaching)
            {
                context.CommerceContext.AddUniqueObjectByType((object)new KeyValuePair<string, bool>("IsFromCache", false));
                return null;
            }

            var cacheKey = $"{arg.Key}|{arg.LocalizationPath}|{context.CommerceContext.CurrentLanguage()}";
            var localizedTerm = await languagesCacheBlock.Commander.GetCacheEntry<LocalizedTerm>(context.CommerceContext, policy.CacheName, cacheKey).ConfigureAwait(false);
            if (localizedTerm == null)
            {
                context.CommerceContext.AddUniqueObjectByType(new KeyValuePair<string, bool>("IsFromCache", false));
                return null;
            }
            context.CommerceContext.AddUniqueObjectByType(new KeyValuePair<string, bool>("IsFromCache", true));
            // ISSUE: explicit non-virtual call
            context.Logger.LogDebug(languagesCacheBlock.Name + ": Mgmt.GetLocMsg." + cacheKey);
            return localizedTerm;
        }
    }
}
