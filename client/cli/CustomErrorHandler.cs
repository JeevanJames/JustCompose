using System;

using ConsoleFx.CmdLine.Program;

using static ConsoleFx.ConsoleExtensions.Clr;
using static ConsoleFx.ConsoleExtensions.ConsoleEx;

namespace JustCompose.Clients.Cli
{
    internal sealed class CustomErrorHandler : ErrorHandler
    {
        public override int HandleError(Exception ex)
        {
            PrintLine($"{Red}{ex}");
            return -1;
        }
    }
}
