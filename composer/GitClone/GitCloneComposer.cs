using System;
using System.Collections.Generic;
using System.IO;

using JustCompose.Core;

using LibGit2Sharp;

namespace JustCompose.Composers.GitClone
{
    public sealed class GitCloneComposer : Composer
    {
        public GitCloneComposer(ComposerProperties properties)
            : base(properties)
        {
        }

        protected override IComposerContext Up()
        {
            string cloneDirectory = Path.GetFullPath(CloneDirectory);
            var co = new CloneOptions
            {
                BranchName = Branch ?? "master",
                Checkout = true,
                RecurseSubmodules = false,
            };

            Console.WriteLine($"Cloning '{CloneUrl}' to '{cloneDirectory}'.");
            string directory = Repository.Clone(CloneUrl.ToString(), cloneDirectory, co);
            Console.WriteLine($"Cloning completed to {directory}");

            return new GitCloneContext(cloneDirectory);
        }

        protected override void Down(IComposerContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var gitCloneContext = (GitCloneContext)context;

            DeleteDirectory(gitCloneContext.CloneDirectory);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Need to ignore")]
        public static void DeleteDirectory(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            string[] subdirectories = Directory.GetDirectories(directory);

            foreach (string file in files)
            {
                try
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
                catch
                {
                    // Swallow
                }
            }

            foreach (string dir in subdirectories)
                DeleteDirectory(dir);

            try
            {
                Directory.Delete(directory, false);
            }
            catch
            {
                // Swallow
            }
        }

        public Uri CloneUrl => Properties.Get<Uri>(GitCloneComposerKeys.CloneUrl);

        public string Branch => Properties.Get(GitCloneComposerKeys.Branch, "master");

        public string CloneDirectory => Properties.Get(GitCloneComposerKeys.CloneDirectory,
            Directory.GetCurrentDirectory());

        public override IEnumerable<PropertyDescriptor> GetPropertyDescriptors()
        {
            yield return new PropertyDescriptor(GitCloneComposerKeys.CloneUrl)
            {
                Description = "Clone URL of the repository",
                Required = true,
            };

            yield return new PropertyDescriptor(GitCloneComposerKeys.Branch)
            {
                Description = "Branch in the repository to checkout",
                Required = false,
                DefaultValue = "master",
            };

            yield return new PropertyDescriptor(GitCloneComposerKeys.CloneDirectory)
            {
                Description = "Local directory to clone the repository to.",
                Required = false,
                DefaultValue = Directory.GetCurrentDirectory(),
            };
        }
    }

    public static class GitCloneComposerKeys
    {
        public const string CloneUrl = nameof(CloneUrl);
        public const string Branch = nameof(Branch);
        public const string CloneDirectory = nameof(CloneDirectory);
    }
}
