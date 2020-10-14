using Microsoft.Extensions.Logging;
using Plugin.NFPA.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;

namespace Plugin.NFPA.Catalog.Pipelines
{
    public class GetLocalizedFormatsPipeline : CommercePipeline<LocalizedFormatArgument, IEnumerable<LocalizedTerm>>, IGetLocalizedFormatsPipeline
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.Sample.Notes.Pipelines.IGetLocalizedProductFeaturesPipelinePipeline" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public GetLocalizedFormatsPipeline(IPipelineConfiguration<IGetLocalizedFormatsPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        { }
    }
}
