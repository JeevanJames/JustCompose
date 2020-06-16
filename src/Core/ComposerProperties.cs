using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace JustCompose.Core
{
    [Serializable]
    public sealed class ComposerProperties : Dictionary<string, string>
    {
        public ComposerProperties()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public ComposerProperties(IDictionary<string, string> dictionary)
            : base(dictionary, StringComparer.OrdinalIgnoreCase)
        {
        }

        private ComposerProperties(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        public T Get<T>(string key, T defaultValue = default)
        {
            return TryGet(key, out T value) ? value : defaultValue;
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (!TryGetValue(key, out string stringValue))
            {
                value = default;
                return false;
            }

            Type type = typeof(T);

            if (type == typeof(string))
            {
                value = (T)(object)stringValue;
                return true;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if (converter.CanConvertFrom(typeof(string)))
            {
                value = (T)converter.ConvertFromString(stringValue);
                return true;
            }

            ConstructorInfo matchingCtor = type.GetConstructor(new[] { typeof(string) });
            if (matchingCtor != null)
            {
                value = (T)matchingCtor.Invoke(new object[] { stringValue });
                return true;
            }

            value = default;
            return false;
        }
    }
}
