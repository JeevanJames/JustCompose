using System.Threading.Tasks;

using ConsoleFx.CmdLine.Program;
using ConsoleFx.CmdLine.Program.HelpBuilders;

namespace JustCompose.Clients.Cli
{
    [Program(Name = "just-compose")]
    public sealed class App : ConsoleProgram
    {
        private static async Task<int> Main()
        {
            var program = new App
            {
                HelpBuilder = new DefaultColorHelpBuilder("help", "h"),
                ErrorHandler = new CustomErrorHandler(),
            };
            program.ScanEntryAssemblyForCommands();
#if DEBUG
            return await program.RunDebugAsync().ConfigureAwait(false);
#else
            return await program.RunWithCommandLineArgsAsync().ConfigureAwait(false);
#endif
        }
    }
}
