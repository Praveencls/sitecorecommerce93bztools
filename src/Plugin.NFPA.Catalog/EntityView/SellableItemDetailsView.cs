using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Threading.Tasks;

namespace Plugin.NFPA.Catalog.EntityView
{
    using Plugin.NFPA.Catalog.Components;
    using Plugin.NFPA.EntityViews.EntityViews;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;

    public class SellableItemDetailsView : ComponentView<SellableItem, SellableItemDetailsComponent>
    {
        public override string ViewName => nameof(SellableItemDetailsView);

        public override string ViewDisplayName => "Sellable Item Details";

        public override Task ModifyView(CommercePipelineExecutionContext context, EntityView entityView, SellableItem entity, SellableItemDetailsComponent component)
        {
            var childView = new Sitecore.Commerce.EntityViews.EntityView
            {
                Name = this.ViewName,
                DisplayName = this.ViewDisplayName,
                EntityId = entityView.EntityId,
                EntityVersion = entityView.EntityVersion,
                ItemId = component.Id
            };

            entityView.ChildViews.Add(childView);

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(SellableItemDetailsComponent.MinQuantity),
                DisplayName = "Min Quantity",
                IsRequired = false,
                RawValue = component?.MinQuantity,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(SellableItemDetailsComponent.MaxQuantity),
                DisplayName = "Max Quantity",
                IsRequired = false,
                RawValue = component?.MaxQuantity,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(SellableItemDetailsComponent.FeaturedDate),
                DisplayName = "Featured Date",
                IsRequired = false,
                RawValue = component?.FeaturedDate,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(SellableItemDetailsComponent.ERPManaged),
                DisplayName = "ERP Managed",
                IsRequired = false,
                RawValue = component?.ERPManaged,
                IsReadOnly = true
            });

            return Task.CompletedTask;
        }

        public override bool ShouldViewApply(CommercePipelineExecutionContext context, Sitecore.Commerce.EntityViews.EntityView entityView, SellableItem entity)
        {
            var catalogViewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();
            var isConnectView = entityView.Name.Equals(catalogViewsPolicy.ConnectSellableItem, StringComparison.OrdinalIgnoreCase);
            return entityView.Name == catalogViewsPolicy.Master || isConnectView;
        }
    }
}
