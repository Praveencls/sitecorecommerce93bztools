using Plugin.NFPA.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Management;
using Sitecore.Framework.Pipelines;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.NFPA.Catalog.Pipelines.Blocks
{
    public class GetTermLocalizedBlock : PipelineBlock<IEnumerable<LocalizedTerm>, IEnumerable<LocalizedTerm>, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander commander;

        public GetTermLocalizedBlock(CommerceCommander commander)
            : base(null)
        {
            this.commander = commander;
        }

        public override async Task<IEnumerable<LocalizedTerm>> Run(IEnumerable<LocalizedTerm> arg, CommercePipelineExecutionContext context)
        {
            GetTermLocalizedBlock localizableTermsByKeyBlock = this;

            if (arg != null)
            {
                return arg;
            }

            var argument = context.CommerceContext.GetObject<LocalizedTermArgument>();
            if (string.IsNullOrEmpty(argument?.LocalizationPath) || string.IsNullOrEmpty(argument?.Key))
            {
                return null;
            }

            var str = context.CommerceContext.CurrentLanguage();
           
            var localizableTermArgument =
                new LocalizableTermArgument(argument?.Key, argument?.LocalizationPath) { Language = str };
            var terms = await localizableTermsByKeyBlock.GetCommerceTerms(localizableTermArgument, context);
            return
                 terms;
        }

        protected virtual async Task<IEnumerable<LocalizedTerm>> GetCommerceTerms(
            LocalizableTermArgument argument,
            CommercePipelineExecutionContext context)
        {
            GetTermLocalizedBlock localizableTermsByKeyBlock = this;

            IEnumerable<LocalizedTerm> terms = new List<LocalizedTerm>();
            if (string.IsNullOrEmpty(argument.Key) || string.IsNullOrEmpty(argument.Path) || (string.IsNullOrEmpty(argument.Language) || context == null))
                return terms;

            IGetItemsByPathPipeline itemsByPathPipeline = this.commander.Pipeline<IGetItemsByPathPipeline>();

            ItemModelArgument itemModelArgument1 = new ItemModelArgument(argument.Path);
            itemModelArgument1.Language = argument.Language;
            CommercePipelineExecutionContext context1 = context;
            IEnumerable<ItemModel> source1 = await itemsByPathPipeline.Run(itemModelArgument1, context1).ConfigureAwait(false);
            List<ItemModel> items = source1?.ToList<ItemModel>();
            if (items == null)
                return terms;
            ItemModel item = items.FirstOrDefault<ItemModel>((Func<ItemModel, bool>)(i => i["ItemName"] != null && ((string)i["ItemName"]).Equals(argument.Key, StringComparison.OrdinalIgnoreCase)));
            ItemModel child;
            if (item != null)
            {
                CommerceContext commerceContext = context.CommerceContext;
                if (!item.HasChildren())
                    return terms;

                IGetItemChildrenPipeline childrenPipeline = this.commander.Pipeline<IGetItemChildrenPipeline>();
                ItemModelArgument itemModelArgument2 = new ItemModelArgument(item.Id());
                itemModelArgument2.Language = argument.Language;
                CommercePipelineExecutionContext context2 = context;
                IEnumerable<ItemModel> source2 = await childrenPipeline.Run(itemModelArgument2, context2).ConfigureAwait(false);
                List<ItemModel> source3 = source2?.ToList<ItemModel>();
                if (source3 == null || !source3.Any<ItemModel>())
                    return terms;

                List<LocalizedTerm> itemTermModel = new List<LocalizedTerm>();
                foreach (ItemModel itemModel in source3.Where<ItemModel>((Func<ItemModel, bool>)(c => c != null)))
                {
                    child = itemModel;
                    LocalizedTerm localizeTerm = await child.ToLocalizedTerm(this.commander, context.CommerceContext).ConfigureAwait(false);
                    
                    itemTermModel.Add(localizeTerm);
                    localizeTerm = (LocalizedTerm)null;
                    child = (ItemModel)null;
                }

                if (itemTermModel.Any())
                    terms = itemTermModel;

                return terms;
            }
            foreach (ItemModel itemModel in items)
            {
                child = itemModel;
                LocalizedTerm cachable = await child.ToLocalizedTerm(this.commander, context.CommerceContext).ConfigureAwait(false);
                
                child = (ItemModel)null;
            }
            foreach (ItemModel itemModel in items.Where<ItemModel>((Func<ItemModel, bool>)(i => i.HasChildren())))
            {
                var localizableTermArgument =
                    new LocalizableTermArgument() { Key = argument.Key, Language = argument.Language, Path = itemModel.Path() };
                terms = (await localizableTermsByKeyBlock.GetCommerceTerms(localizableTermArgument, context).ConfigureAwait(false)).ToList<LocalizedTerm>();
                if (terms.Any<LocalizedTerm>())
                    break;
            }
            return terms;
        }

    }
}