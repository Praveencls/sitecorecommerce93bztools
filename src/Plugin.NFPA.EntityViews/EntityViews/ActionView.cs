using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.NFPA.EntityViews.EntityViews
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    public abstract class ActionView<TEntity> : View<TEntity>
     where TEntity : CommerceEntity
    {
        public virtual string ActionName => this.ViewName;

        public virtual string ActionDisplayName => this.ViewDisplayName;

        public abstract string ActionIcon { get; }

        public virtual bool RequiresConfirmation => false;

        public virtual bool IsEnabled => true;

        public abstract bool ShouldAddAction(CommercePipelineExecutionContext context, EntityView entityView,
            TEntity entity);

        public override bool ShouldViewApply(CommercePipelineExecutionContext context, EntityView entityView,
            TEntity entity)
        {
            return ActionName.Equals(entityView.Action, StringComparison.OrdinalIgnoreCase);
        }

        public virtual string GetEntityView(CommercePipelineExecutionContext context, EntityView entityView)
        {
            return entityView.Name;
        }

        public abstract Task DoAction(CommercePipelineExecutionContext context,
            EntityView entityView,
            TEntity entity);
    }
}
