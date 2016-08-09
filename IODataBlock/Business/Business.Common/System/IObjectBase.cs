using System;
using Newtonsoft.Json;

namespace Business.Common.System
{
    public interface IObjectBase<T>
    {
        String ToJson(Boolean indented = false);

        void PopulateFromJson(String value);

        void PopulateFromJson(string value, JsonSerializerSettings settings);
    }

}