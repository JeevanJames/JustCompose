using System.Diagnostics;

using JustCompose.Core;

namespace JustCompose.Composers.Script
{
    public sealed class ScriptContext : IComposerContext
    {
        internal ScriptContext(Process process)
        {
            Process = process;
        }

        public Process Process { get; }
    }
}
