using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace JustCompose.Core
{
    public sealed class CompositionExecutor : Collection<Composition>
    {
        public event EventHandler<StatusEventArgs<StepStatus>>? OnStatus;

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

        private async Task ExecuteAsync(Composition composition)
        {
            IReadOnlyList<(Composer composer, IComposerContext context, Step step)>? executedSteps = null;

            try
            {
                executedSteps = await ExecuteUpAsync(composition).ConfigureAwait(false);
            }
            finally
            {
                if (executedSteps != null)
                    await ExecuteDownAsync(executedSteps).ConfigureAwait(false);
            }
        }

        private async Task<IReadOnlyList<(Composer composer, IComposerContext context, Step step)>> ExecuteUpAsync(
            Composition composition)
        {
            var executedSteps = new List<(Composer composer, IComposerContext context, Step step)>(composition.Count);

            foreach (Step step in composition)
            {
                OnStatus.Report(StepStatus.Up,
                    "Executing step up '{StepName}'.",
                    new { StepName = step.Name });

                var composer = (Composer)Activator.CreateInstance(step.ComposerType,
                    new ComposerProperties(step));

                try
                {
                    var context = await composer.UpAsync().ConfigureAwait(false);
                    executedSteps.Add((composer, context, step));
                }
                catch (Exception ex)
                {
                    OnStatus.Report(StepStatus.Error,
                        "Error occurred in step up '{StepName}' - '{ErrorMessage}'.",
                        new { StepName = step.Name, ErrorMessage = ex.Message });
                    break;
                }
            }

            return executedSteps;
        }

        private async Task ExecuteDownAsync(
            IReadOnlyList<(Composer composer, IComposerContext context, Step step)> executedSteps)
        {
            for (var i = executedSteps.Count - 1; i >= 0; i--)
            {
                var (composer, context, step) = executedSteps[i];

                OnStatus.Report(StepStatus.Down,
                    "Executing step down '{StepName}'.",
                    new { StepName = step.Name });

                try
                {
                    await composer.DownAsync(context).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    OnStatus.Report(StepStatus.Error,
                        "Error occurred in step down '{StepName}' - '{ErrorMessage}'.",
                        new { StepName = step.Name, ErrorMessage = ex.Message });
                }
            }
        }
    }

    public enum StepStatus
    {
        Up,
        Down,
        Status,
        Progress,
        Error,
    }
}
