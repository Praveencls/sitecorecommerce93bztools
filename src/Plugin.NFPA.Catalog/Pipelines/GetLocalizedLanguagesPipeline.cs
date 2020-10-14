using Microsoft.Extensions.Logging;
using Plugin.NFPA.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;

namespace Plugin.NFPA.Catalog.Pipelines
{
    public class GetLocalizedLanguagesPipeline : CommercePipeline<LocalizedTermArgument, IEnumerable<LocalizedTerm>>, IGetLocalizedLanguagesPipeline
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.Sample.Notes.Pipelines.IGetLocalizedProductFeaturesPipelinePipeline" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public GetLocalizedLanguagesPipeline(IPipelineConfiguration<IGetLocalizedLanguagesPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory) { }
    }
}
