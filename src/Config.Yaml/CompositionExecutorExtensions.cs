using System;
using System.IO;

using Humanizer;

using JustCompose.Config.Yaml.Config;
using JustCompose.Core;

using YamlDotNet.Serialization;

namespace JustCompose.Config.Yaml
{
    public static class CompositionExecutorExtensions
    {
        public static void LoadFromYaml(this CompositionExecutor executor, FileInfo file)
        {
            if (executor is null)
                throw new ArgumentNullException(nameof(executor));
            if (file is null)
                throw new ArgumentNullException(nameof(file));

            var serializer = new Deserializer();
            string yaml = File.ReadAllText(file.FullName);
            var config = serializer.Deserialize<YamlConfig>(yaml);

            foreach ((string name, CompositionConfig compositionCfg) in config.Compositions)
            {
                if (compositionCfg.Steps is null)
                    continue;

                var composition = new Composition(name, compositionCfg.Description);

                foreach (StepConfig stepCfg in compositionCfg.Steps)
                {
                    if (!config.Composers.TryGetValue(stepCfg.Composer, out string composerTypeName))
                        throw new InvalidOperationException($"Cannot find composer named '{stepCfg.Composer}.");
                    Type composerType = Type.GetType(composerTypeName);
                    var step = new Step(composerType);
                    foreach (var (key, value) in stepCfg.Properties)
                        step.Add(key.Pascalize(), value);
                    composition.Add(step);
                }

                executor.Add(composition);
            }
        }
    }
}
