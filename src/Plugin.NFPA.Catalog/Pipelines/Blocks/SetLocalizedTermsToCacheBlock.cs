using Plugin.NFPA.Catalog.Pipelines.Arguments;
using Plugin.NFPA.Catalog.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Caching;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.NFPA.Catalog.Pipelines.Blocks
{
    public class SetLocalizedTermsToCacheBlock : PipelineBlock<IEnumerable<LocalizedTerm>, IEnumerable<LocalizedTerm>, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander commander;

        public SetLocalizedTermsToCacheBlock(CommerceCommander commander)
            : base( ) => this.commander = commander;

        public override async Task<IEnumerable<LocalizedTerm>> Run(IEnumerable<LocalizedTerm> arg, CommercePipelineExecutionContext context)
        {
            if (arg == null)
            {
                return null;
            }

            var customerStatusArgument = context.CommerceContext.GetObject<LocalizedTermArgument>();
            if (string.IsNullOrEmpty(customerStatusArgument?.LocalizationPath) || string.IsNullOrEmpty(customerStatusArgument?.Key))
            {
                return arg;
            }

            var keyValuePair = context.CommerceContext.GetObject<KeyValuePair<string, bool>>((Func<KeyValuePair<string, bool>, bool>)(k => k.Key.Equals("IsFromCache", StringComparison.OrdinalIgnoreCase)));
            var str = context.CommerceContext.CurrentLanguage();
            var policy = context.GetPolicy<LanguagesCachePolicy>();
            if (!policy.AllowCaching || keyValuePair.Value)
            {
                return arg;
            }

            var cacheEntryKey = customerStatusArgument?.Key + "|" +customerStatusArgument.LocalizationPath + "|" + str;
            var localizedTerms = arg as LocalizedTerm [ ] ?? arg.ToArray( );
            var num = await this.commander.SetCacheEntry<LocalizedTerm>(context.CommerceContext, policy.CacheName, cacheEntryKey, (ICachable)new Cachable<IEnumerable<LocalizedTerm>>(localizedTerms, 1L), policy.GetCacheEntryOptions()).ConfigureAwait(false) ? 1 : 0;
            return localizedTerms;
        }
    }
}
