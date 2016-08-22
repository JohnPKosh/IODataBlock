using Business.Common.GenericResponses;

namespace HubSpot.Services.Companies
{
    /* http://developers.hubspot.com/docs/methods/companies/companies-overview */

    public interface ICompanyService
    {
        #region Raw API Implementation

        #region Create / Update / Delete

        /* http://developers.hubspot.com/docs/methods/companies/create_company */
        /* Example URL to POST to:  https://api.hubapi.com/companies/v2/companies/?hapikey=demo */

        IResponseObject<string, string> Create(string value);

        /* http://developers.hubspot.com/docs/methods/companies/update_company */
        /* Example URL to PUT to:  https://api.hubapi.com/companies/v2/companies/10444744?hapikey=demo */

        IResponseObject<string, string> Update(string value, int id);

        /* http://developers.hubspot.com/docs/methods/companies/delete_company */
        /* Example URL to DELETE to:  https://api.hubapi.com/companies/v2/companies/10444744?hapikey=demo&portalId=62515 */

        IResponseObject<string, string> Delete(int id);

        #endregion Create / Update / Delete

        #region Read Methods

        /* http://developers.hubspot.com/docs/methods/companies/get_companies_created */
        /* Example URL:  https://api.hubapi.com/companies/v2/companies/recent/created?hapikey=demo */

        IResponseObject<string, string> SearchRecentlyCreated(int? count = null, long? timeOffset = null);

        /* http://developers.hubspot.com/docs/methods/companies/get_companies_modified */
        /* Example URL:  https://api.hubapi.com/companies/v2/companies/recent/modified?hapikey=demo */

        IResponseObject<string, string> SearchRecentlyUpdated(int? count = null, long? timeOffset = null);

        /* http://developers.hubspot.com/docs/methods/companies/get_company */
        /* Example URL:  https://api.hubapi.com/companies/v2/companies/10444744?hapikey=demo */

        IResponseObject<string, string> GetById(int companyId);

        /* http://developers.hubspot.com/docs/methods/companies/get_companies_by_domain */
        /* Example URL:  https://api.hubapi.com/companies/v2/companies/domain/example.com?hapikey=demo */

        IResponseObject<string, string> GetByDomain(string domain);

        /* http://developers.hubspot.com/docs/methods/companies/get_company_contacts */
        /* Example URL:  https://api.hubapi.com/companies/v2/companies/10444744/contacts?hapikey=demo&portalId=62515 */

        IResponseObject<string, string> SearchContacts(int companyId, int? count = null, int? vidOffset = null);

        /* http://developers.hubspot.com/docs/methods/companies/get_company_contacts_by_id */
        /* Example URL:  https://api.hubapi.com/companies/v2/companies/10444744/vids?hapikey=demo&portalId=62515 */

        IResponseObject<string, string> SearchContactIds(int companyId, int? count = null, int? vidOffset = null);

        #endregion Read Methods

        #region Contact Methods

        /* http://developers.hubspot.com/docs/methods/companies/add_contact_to_company */
        /* Example URL to PUT to:  https://api.hubapi.com/companies/v2/companies/39238082/contacts/270146?hapikey=demo */

        IResponseObject<string, string> AddContactToCompany(int companyId, int contactId);

        /* http://developers.hubspot.com/docs/methods/companies/remove_contact_from_company */
        /* Example URL to DELETE to:  https://api.hubapi.com/companies/v2/companies/39238082/contacts/270146?hapikey=demo */

        IResponseObject<string, string> RemoveContactFromCompany(int companyId, int contactId);

        #endregion Contact Methods

        #endregion Raw API Implementation
    }
}