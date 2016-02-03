using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Business.Common.Extensions;
using Business.Common.IO;
using Business.Common.System.States;
using HubSpot.Models.Properties;
using HubSpot.Services;

namespace HubSpot.Models.Companies
{
    public class CompanyPropertyManager : IPropertyManager
    {
        public CompanyPropertyManager(IPropertyService propertyService, IStateLoader stateLoader, TimeSpan? ttl = null)
        {
            _propertyService = propertyService;
            _ttl = ttl ?? TimeSpan.FromHours(1);
            _stateLoader = stateLoader;
            _loadProperties();
        }

        private void _loadProperties()
        {
            if (CompanyPropertyState.Instance.IsLoaded) return;
            if (CompanyPropertyState.Instance.TryLoad(_stateLoader))
            {
                if (CompanyPropertyState.Instance.Value.LastUpdated.HasValue)
                {
                    if (DateTime.Now.Subtract(_ttl) > CompanyPropertyState.Instance.Value.LastUpdated.Value)
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
            var result = _propertyService.GetAllProperties();
            if(result.HasExceptions)throw new Exception(result.ExceptionList.Exceptions.First().Message);
            var data = result.ResponseData.ConvertJson<List<PropertyTypeModel>>();
            CompanyPropertyState.Instance.Value = new PropertyTypeListModel
            {
                Properties = data,
                LastUpdated = DateTime.Now
            };
            if (!Directory.Exists(IOUtility.AppDataFolderPath)) Directory.CreateDirectory(IOUtility.AppDataFolderPath);
            CompanyPropertyState.Instance.Save(_stateLoader);
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

        private readonly IPropertyService _propertyService;

        public DateTime? LastUpdated => CompanyPropertyState.Instance.Value.LastUpdated;

        public List<PropertyTypeModel> Properties => CompanyPropertyState.Instance.Value.Properties;

        
    }
}
