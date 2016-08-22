using Business.Common.System.States;

namespace HubSpot.Models.Properties
{
    public interface IPropertyState
    {
        PropertyTypeListModel Value { get; set; }
        bool IsLoaded { get; }

        void Load(IStateLoader loader);

        bool TryLoad(IStateLoader loader);

        void Save(IStateLoader loader);

        bool TrySave(IStateLoader loader);
    }
}