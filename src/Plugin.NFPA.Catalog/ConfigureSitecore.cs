using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Configuration;
using System.Reflection;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace Plugin.NFPA.Core
{
    using Plugin.NFPA.Catalog.EntityView;
    using Plugin.NFPA.Catalog.Pipelines;
    using Plugin.NFPA.Catalog.Pipelines.Blocks;
    using Plugin.NFPA.EntityViews.Pipelines.Blocks;
    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.AddTransient<SellableItemDetailsView>();
            services.AddTransient<SellableItemDetailsActionView>();
            services.AddTransient<VariantDetailsView>();
            services.AddTransient<VariantDetailsActionView>();
            services.Sitecore().Pipelines(config =>
                config

               .ConfigurePipeline<IConfigureServiceApiPipeline>(configure => configure.Add<Plugin.NFPA.Catalog.ConfigureServiceApiBlock>())

                    .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                    {
                        c.Add<GetViewBlock<SellableItemDetailsView, SellableItem>>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetViewBlock<SellableItemDetailsActionView, SellableItem>>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetViewBlock<VariantDetailsView, SellableItem>>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetViewBlock<VariantDetailsActionView, SellableItem>>().After<GetSellableItemDetailsViewBlock>();
                    })
                    .ConfigurePipeline<IPopulateEntityViewActionsPipeline>(c =>
                    {
                        c.Add<PopulateActionBlock<SellableItemDetailsActionView, SellableItem>>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateActionBlock<VariantDetailsActionView, SellableItem>>().After<InitializeEntityViewActionsBlock>();
                    })
                    .ConfigurePipeline<IDoActionPipeline>(c =>
                    {
                        c.Add<DoActionBlock<VariantDetailsActionView, SellableItem>>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionBlock<SellableItemDetailsActionView, SellableItem>>().After<ValidateEntityVersionBlock>();
                    }).AddPipeline<IGetLocalizedLanguagesPipeline, GetLocalizedLanguagesPipeline>(c =>
                   c.Add<GetLocalizedTermsFromCacheBlock>()
                       .Add<GetTermLocalizedBlock>()
                       .Add<SetLocalizedTermsToCacheBlock>()
               )
            );
            services.RegisterAllCommands(assembly);
        }
    }
}
