using System.IO;
using System.Threading.Tasks;

using ConsoleFx.CmdLine.Program;
using ConsoleFx.CmdLine.Program.HelpBuilders;

using JustCompose.Config.Yaml;
using JustCompose.Core;

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

        protected override async Task<int> HandleCommandAsync()
        {
            //var executor = new CompositionExecutor
            //{
            //    new Composition("ci", "CI composition")
            //    {
            //        new Step(typeof(ScriptComposer))
            //        {
            //            [ScriptComposerKeys.ScriptType] = ScriptType.Powershell.ToString(),
            //            [ScriptComposerKeys.Source] = ScriptSource.Inline.ToString(),
            //            [ScriptComposerKeys.InlineScript] = PowershellScript,
            //        },
            //    },
            //};
            //await executor.ExecuteAsync().ConfigureAwait(false);

            var executor = new CompositionExecutor();
            var file = new FileInfo("just-compose.yml");
            executor.LoadFromYaml(file);
            await executor.ExecuteAsync().ConfigureAwait(false);

            return 0;
        }

//        private const string NativeScript = @"@echo off
//d:
//cd \Code\GitHub\IniFile
//dotnet build -c Release
//tree";

//        private const string PowershellScript = @"Write-Host 'Hello World'
//cd D:\Code\GitHub\ConsoleFx
//dotnet build -c Release";
    }
}
