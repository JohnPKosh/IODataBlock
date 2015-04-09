using System.Dynamic;
using Business.Common.System.States;

namespace Business.Common.System.App
{
    public class DynamicAppState
    {
        private static DynamicAppState _instance = new DynamicAppState();

        private DynamicAppState()
        {
        }

        public static DynamicAppState Instance
        {
            get { return _instance ?? (_instance = new DynamicAppState()); }
        }

        private dynamic _value = new ExpandoObject();

        public dynamic Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (_value != null) IsLoaded = true;
            }
        }

        public bool IsLoaded { get; private set; }

        public void Load(IDynamicStateLoader loader)
        {
            Value = loader.LoadState();
        }

        public bool TryLoad(IDynamicStateLoader loader)
        {
            dynamic newValue;
            if (!loader.TryLoadState(out newValue)) return false;
            Value = newValue;
            return true;
        }

        public void Save(IDynamicStateLoader loader)
        {
            loader.SaveState(_value);
        }

        public bool TrySave(IDynamicStateLoader loader)
        {
            return loader.TrySaveState(_value);
        }
    }
}