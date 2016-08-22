using Newtonsoft.Json.Linq;
using NsRest.Search;
using NsRest.Services;
using System.Linq;

namespace BasicTests.Integration.NsRest.Sandbox
{
    public class NsRestContactHelper
    {
        public NsRestContactHelper()
        {
            //configMgr = new ConfigMgr();
            ////NsBaseUrl = configMgr.GetAppSetting("nsbaseurl");
            //NsBaseUrl = configMgr.GetAppSetting("nssandboxurl");
            //NsAccount = configMgr.GetAppSetting("nsaccount");
            //NsEmail = configMgr.GetAppSetting("nsemail");
            //NsPassword = configMgr.GetAppSetting("nspassword");
            //NsRole = configMgr.GetAppSetting("nsrole");

            //scriptSettings = new Dictionary<string, INetSuiteScriptSetting>
            //{
            //    {"crud", NetSuiteScriptSetting.Create("customscript_record_crud", "customdeploy_record_crud")},
            //    {"search", NetSuiteScriptSetting.Create("customscript_record_search", "customdeploy_record_search")}
            //};
            //baseService = BaseService.Create(NsBaseUrl, NetSuiteLogin.Create(NsAccount, NsEmail, NsPassword, NsRole), scriptSettings);

            baseService = BaseService.Create(true);
            TypeName = "contact";
        }

        //private ConfigMgr configMgr { get; set; }

        //private string NsBaseUrl { get; set; }
        //private string NsAccount { get; set; }
        //private string NsEmail { get; set; }
        //private string NsRole { get; set; }
        //private string NsPassword { get; set; }
        private BaseService baseService { get; set; }

        //private IDictionary<string, INetSuiteScriptSetting> scriptSettings { get; set; }
        private string TypeName { get; set; }

        public JObject SearchJObjectsByEmail(string emailAddress)
        {
            var columns = new[] { "company", "email", "phone" };
            var filters = new[] { NsSearchFilter.NewStringFilter("email", SearchStringFieldOperatorType.Is, emailAddress) };

            var response = baseService.SearchJObjects(TypeName, filters, columns, null, "search");
            var contact = response.ResponseData.FirstOrDefault();
            //var emailItem = contact["columns"]["email"];
            //var email = emailItem.Value<string>();
            //var companyItem = contact["columns"]["company"];
            //var companyName = companyItem["name"].Value<string>();
            //var companyid = companyItem["internalid"].Value<string>();
            //var phoneItem = contact["columns"]["phone"];
            //if (phoneItem != null)
            //{
            //    var phone = phoneItem.Value<string>();
            //}
            return contact;
        }
    }
}