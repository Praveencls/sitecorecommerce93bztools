using Plugin.NFPA.Catalog.Components;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.NFPA.Catalog.EntityView
{
    using Plugin.NFPA.EntityViews.EntityViews;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.Search;

    public class SellableItemDetailsActionView : ComponentActionView<SellableItem, SellableItemDetailsComponent>
    {
        public override string ActionIcon => "edit";

        public override string ViewName => nameof(SellableItemDetailsActionView);

        public override string ViewDisplayName => "Edit Sellable Item Details";

        public SellableItemDetailsActionView(PersistEntityCommand persistEntityCommand)
            : base(persistEntityCommand) {
        }

        public override Task DoAction(CommercePipelineExecutionContext context, EntityView entityView, SellableItem entity, SellableItemDetailsComponent component)
        {
            foreach (var property in entityView.Properties)
            {
                if (property.Name == nameof(SellableItemDetailsComponent.FeaturedDate))
                {
                    component.FeaturedDate = DateTimeOffset.Parse(property.Value);
                }

                if (property.Name == nameof(SellableItemDetailsComponent.ERPManaged))
                {
                    component.ERPManaged = bool.Parse(property.Value);
                }

                if (property.Name == nameof(SellableItemDetailsComponent.MaxQuantity))
                {
                    component.MaxQuantity = property.Value;
                }

                if (property.Name == nameof(SellableItemDetailsComponent.MinQuantity))
                {
                    component.MinQuantity = property.Value;
                }
            }

            return Task.CompletedTask;
        }

        public override Task ModifyView(CommercePipelineExecutionContext context, EntityView entityView, SellableItem entity, SellableItemDetailsComponent component)
        {
            var policy2 = context.CommerceContext.Environment.GetComponent<PolicySetsComponent>().GetPolicy<SearchScopePolicy>();
            var policyList = new List<Policy>
            {
                new Policy
                {
                    PolicyId = "EntityType",
                    Models = new List<Model>
                    {
                        new Model { Name = nameof(SellableItem) }
                    }
                }
            };
            policyList.Add(policy2);

            entityView.Properties.Add(new ViewProperty
            {
                Name = nameof(SellableItemDetailsComponent.MaxQuantity),
                DisplayName = "Max Quantity",
                IsRequired = false,
                RawValue = component.MaxQuantity,
                IsReadOnly = false
            });

            entityView.Properties.Add(new ViewProperty
            {
                Name = nameof(SellableItemDetailsComponent.MinQuantity),
                DisplayName = "Min Quantity",
                IsRequired = false,
                RawValue = component.MinQuantity,
                IsReadOnly = false
            });

            entityView.Properties.Add(new ViewProperty
            {
                Name = nameof(SellableItemDetailsComponent.FeaturedDate),
                DisplayName = "Featured Date",
                IsRequired = false,
                RawValue = component.FeaturedDate,
                IsReadOnly = false
            });

            entityView.Properties.Add(new ViewProperty
            {
                Name = nameof(SellableItemDetailsComponent.ERPManaged),
                DisplayName = "ERP Managed",
                IsRequired = false,
                RawValue = component.ERPManaged,
                IsReadOnly = false
            });

            return Task.CompletedTask;
        }

        public override bool ShouldAddAction(CommercePipelineExecutionContext context, EntityView entityView, SellableItem entity) => entityView.Name.Equals(nameof(SellableItemDetailsView));
    }
}
