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
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;

    [PipelineDisplayName("Plugin.NFPA.EntityViews.Pipelines.Blocks.PopulateActionBlock")]
    public class PopulateActionBlock<TActionView, TEntity>
        : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
        where TActionView : ActionView<TEntity>
        where TEntity : CommerceEntity
    {
        private readonly TActionView _actionView;

        /// <summary>
        /// Gets or sets the commander.
        /// </summary>
        /// <value>
        /// The commander.
        /// </value>
        public CommerceCommander Commander { get; set; }

        public ViewCommander ViewCommander { get; }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:Sitecore.Framework.Pipelines.PipelineBlock" /> class.</summary>
        /// <param name="commander">The commerce commander.</param>
        public PopulateActionBlock(CommerceCommander commander, ViewCommander viewCommander, TActionView actionView)
            : base(null)
        {
            _actionView = actionView;
            this.Commander = commander;
            ViewCommander = viewCommander;
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
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{this.Name}: The argument can not be null");

            var request = this.ViewCommander.CurrentEntityViewArgument(context.CommerceContext);
            if (request == null) return Task.FromResult(arg);

            var entity = request.Entity as TEntity;
            if (entity == null) return Task.FromResult(arg);

            if (!_actionView.ShouldAddAction(context, arg, entity))
                return Task.FromResult(arg);

            var actionPolicy = arg.GetPolicy<ActionsPolicy>();
            actionPolicy.AddAction(
                new EntityActionView
                {
                    Name = _actionView.ActionName,
                    DisplayName = _actionView.ActionDisplayName,
                    Description = string.Empty,
                    IsEnabled = _actionView.IsEnabled,
                    EntityView = _actionView.GetEntityView(context, arg),
                    Icon = _actionView.ActionIcon,
                    RequiresConfirmation = _actionView.RequiresConfirmation
                });

            return Task.FromResult(arg);
        }
    }
}
