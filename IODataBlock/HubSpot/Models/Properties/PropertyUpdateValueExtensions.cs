using System;
using System.Collections.Generic;
using System.Linq;
using Business.Common.Extensions;
using Business.Common.System;

namespace HubSpot.Models.Properties
{
    public static class PropertyUpdateValueExtensions
    {

        private static IEnumerable<Type> NumericTypes()
        {
            return new[]
            {
                typeof (byte),
                typeof (sbyte),
                typeof (short),
                typeof (ushort),
                typeof (int),
                typeof (uint),
                typeof (long),
                typeof (ulong),
                typeof (float),
                typeof (double),
                typeof (decimal),
                typeof (byte?),
                typeof (sbyte?),
                typeof (short?),
                typeof (ushort?),
                typeof (int?),
                typeof (uint?),
                typeof (long?),
                typeof (ulong?),
                typeof (float?),
                typeof (double?),
                typeof (decimal?)
            };
        } 

        public static T GetValue<T>(this PropertyUpdateValue item)
        {
            var fieldType = item.PropertyType.fieldType;

            //DateTime? ts = new UnixMsTimestamp(item.Value);

            switch (fieldType)
            {
                case "datetime":
                    if (typeof (T) == typeof (DateTime?))
                    {
                        DateTime? ts = new UnixMsTimestamp(item.Value);
                        return ts.ToOrDefault<T>();
                    }
                    else if (typeof(T) == typeof(DateTime))
                    {
                        DateTime? ts = new UnixMsTimestamp(item.Value);
                        return ts.Value.ToOrDefault<T>();
                    }
                    else throw new ArgumentException($@"Unable to cast {fieldType} to {typeof (T).FullName}!");
                case "number":
                    if (NumericTypes().Contains(typeof(T)))
                    {
                        return item.Value.To<T>();
                    }
                    else throw new ArgumentException($@"Unable to cast {fieldType} to {typeof(T).FullName}!");
                case "bool":
                    if (typeof (T) == typeof (bool) || typeof (T) == typeof (bool?))
                    {
                        return item.Value.To<T>();
                    }
                    else throw new ArgumentException($@"Unable to cast {fieldType} to {typeof(T).FullName}!");
                default:
                    return item.Value.To<T>();
            }
        }


    }
}
