using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace JustCompose.Core
{
    public sealed class CompositionExecutor : Collection<Composition>
    {
        public async Task ExecuteAsync()
        {
            Composition composition = this.FirstOrDefault();
            if (composition != null)
                await ExecuteAsync(composition).ConfigureAwait(false);
        }

        public Task ExecuteAsync(string compositionName)
        {
            if (compositionName is null)
                throw new ArgumentNullException(nameof(compositionName));
            Composition composition = this.FirstOrDefault(
                c => compositionName.Equals(c.Name, StringComparison.OrdinalIgnoreCase));
            if (composition is null)
            {
                throw new ArgumentException($"Composition named '{compositionName}' could not be found.",
                    nameof(compositionName));
            }

            return ExecuteAsync(composition);
        }

        private static async Task ExecuteAsync(Composition composition)
        {
            var executedSteps = new List<(Composer composer, IComposerContext context)>(composition.Count);

            try
            {
                foreach (Step step in composition)
                {
                    var composer = (Composer)Activator.CreateInstance(step.ComposerType,
                        new ComposerProperties(step));
                    var context = await composer.UpAsync().ConfigureAwait(false);
                    executedSteps.Add((composer, context));
                }
            }
            finally
            {
                for (var i = executedSteps.Count - 1; i >= 0; i--)
                {
                    var (composer, context) = executedSteps[i];
                    await composer.DownAsync(context).ConfigureAwait(false);
                }
            }
        }
    }
}
