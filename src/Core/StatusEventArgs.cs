using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace JustCompose.Core
{
    public sealed class StatusEventArgs<TType> : EventArgs
        where TType : Enum
    {
        public StatusEventArgs(TType statusType, string? message = null, object? metadata = null)
        {
            if (message is null && metadata is null)
                throw new ArgumentException($"Both {nameof(message)} and {nameof(metadata)} parameters cannot be null. Specify at least one.");

            StatusType = statusType;

            Message = message is null || metadata is null
                ? message
                : Patterns.PlaceholderPattern.Replace(message, match =>
                {
                    string propertyName = match.Groups[1].Value;
                    PropertyInfo property = metadata.GetType().GetProperty(propertyName,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (!property.CanRead)
                        throw new NotSupportedException($"Property {property.Name} cannot be read.");
                    if (property.GetIndexParameters().Length > 0)
                        throw new NotSupportedException($"Property {property.Name} is an indexed property and is not supported.");
                    object value = property.GetValue(metadata);
                    return value.ToString();
                });
        }

        public TType StatusType { get; }

        public string? Message { get; }
    }

    internal static class Patterns
    {
        internal static readonly Regex PlaceholderPattern = new Regex(@"\{(\w+)\}",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }

    public static class StatusEventArgsExtensions
    {
        public static void Report<TType>(this EventHandler<StatusEventArgs<TType>>? handler,
            TType statusType,
            object metadata)
            where TType : Enum
        {
            EventHandler<StatusEventArgs<TType>>? handlerCopy = handler;
            if (handlerCopy != null)
            {
                var args = new StatusEventArgs<TType>(statusType, null, metadata);
                handlerCopy(null, args);
            }
        }

        public static void Report<TType>(this EventHandler<StatusEventArgs<TType>>? handler,
            TType statusType,
            string message)
            where TType : Enum
        {
            EventHandler<StatusEventArgs<TType>>? handlerCopy = handler;
            if (handlerCopy != null)
            {
                var args = new StatusEventArgs<TType>(statusType, message);
                handlerCopy(null, args);
            }
        }

        public static void Report<TType>(this EventHandler<StatusEventArgs<TType>>? handler,
            TType statusType,
            string message,
            object metadata)
            where TType : Enum
        {
            EventHandler<StatusEventArgs<TType>>? handlerCopy = handler;
            if (handlerCopy != null)
            {
                var args = new StatusEventArgs<TType>(statusType, message, metadata);
                handlerCopy(null, args);
            }
        }
    }
}
