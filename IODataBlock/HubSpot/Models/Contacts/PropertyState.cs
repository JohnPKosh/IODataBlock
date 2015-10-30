using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.System.App;
using Business.Common.System.States;

namespace HubSpot.Models.Contacts
{
    public class PropertyState
    {
        private static PropertyState _instance = new PropertyState();

        private PropertyState()
        {
        }

        public static PropertyState Instance
        {
            get { return _instance ?? (_instance = new PropertyState()); }
        }

        private ContactPropertyListDto _value = null;

        public ContactPropertyListDto Value
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
            Value = loader.LoadState<ContactPropertyListDto>();
        }

        public bool TryLoad(IStateLoader loader)
        {
            ContactPropertyListDto newValue;
            if (!loader.TryLoadState<ContactPropertyListDto>(out newValue)) return false;
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
