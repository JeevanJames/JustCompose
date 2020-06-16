using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JustCompose.Core
{
    [Serializable]
#pragma warning disable CA1716 // Identifiers should not match keywords
    public sealed class Step : Dictionary<string, string>
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        public Step(string name, Type composerType)
        {
            Name = name;
            ComposerType = composerType;
        }

        private Step(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string Name { get; }

        public Type ComposerType { get; }
    }
}
