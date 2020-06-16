using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using JustCompose.Core;

namespace JustCompose.Composers.Script
{
    public sealed class ScriptComposer : Composer
    {
        public ScriptComposer(ComposerProperties properties)
            : base(properties)
        {
        }

        protected override IComposerContext Up()
        {
            string program;
            string programArgs;
            string scriptFileExtension;
            if (ScriptType == ScriptType.Native)
            {
                program = "cmd";
                programArgs = "/c";
                scriptFileExtension = "bat";
            }
            else
            {
                program = "powershell";
                programArgs = string.Empty;
                scriptFileExtension = "ps1";
            }

            string scriptFilePath;
            if (Source == ScriptSource.Inline)
            {
                scriptFilePath = Path.ChangeExtension(Path.GetTempFileName(), scriptFileExtension);
                string inlineScript = Properties.Get(ScriptComposerKeys.InlineScript, string.Empty);
                File.WriteAllText(scriptFilePath, inlineScript);
            }
            else
            {
                string filePath = Properties.Get(ScriptComposerKeys.ScriptFilePath, $"./script.{scriptFileExtension}");
                scriptFilePath = Path.GetFullPath(filePath);
            }

            string arguments = $@"{programArgs} ""{scriptFilePath}"" {Arguments}";

            var process = new Process();
            process.StartInfo.FileName = program;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.ErrorDialog = false;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.OutputDataReceived += (_, e) => Console.WriteLine(e.Data);
            process.ErrorDataReceived += (_, e) => Console.Error.WriteLine(e.Data);

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return new ScriptContext(process);
        }

        protected override void Down(IComposerContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var scriptContext = (ScriptContext)context;

            if (scriptContext.Process.HasExited)
                return;

            if (!scriptContext.Process.WaitForExit(5000))
                scriptContext.Process.Kill();
        }

        public ScriptType ScriptType => Properties.Get(ScriptComposerKeys.ScriptType, ScriptType.Native);

        public ScriptSource Source => Properties.Get(ScriptComposerKeys.Source, ScriptSource.Inline);

        public string InlineScript => Properties.Get(ScriptComposerKeys.InlineScript, string.Empty);

        public string ScriptFilePath => Properties.Get<string>(ScriptComposerKeys.ScriptFilePath);

        public string Arguments => Properties.Get<string>(ScriptComposerKeys.Arguments);

        public override IEnumerable<PropertyDescriptor> GetPropertyDescriptors()
        {
            yield return new PropertyDescriptor(ScriptComposerKeys.ScriptType)
            {
                Description = "Type of script to execute",
                DefaultValue = "Powershell",
                ValidValues =
                {
                    ["Native"] = "Executes a native operating system script",
                    ["Powershell"] = "Executes a Powershell script",
                },
            };

            yield return new PropertyDescriptor(ScriptComposerKeys.Source)
            {
                Description = "Source of the script.",
                DefaultValue = "Inline",
                ValidValues =
                {
                    ["Inline"] = "Block of script inline",
                    ["File"] = "Block of script from a file",
                },
            };

            yield return new PropertyDescriptor(ScriptComposerKeys.InlineScript)
            {
                Description = "Inline script to execute",
            };

            yield return new PropertyDescriptor(ScriptComposerKeys.ScriptFilePath)
            {
                Description = "Path to the script file",
            };

            yield return new PropertyDescriptor(ScriptComposerKeys.Arguments)
            {
                Description = "Arguments to the script",
            };
        }
    }

    public static class ScriptComposerKeys
    {
        public const string ScriptType = nameof(ScriptType);
        public const string Source = nameof(Source);
        public const string InlineScript = nameof(InlineScript);
        public const string ScriptFilePath = nameof(ScriptFilePath);
        public const string Arguments = nameof(Arguments);
    }
}
