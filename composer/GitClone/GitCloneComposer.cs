using JustCompose.Core;

namespace JustCompose.Composers.GitClone
{
    public sealed class GitCloneComposer : Composer
    {
        public GitCloneComposer(ComposerProperties properties)
            : base(properties)
        {
        }
    }

    public sealed class GitCloneContext : IComposerContext
    {
    }
}
