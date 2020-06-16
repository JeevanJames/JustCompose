using System.Collections.Generic;

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
}
