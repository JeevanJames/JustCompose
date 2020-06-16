using System.Collections.Generic;
using System.Diagnostics;

using YamlDotNet.Serialization;

namespace JustCompose.Config.Yaml.Config
{
    [DebuggerDisplay("Composition: {Description} ({Steps.Count} steps)")]
    public sealed class CompositionConfig
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "steps")]
        public IList<StepConfig> Steps { get; set; }
    }
}
