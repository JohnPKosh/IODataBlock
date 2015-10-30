using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Extensions;
using Business.Common.IO;
using Business.Common.System.States;
using HubSpot.Services;

namespace HubSpot.Models.Contacts
{
    public class PropertyManager
    {
        public PropertyManager(string hapikey, string jsonFilePath = null, TimeSpan? ttl = null)
        {
            _hapiKey = hapikey;
            if (string.IsNullOrWhiteSpace(jsonFilePath)) jsonFilePath = Path.Combine(IOUtility.AppDataFolderPath, @"ContactPropertyList.json");
            _ttl = ttl ?? TimeSpan.FromHours(1); 
            _stateLoader = new JsonFileLoader(new FileInfo(jsonFilePath));
            _loadProperties();
        }

        private void _loadProperties()
        {
            if (PropertyState.Instance.IsLoaded) return;
            if (PropertyState.Instance.TryLoad(_stateLoader))
            {
                if (PropertyState.Instance.Value.LastUpdated.HasValue)
                {
                    if (DateTime.Now.Subtract(_ttl) > PropertyState.Instance.Value.LastUpdated.Value)
                    {
                        TrySetProperyState();
                    }
                }
                else
                {
                    SetPropertyState();  // likely never gets here?
                }
            }
            else
            {
                SetPropertyState();
            }
        }

        private void SetPropertyState()
        {
            var service = new ContactPropertyService(_hapiKey);
            var result = service.GetAllProperties();
            if(result.HasExceptions)throw new Exception(result.ExceptionList.Exceptions.First().Message);
            var data = result.ResponseData.ConvertJson<List<ContactPropertyDto>>();
            PropertyState.Instance.Value = new ContactPropertyListDto
            {
                Properties = data,
                LastUpdated = DateTime.Now
            };
            if (!Directory.Exists(IOUtility.AppDataFolderPath)) Directory.CreateDirectory(IOUtility.AppDataFolderPath);
            PropertyState.Instance.Save(_stateLoader);
        }

        private bool TrySetProperyState()
        {
            try
            {
                SetPropertyState();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private readonly IStateLoader _stateLoader;

        private readonly TimeSpan _ttl;

        private readonly string _hapiKey;

        //public ContactPropertyListDto PropertyList => PropertyState.Instance.Value;

        public DateTime? LastUpdated => PropertyState.Instance.Value.LastUpdated;

        public List<ContactPropertyDto> Properties => PropertyState.Instance.Value.Properties;

        
    }
}
