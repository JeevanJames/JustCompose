using System.Collections.ObjectModel;

namespace JustCompose.Core
{
    public sealed class Composition : Collection<Step>
    {
        public Composition(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }

        public string Description { get; }
    }
}
