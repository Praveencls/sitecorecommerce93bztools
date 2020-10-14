using Plugin.NFPA.Catalog.Components;
using Plugin.NFPA.Catalog.Pipelines;
using Plugin.NFPA.Catalog.Pipelines.Arguments;
using Plugin.NFPA.Catalog.Policies;
using Plugin.NFPA.EntityViews.EntityViews;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.NFPA.Catalog.EntityView
{
    public class VariantDetailsActionView : ComponentActionView<SellableItem, VariantDetailsComponent>
    {
        private readonly FindEntityCommand findEntityCommand;
        public override string ViewName => nameof(VariantDetailsActionView);
        protected ViewCommander Commander { get; set; }
        public override string ViewDisplayName => "Edit Variant Details";
        public VariantDetailsActionView(PersistEntityCommand persistEntityCommand, FindEntityCommand findEntityCommand, ViewCommander commander) : base(persistEntityCommand)
        {
            this.findEntityCommand = findEntityCommand;
            this.Commander = commander;
        }

        public override string ActionIcon => "edit";

        public override Task DoAction(CommercePipelineExecutionContext context, Sitecore.Commerce.EntityViews.EntityView entityView, SellableItem entity, VariantDetailsComponent component)
        {
            foreach (var property in entityView.Properties)
            {
                if (property.Name == nameof(VariantDetailsComponent.ERPManaged))
                {
                    component.ERPManaged = bool.Parse(property.Value);
                }

                if (property.Name == nameof(VariantDetailsComponent.PIN))
                {
                    component.PIN = property.Value;
                }

                if (property.Name == nameof(VariantDetailsComponent.Language))
                {
                    component.Language = property.Value;
                }

                if (property.Name == nameof(VariantDetailsComponent.Format))
                {
                    component.Format = property.Value;
                }

                if (property.Name == nameof(VariantDetailsComponent.CanBundle))
                {
                    component.CanBundle = bool.Parse(property.Value);
                }

                if (property.Name == nameof(VariantDetailsComponent.Year))
                {
                    component.Year = property.Value;
                }

                if (property.Name == nameof(VariantDetailsComponent.SortOrder))
                {
                    component.SortOrder = property.Value;
                }
            }

            return Task.CompletedTask;
        }

        public override bool ShouldAddAction(CommercePipelineExecutionContext context, Sitecore.Commerce.EntityViews.EntityView entityView, SellableItem entity)
        {
            return entityView.Name.Equals(nameof(VariantDetailsView));
        }

        public override VariantDetailsComponent GetComponent(CommercePipelineExecutionContext context, Sitecore.Commerce.EntityViews.EntityView entityView,
            SellableItem entity)
        {
            var variation = entity.GetVariation(entityView.ItemId);
            return variation?.GetComponent<VariantDetailsComponent>();
        }

        public override void SetComponent(CommercePipelineExecutionContext context, Sitecore.Commerce.EntityViews.EntityView entityView, SellableItem entity,
            VariantDetailsComponent component)
        {
            var variation = entity.GetVariation(entityView.ItemId);
            variation.SetComponent(component);
        }
        public override async Task ModifyView(CommercePipelineExecutionContext context, Sitecore.Commerce.EntityViews.EntityView entityView, SellableItem entity, VariantDetailsComponent component)
        {
            entityView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.ERPManaged),
                DisplayName = "ERP Managed",
                IsRequired = false,
                RawValue = component.ERPManaged
            });

            entityView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.PIN),
                DisplayName = "PIN",
                IsRequired = false,
                RawValue = component.PIN
            });

            var languageSelectionsPolicy = new AvailableSelectionsPolicy();

            var selectizeConfig = await this.GetLanguages(context);
            if (selectizeConfig != null)
            {
                languageSelectionsPolicy.List.AddRange(selectizeConfig.Options);
            }

            entityView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.Language),
                DisplayName = "Language",
                IsRequired = false,
                RawValue = component.Language,
                IsReadOnly = false,
                UiType = "Dropdown",
                Policies = new List<Policy>() { languageSelectionsPolicy }
            });
            var formatSelectionsPolicy = new AvailableSelectionsPolicy();
            var formatConfig = await this.GetFormats(context);
            if (formatConfig != null)
            {
                formatSelectionsPolicy.List.AddRange(formatConfig.Options);
            }

            entityView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.Format),
                DisplayName = "Format",
                IsRequired = false,
                RawValue = component.Format,
                IsReadOnly = false,
                UiType = "Dropdown",
                Policies = new List<Policy>() { formatSelectionsPolicy }
            });
            entityView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.CanBundle),
                DisplayName = "Can Bundle",
                IsRequired = false,
                RawValue = component.CanBundle
            });
            entityView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.Year),
                DisplayName = "Year",
                IsRequired = false,
                RawValue = component.Year
            });

            entityView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.SortOrder),
                DisplayName = "Sort Order",
                IsRequired = false,
                RawValue = component.SortOrder
            });
        }

        private async Task<SelectOptionConfigPolicy> GetLanguages(CommercePipelineExecutionContext context)
        {
            var availableSelectionsPolicy = new SelectOptionConfigPolicy();

            availableSelectionsPolicy.Options = await this.TryGetLanguages(context);

            return availableSelectionsPolicy;
        }

        private async Task<List<Selection>> TryGetLanguages(CommercePipelineExecutionContext context)
        {
            var policy = context.GetPolicy<SelectOptionControlPanelPolicy>();
            var termPath = $"{policy.StoreFrontPath}/{context.CommerceContext.CurrentShopName()}/{policy.CommerceTermsPath}";
            var localizedOptions = await this.Commander.Pipeline<IGetLocalizedLanguagesPipeline>().Run(new LocalizedTermArgument(policy.LanguagesPath,termPath), context);
            return localizedOptions?.Select(term => new Selection() { DisplayName = term.Value, Name = term.Key }).ToList() ?? new List<Selection>();
        }

        private async Task<SelectOptionConfigPolicy> GetFormats(CommercePipelineExecutionContext context)
        {
            var availableSelectionsPolicy = new SelectOptionConfigPolicy();
            availableSelectionsPolicy.Options = await this.TryGetFormats(context);
            return availableSelectionsPolicy;
        }

        private async Task<List<Selection>> TryGetFormats(CommercePipelineExecutionContext context)
        {
            var policy = context.GetPolicy<SelectOptionControlPanelPolicy>();
            var termPath = $"{policy.StoreFrontPath}/{context.CommerceContext.CurrentShopName()}/{policy.CommerceTermsPath}";
            var localizedOptions = await this.Commander.Pipeline<IGetLocalizedLanguagesPipeline>().Run(new LocalizedTermArgument(policy.FormatsPath, termPath), context);
            return localizedOptions?.Select(term => new Selection() { DisplayName = term.Value, Name = term.Key }).ToList() ?? new List<Selection>();
        }
    }
}
