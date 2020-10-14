using Plugin.NFPA.Catalog.Components;
using Plugin.NFPA.EntityViews.EntityViews;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.NFPA.Catalog.EntityView
{
    public class VariantDetailsView : ComponentView<SellableItem, VariantDetailsComponent>
    {
        public override string ViewName => nameof(VariantDetailsView);
        public override string ViewDisplayName => "Variant Details";

        public override async Task ModifyView(CommercePipelineExecutionContext context,
            Sitecore.Commerce.EntityViews.EntityView entityView, SellableItem entity, VariantDetailsComponent component)
        {
            var childView = new Sitecore.Commerce.EntityViews.EntityView
            {
                Name = this.ViewName,
                DisplayName = this.ViewDisplayName,
                EntityId = entityView.EntityId,
                EntityVersion = entityView.EntityVersion,
                ItemId = entityView.ItemId
            };

            entityView.ChildViews.Add(childView);

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.ERPManaged),
                DisplayName = "ERP Managed",
                IsRequired = false,
                RawValue = component?.ERPManaged,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.PIN),
                DisplayName = "PIN",
                IsRequired = false,
                RawValue = component?.PIN,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.Language),
                DisplayName = "Language",
                IsRequired = false,
                RawValue = component?.Language,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.Format),
                DisplayName = "Format",
                IsRequired = false,
                RawValue = component?.Format,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.CanBundle),
                DisplayName = "Can Bundle",
                IsRequired = false,
                RawValue = component?.CanBundle,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.Year),
                DisplayName = "Year",
                IsRequired = false,
                RawValue = component?.Year,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantDetailsComponent.SortOrder),
                DisplayName = "Sort Order",
                IsRequired = false,
                RawValue = component?.SortOrder,
                IsReadOnly = true
            });
        }

        public override bool ShouldViewApply(CommercePipelineExecutionContext context, Sitecore.Commerce.EntityViews.EntityView entityView, SellableItem entity)
        {
            var knownViews = context.GetPolicy<KnownCatalogViewsPolicy>();
            return entityView.Name == knownViews.Variant || entityView.Name == knownViews.ConnectSellableItem;
        }
        public override VariantDetailsComponent GetComponent(CommercePipelineExecutionContext context, Sitecore.Commerce.EntityViews.EntityView entityView,
            SellableItem entity)
        {
            var variation = entity.GetVariation(entityView.ItemId) ?? entity.GetComponent<ItemVariationComponent>();

            return variation?.GetComponent<VariantDetailsComponent>();
        }
    }
}
