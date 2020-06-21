using System.Collections.Generic;

namespace JustCompose.Core
{
    public sealed class PropertyDescriptor
    {
        public PropertyDescriptor(string name, string? description = null, bool required = false)
        {
            Name = name;
            Description = description;
            Required = required;
        }

        public string Name { get; }

        public string? Description { get; set; }

        public bool Required { get; set; }

        public string? DefaultValue { get; set; }

        public IDictionary<string, string> ValidValues { get; } = new Dictionary<string, string>();
    }
}
