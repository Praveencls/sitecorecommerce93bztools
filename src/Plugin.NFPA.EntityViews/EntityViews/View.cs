using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.NFPA.EntityViews.EntityViews
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;

    public abstract class View<TEntity>
        where TEntity : CommerceEntity
    {
        public abstract string ViewName { get; }

        public abstract string ViewDisplayName { get; }

        public virtual bool ShouldViewApply(CommercePipelineExecutionContext context, EntityView entityView,
            TEntity entity)
        {
            return string.IsNullOrWhiteSpace(entityView.Action);
        }

        public abstract Task ModifyView(CommercePipelineExecutionContext context, EntityView entityView, TEntity entity);
    }
}
