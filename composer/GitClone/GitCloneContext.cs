using JustCompose.Core;

namespace JustCompose.Composers.GitClone
{
    public sealed class GitCloneContext : IComposerContext
    {
        public GitCloneContext(string cloneDirectory)
        {
            CloneDirectory = cloneDirectory;
        }

        public string CloneDirectory { get; }
    }
}
