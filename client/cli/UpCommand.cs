using System.IO;
using System.Threading.Tasks;

using ConsoleFx.CmdLine;
using ConsoleFx.CmdLine.Program;

using JustCompose.Config.Yaml;
using JustCompose.Core;

namespace JustCompose.Clients.Cli
{
    [Command("up")]
    [Help("Runs the scripts from the just-compose.yml in the current file.")]
    public sealed class UpCommand : Command
    {
        protected override async Task<int> HandleCommandAsync()
        {
            var executor = new CompositionExecutor();

            string configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "just-compose.yml");
            var file = new FileInfo(configFilePath);
            executor.LoadFromYaml(file);

            await executor.ExecuteAsync().ConfigureAwait(false);

            return 0;
        }
    }
}
