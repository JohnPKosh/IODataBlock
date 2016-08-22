using Business.Common.System.States;

namespace HubSpot.Models.Properties
{
    public class PropertyState : IPropertyState
    {
        private static PropertyState _instance = new PropertyState();

        private PropertyState()
        {
        }

        public static PropertyState Instance
        {
            get { return _instance ?? (_instance = new PropertyState()); }
        }

        private PropertyTypeListModel _value = null;

        public PropertyTypeListModel Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (_value != null) IsLoaded = true;
            }
        }

        public bool IsLoaded { get; private set; }

        public void Load(IStateLoader loader)
        {
            Value = loader.LoadState<PropertyTypeListModel>();
        }

        public bool TryLoad(IStateLoader loader)
        {
            PropertyTypeListModel newValue;
            if (!loader.TryLoadState<PropertyTypeListModel>(out newValue)) return false;
            Value = newValue;
            return true;
        }

        public void Save(IStateLoader loader)
        {
            loader.SaveState(_value);
        }

        public bool TrySave(IStateLoader loader)
        {
            return loader.TrySaveState(_value);
        }
    }
}