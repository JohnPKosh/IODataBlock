using Business.Common.GenericResponses;
using HubSpot.Services.ModeTypes;
using System.Collections.Generic;

namespace HubSpot.Services.Contacts
{
    /* http://developers.hubspot.com/docs/methods/contacts/contacts-overview */

    public interface IContactService
    {
        #region Raw API Implementation

        #region Create / Update / Delete

        /* http://developers.hubspot.com/docs/methods/contacts/create_contact */
        /* Example URL to POST to:  https://api.hubapi.com/contacts/v1/contact/?hapikey=demo */

        IResponseObject<string, string> Create(string value);

        /* http://developers.hubspot.com/docs/methods/contacts/update_contact */
        /* Example URL to POST to:  https://api.hubapi.com/contacts/v1/contact/vid/61571/profile?hapikey=demo */

        IResponseObject<string, string> Update(string value, int id);

        /* http://developers.hubspot.com/docs/methods/contacts/create_or_update */
        /* Example URL to POST to:  http://api.hubapi.com/contacts/v1/contact/createOrUpdate/email/test@hubspot.com/?hapikey=demo */

        IResponseObject<string, string> Upsert(string value, string email);

        /* http://developers.hubspot.com/docs/methods/contacts/batch_create_or_update */
        /* Example URL to POST to:  "http://api.hubapi.com/contacts/v1/contact/batch/?hapikey=demo" */

        IResponseObject<string, string> BatchUpsert(string value);

        /* http://developers.hubspot.com/docs/methods/contacts/delete_contact */
        /* Example URL:  https://api.hubapi.com/contacts/v1/contact/vid/61571?hapikey=demo */

        IResponseObject<string, string> Delete(int id);

        #endregion Create / Update / Delete

        #region Read Contacts

        /* http://developers.hubspot.com/docs/methods/contacts/get_contacts */
        /* Example URL:  https://api.hubapi.com/contacts/v1/lists/all/contacts/all?hapikey=demo */

        IResponseObject<string, string> SearchAll(int? count = null, int? vidOffset = null, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_recently_updated_contacts */
        /* Example URL:  https://api.hubapi.com/contacts/v1/lists/recently_updated/contacts/recent?hapikey=demo */

        IResponseObject<string, string> SearchRecent(int? count = null, long? timeOffset = null, int? vidOffset = null, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_contact */
        /* Example URL:  https://api.hubapi.com/contacts/v1/contact/vid/215913/profile?hapikey=demo */

        IResponseObject<string, string> GetById(int contactId, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_batch_by_vid */
        /* Example URL:  http://api.hubapi.com/contacts/v1/contact/vids/batch/?portalId=62515&vid=191254&vid=190628&hapikey=demo */

        IResponseObject<string, string> SearchByIds(IEnumerable<int> contactIds, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false, bool includeDeleted = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_contact_by_email */
        /* Example URL:  https://api.hubapi.com/contacts/v1/contact/email/testingapis@hubspot.com/profile?hapikey=demo */

        IResponseObject<string, string> GetByEmail(string email, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_batch_by_email */
        /* Example URL:  http://api.hubapi.com/contacts/v1/contact/emails/batch/?portalId=62515&email=testingapis@hubspot.com&email=testingapisawesomeandstuff@hubspot.com&hapikey=demo */

        IResponseObject<string, string> SearchByEmails(IEnumerable<string> email, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false, bool includeDeleted = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_contact_by_utk */
        /* Example URL:  http://api.hubapi.com/contacts/v1/contact/utk/f844d2217850188692f2610c717c2e9b/profile?hapikey=demo */

        IResponseObject<string, string> GetByTokenId(string contactId, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_batch_by_utk */
        /* Example URL:  http://api.hubapi.com/contacts/v1/contact/utks/batch/?utk=f844d2217850188692f2610c717c2e9b&utk=j94344d22178501692f2610c717c2e9b&hapikey=demo */

        IResponseObject<string, string> SearchByTokenIds(IEnumerable<string> contactIds, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false, bool includeDeleted = false);

        /* http://developers.hubspot.com/docs/methods/contacts/search_contacts */
        /* Example URL:  https://api.hubapi.com/contacts/v1/search/query?q=example&hapikey=demo */

        IResponseObject<string, string> SearchByQuery(string partialmatchNameOrEmail, int? count = null, int? vidOffset = null, IEnumerable<string> properties = null);

        #endregion Read Contacts

        #endregion Raw API Implementation
    }
}