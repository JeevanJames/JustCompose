using System.Collections.Generic;
using System.Diagnostics;

using YamlDotNet.Serialization;

namespace JustCompose.Config.Yaml.Config
{
    public sealed class YamlConfig
    {
        [YamlMember(Alias = "composers")]
        public IDictionary<string, string> Composers { get; set; }

        [YamlMember(Alias = "compositions")]
        public IDictionary<string, CompositionConfig> Compositions { get; set; }
    }

    [DebuggerDisplay("Composition: {Description} ({Steps.Count} steps)")]
    public sealed class CompositionConfig
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "steps")]
        public IList<StepConfig> Steps { get; set; }
    }

    [DebuggerDisplay("Step: {Composer} ({Properties?.Count} properties")]
    public sealed class StepConfig
    {
        [YamlMember(Alias = "composer")]
        public string Composer { get; set; }

        [YamlMember(Alias = "properties")]
        public IDictionary<string, string> Properties { get; set; }
    }
}
