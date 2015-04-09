using Business.Common.System.States;

namespace Business.Common.System.App
{
    public class AppState
    {
        private static AppState _instance = new AppState();

        private AppState()
        {
        }

        public static AppState Instance
        {
            get { return _instance ?? (_instance = new AppState()); }
        }

        private object _value = null;

        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (_value != null) IsLoaded = true;
            }
        }

        public bool IsLoaded { get; private set; }

        public void Load<T>(IStateLoader loader)
        {
            Value = loader.LoadState<T>();
        }

        public bool TryLoad<T>(IStateLoader loader)
        {
            T newValue;
            if (!loader.TryLoadState<T>(out newValue)) return false;
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