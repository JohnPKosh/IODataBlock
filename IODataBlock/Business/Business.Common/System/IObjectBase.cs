using Newtonsoft.Json;

namespace Business.Common.System
{
    public interface IObjectBase<T>
    {
        string ToJson(bool indented = false);

        void PopulateFromJson(string value);

        void PopulateFromJson(string value, JsonSerializerSettings settings);
    }

}