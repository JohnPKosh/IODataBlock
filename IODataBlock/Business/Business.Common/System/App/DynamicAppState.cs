﻿using Business.Common.System.States;
using System.Dynamic;

namespace Business.Common.System.App
{
    public class DynamicAppState
    {
        private static DynamicAppState _instance = new DynamicAppState();

        private DynamicAppState()
        {
        }

        public static DynamicAppState Instance => _instance ?? (_instance = new DynamicAppState());

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

        public void Load(IDynamicLoader loader)
        {
            Value = loader.Load();
        }

        public bool TryLoad(IDynamicLoader loader)
        {
            dynamic newValue;
            if (!loader.TryLoad(out newValue)) return false;
            Value = newValue;
            return true;
        }

        public void Save(IDynamicLoader loader)
        {
            loader.Save(_value);
        }

        public bool TrySave(IDynamicLoader loader)
        {
            return loader.TrySave(_value);
        }
    }
}