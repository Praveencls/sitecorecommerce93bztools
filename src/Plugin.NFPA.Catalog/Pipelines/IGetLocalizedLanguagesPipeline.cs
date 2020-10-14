using System.Collections.Generic;

namespace Plugin.NFPA.Catalog.Pipelines
{
    using Plugin.NFPA.Catalog.Pipelines.Arguments;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;
    public interface IGetLocalizedLanguagesPipeline : IPipeline<LocalizedTermArgument, IEnumerable<LocalizedTerm>, CommercePipelineExecutionContext> { }
}
