using System.Collections.Generic;
using Business.Common.Responses;
using HubSpot.Models;
using HubSpot.Models.Contacts;
using HubSpot.Models.Properties;
using HubSpot.Services.ModeTypes;

namespace HubSpot.Services
{
    /* http://developers.hubspot.com/docs/methods/contacts/contacts-overview */

    public interface IContactService
    {
        #region Raw API Implementation

        #region Create / Update / Delete

        /* http://developers.hubspot.com/docs/methods/contacts/create_contact */
        /* Example URL to POST to:  https://api.hubapi.com/contacts/v1/contact/?hapikey=demo */

        IResponseObject<string, string> CreateContact(string value);

        /* http://developers.hubspot.com/docs/methods/contacts/update_contact */
        /* Example URL to POST to:  https://api.hubapi.com/contacts/v1/contact/vid/61571/profile?hapikey=demo */

        IResponseObject<string, string> UpdateContact(string value, int id);

        /* http://developers.hubspot.com/docs/methods/contacts/create_or_update */
        /* Example URL to POST to:  http://api.hubapi.com/contacts/v1/contact/createOrUpdate/email/test@hubspot.com/?hapikey=demo */

        IResponseObject<string, string> UpsertContact(string value, string email);

        /* http://developers.hubspot.com/docs/methods/contacts/batch_create_or_update */
        /* Example URL to POST to:  "http://api.hubapi.com/contacts/v1/contact/batch/?hapikey=demo" */

        IResponseObject<string, string> BatchUpsertContacts(string value);

        /* http://developers.hubspot.com/docs/methods/contacts/delete_contact */
        /* Example URL:  https://api.hubapi.com/contacts/v1/contact/vid/61571?hapikey=demo */

        IResponseObject<string, string> DeleteContact(int id);

        #endregion Create / Update / Delete

        #region Read Contacts

        /* http://developers.hubspot.com/docs/methods/contacts/get_contacts */
        /* Example URL:  https://api.hubapi.com/contacts/v1/lists/all/contacts/all?hapikey=demo */

        IResponseObject<string, string> GetAllContacts(int? count = null, int? vidOffset = null, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_recently_updated_contacts */
        /* Example URL:  https://api.hubapi.com/contacts/v1/lists/recently_updated/contacts/recent?hapikey=demo */

        IResponseObject<string, string> GetRecentContacts(int? count = null, long? timeOffset = null, int? vidOffset = null, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_contact */
        /* Example URL:  https://api.hubapi.com/contacts/v1/contact/vid/215913/profile?hapikey=demo */

        IResponseObject<string, string> GetContactById(int contactId, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_batch_by_vid */
        /* Example URL:  http://api.hubapi.com/contacts/v1/contact/vids/batch/?portalId=62515&vid=191254&vid=190628&hapikey=demo */

        IResponseObject<string, string> GetContactsByIds(IEnumerable<int> contactIds, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false, bool includeDeleted = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_contact_by_email */
        /* Example URL:  https://api.hubapi.com/contacts/v1/contact/email/testingapis@hubspot.com/profile?hapikey=demo */

        IResponseObject<string, string> GetContactByEmail(string email, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_batch_by_email */
        /* Example URL:  http://api.hubapi.com/contacts/v1/contact/emails/batch/?portalId=62515&email=testingapis@hubspot.com&email=testingapisawesomeandstuff@hubspot.com&hapikey=demo */

        IResponseObject<string, string> GetContactsByEmails(IEnumerable<string> email, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false, bool includeDeleted = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_contact_by_utk */
        /* Example URL:  http://api.hubapi.com/contacts/v1/contact/utk/f844d2217850188692f2610c717c2e9b/profile?hapikey=demo */

        IResponseObject<string, string> GetContactByTokenId(string contactId, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_batch_by_utk */
        /* Example URL:  http://api.hubapi.com/contacts/v1/contact/utks/batch/?utk=f844d2217850188692f2610c717c2e9b&utk=j94344d22178501692f2610c717c2e9b&hapikey=demo */

        IResponseObject<string, string> GetContactsByTokenIds(IEnumerable<string> contactIds, IEnumerable<string> properties = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false, bool includeDeleted = false);

        /* http://developers.hubspot.com/docs/methods/contacts/search_contacts */
        /* Example URL:  https://api.hubapi.com/contacts/v1/search/query?q=example&hapikey=demo */

        IResponseObject<string, string> GetContactsByQuery(string partialmatchNameOrEmail, int? count = null, int? vidOffset = null, IEnumerable<string> properties = null);

        #endregion Read Contacts 

        #endregion
    }
}