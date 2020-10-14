using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Search;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.NFPA.Catalog.IndexHandler
{
    public class ProductVariantFieldHandler : AbstractIndexFieldHandler
    {
        public override object ComposeValue(object source, ConcurrentDictionary<string, object> context)
        {
            if (source == null || context == null || !(source is CatalogItemBase))
                return (object)Enumerable.Empty<string>();
            switch (source)
            {
                case Category category:
                    return (object)new List<string>()
          {
            category.Name ?? string.Empty,
            category.DisplayName ?? string.Empty,
            category.SitecoreId ?? string.Empty,
            category.ChildrenSellableItemList ?? string.Empty
          };
                case SellableItem sellableItem:
                    string str1 = string.Empty;

                    List<string> list = new List<string>();

                    var itemvariations = sellableItem.GetComponent<ItemVariationsComponent>().ChildComponents
                        .OfType<ItemVariationComponent>();

                    foreach (var variation in itemvariations)
                    {
                        //var variationPropertyPolicy = variation.GetPolicy<VariationPropertyPolicy>();
                        //var testPIN = GetVariationProperty(variation, "PIN");

                        list.Add(variation.Id + "#" +  GetVariationProperty(variation, "PIN"));
                    }


                    //var variationPropertyPolicy = context.CommerceContext.Environment.GetPolicy<VariationPropertyPolicy>();
                    //foreach (var variation in variations)
                    //{
                    //    var variationView = variantsView.ChildViews.FirstOrDefault(c => ((EntityView)c).ItemId == variation.Id) as EntityView;
                    //    PopulateVariationProperties(variationView, variation, variationPropertyPolicy);
                    //}

                    //.Select<ItemVariationComponent, string>((Func<ItemVariationComponent, string>)(x => x.Id)));


                    //if (sellableItem.HasComponent<CatalogsComponent>())
                    //{
                    //    CatalogHierarchyInformation hierarchyInformation = context.Values.OfType<CatalogHierarchyInformation>().FirstOrDefault<CatalogHierarchyInformation>();
                    //    if (hierarchyInformation != null)
                    //    {
                    //        CatalogHierarchyNode catalogNode = hierarchyInformation.Nodes.FirstOrDefault<CatalogHierarchyNode>();
                    //        List<string> list = sellableItem.EntityComponents.OfType<CatalogsComponent>().Where<CatalogsComponent>((Func<CatalogsComponent, bool>)(x => x.Catalogs.Any<CatalogComponent>((Func<CatalogComponent, bool>)(catalog =>
                    //        {
                    //            string name = catalog.Name;
                    //            CatalogHierarchyNode catalogHierarchyNode = catalogNode;
                    //            string str2 = catalogHierarchyNode != null ? catalogHierarchyNode.EntityId.SimplifyEntityName() : (string)null;
                    //            return name.Equals(str2, StringComparison.OrdinalIgnoreCase);
                    //        })))).SelectMany<CatalogsComponent, CatalogComponent>((Func<CatalogsComponent, IEnumerable<CatalogComponent>>)(x => x.Catalogs)).Select<CatalogComponent, string>((Func<CatalogComponent, string>)(x => x.ItemDefinition)).ToList<string>();
                    //        if (list.Any<string>())
                    //            str1 = list.FirstOrDefault<string>();
                    //    }
                    //}
                    return (object)list;

                default:
                    return (object)Enumerable.Empty<string>();
            }
        }

        private string GetVariantProperty(ItemVariationComponent variation, VariationPropertyPolicy variationPropertyPolicy, string v)
        {
            foreach (var variationProperty in variationPropertyPolicy.PropertyNames)
            {
                var property = GetVariationProperty(variation, variationProperty);
                if (variationProperty == "PIN")
                    return (string)property;
            }

            return string.Empty;
        }

        protected virtual object GetVariationProperty(ItemVariationComponent variationComponent, string variationProperty)
        {
            foreach (var component in variationComponent.ChildComponents)
            {
                var property = component.GetType().GetProperty(variationProperty);
                if (property != null)
                {
                    return property.GetValue(component);
                }
            }

            return null;
        }
    }
}
