using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.NFPA.EntityViews.Pipelines.Blocks
{
    using Plugin.NFPA.EntityViews.EntityViews;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;

    [PipelineDisplayName("Plugin.NFPA.EntityViews.Pipelines.Blocks.GetViewBlock")]
    public class GetViewBlock<TView, TEntity>
        : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
        where TView : View<TEntity>
        where TEntity : CommerceEntity
    {
        public readonly CommerceCommander CommerceCommander;
        public readonly ViewCommander ViewCommander;
        private readonly TView _view;

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:Sitecore.Framework.Pipelines.PipelineBlock" /> class.</summary>
        /// <param name="commander">The commerce commander.</param>
        public GetViewBlock(CommerceCommander commerceCommander, ViewCommander viewCommander, TView view)
            : base(null)
        {
            CommerceCommander = commerceCommander;
            ViewCommander = viewCommander;
            _view = view;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="arg">
        /// The pipeline argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="PipelineArgument"/>.
        /// </returns>
        public override async Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{this.Name}: The argument can not be null");

            var request = this.ViewCommander.CurrentEntityViewArgument(context.CommerceContext);
            if (request == null) return arg;

            var entity = request.Entity as TEntity;
            if (entity == null) return arg;

            if (!_view.ShouldViewApply(context, arg, entity)) return arg;

            await _view.ModifyView(context, arg, entity);

            return arg;
        }
    }
}
