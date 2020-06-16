using System.Collections.Generic;
using System.Diagnostics;

using YamlDotNet.Serialization;

namespace JustCompose.Config.Yaml.Config
{
    [DebuggerDisplay("Step: {Composer} ({Properties?.Count} properties")]
    public sealed class StepConfig
    {
        [YamlMember(Alias = "composer")]
        public string Composer { get; set; }

        [YamlMember(Alias = "properties")]
        public IDictionary<string, string> Properties { get; set; }
    }
}
