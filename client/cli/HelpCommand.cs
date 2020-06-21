using System;
using System.Collections.Generic;
using System.Linq;

using ConsoleFx.CmdLine;

using JustCompose.Core;

using static ConsoleFx.ConsoleExtensions.Clr;
using static ConsoleFx.ConsoleExtensions.ConsoleEx;

namespace JustCompose.Clients.Cli
{
    [Command("help", "h")]
    public sealed class HelpCommand : Command
    {
        protected override int HandleCommand()
        {
            List<Type> composerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type => !type.IsAbstract && typeof(Composer).IsAssignableFrom(type))
                .ToList();

            foreach (Type composerType in composerTypes)
            {
                var composer = (Composer)Activator.CreateInstance(composerType, new ComposerProperties());
                IEnumerable<PropertyDescriptor> properties = composer.GetPropertyDescriptors();

                PrintLine($"{Yellow}{composerType.Name}");
                foreach (PropertyDescriptor property in properties)
                {
                    PrintLine($"  {Magenta}{property.Name}");
                    PrintLine($"    Description: {Cyan}{property.Description}");
                    PrintLine($"    Required   : {Cyan}{property.Required}");
                }

                PrintBlank();
            }

            return 0;
        }
    }
}
