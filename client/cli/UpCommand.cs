using System.IO;
using System.Threading.Tasks;

using ConsoleFx.CmdLine;
using ConsoleFx.CmdLine.Program;

using JustCompose.Config.Yaml;
using JustCompose.Core;

using static ConsoleFx.ConsoleExtensions.Clr;
using static ConsoleFx.ConsoleExtensions.ConsoleEx;

namespace JustCompose.Clients.Cli
{
    [Command("up")]
    [Help("Runs the scripts from the just-compose.yml in the current file.")]
    public sealed class UpCommand : Command
    {
        [Argument(Order = 0, Optional = true)]
        [Help("composition name", "Name of the composition to execute.")]
        public string CompositionName { get; set; }

        protected override async Task<int> HandleCommandAsync()
        {
            var executor = new CompositionExecutor();

            string configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "just-compose.yml");
            var file = new FileInfo(configFilePath);
            executor.LoadFromYaml(file);
            executor.OnStatus += Executor_OnStatus;

            if (string.IsNullOrWhiteSpace(CompositionName))
                await executor.ExecuteAsync().ConfigureAwait(false);
            else
                await executor.ExecuteAsync(CompositionName).ConfigureAwait(false);

            return 0;
        }

        private void Executor_OnStatus(object sender, StatusEventArgs<StepStatus> e)
        {
            string message = e.StatusType switch
            {
                StepStatus.Up => $"{Cyan}{e.Message}",
                StepStatus.Down => $"{Cyan}{e.Message}",
                StepStatus.Error => $"{Red}{e.Message}",
                StepStatus.Status => $"{Cyan}{e.Message}",
                StepStatus.Progress => $"{Cyan}{e.Message}",
                _ => string.Empty,
            };
            PrintLine(message);
        }
    }
}
