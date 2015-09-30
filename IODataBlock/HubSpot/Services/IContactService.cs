using System.Collections.Generic;
using Business.Common.Responses;
using HubSpot.Models;

namespace HubSpot.Services
{
    /* http://developers.hubspot.com/docs/methods/contacts/contacts-overview */

    public interface IContactService
    {
        #region Create / Update / Delete

        /* http://developers.hubspot.com/docs/methods/contacts/create_contact */
        /* Example URL to POST to:  https://api.hubapi.com/contacts/v1/contact/?hapikey=demo */

        IResponseObject CreateContact(string value);

        IResponseObject CreateContact(ContactDto value);

        /* http://developers.hubspot.com/docs/methods/contacts/update_contact */
        /* Example URL to POST to:  https://api.hubapi.com/contacts/v1/contact/vid/61571/profile?hapikey=demo */

        IResponseObject UpdateContact(string value, int id);

        IResponseObject UpdateContact(ContactDto value, int id);

        /* http://developers.hubspot.com/docs/methods/contacts/create_or_update */
        /* Example URL to POST to:  http://api.hubapi.com/contacts/v1/contact/createOrUpdate/email/test@hubspot.com/?hapikey=demo */

        IResponseObject UpsertContact(string value);

        IResponseObject UpsertContact(ContactDto value);

        /* http://developers.hubspot.com/docs/methods/contacts/batch_create_or_update */
        /* Example URL to POST to:  "http://api.hubapi.com/contacts/v1/contact/batch/?hapikey=demo" */

        IResponseObject BatchUpsertContacts(string value);

        IResponseObject BatchUpsertContacts(IEnumerable<ContactDto> value);

        /* http://developers.hubspot.com/docs/methods/contacts/delete_contact */
        /* Example URL:  https://api.hubapi.com/contacts/v1/contact/vid/61571?hapikey=demo */

        IResponseObject DeleteContact(int id);

        #endregion Create / Update / Delete

        #region Read Contacts

        /* http://developers.hubspot.com/docs/methods/contacts/get_contacts */
        /* Example URL:  https://api.hubapi.com/contacts/v1/contact/vid/61571?hapikey=demo */

        IEnumerable<IResponseObject> GetAllContacts(int? count = null, int? vidOffset = null, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_recently_updated_contacts */
        /* Example URL:  https://api.hubapi.com/contacts/v1/lists/recently_updated/contacts/recent?hapikey=demo */

        IEnumerable<IResponseObject> GetRecentContacts(int? count = null, long? timeOffset = null, int? vidOffset = null, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_contact */
        /* Example URL:  https://api.hubapi.com/contacts/v1/contact/vid/215913/profile?hapikey=demo */

        IResponseObject GetContactById(int? contactId, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_batch_by_vid */
        /* Example URL:  http://api.hubapi.com/contacts/v1/contact/vids/batch/?portalId=62515&vid=191254&vid=190628&hapikey=demo */

        IResponseObject GetContactsByIds(IEnumerable<int> contactIds, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false, bool includeDeleted = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_contact_by_email */
        /* Example URL:  https://api.hubapi.com/contacts/v1/contact/email/testingapis@hubspot.com/profile?hapikey=demo */

        IResponseObject GetContactByEmail(string email, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_batch_by_email */
        /* Example URL:  http://api.hubapi.com/contacts/v1/contact/emails/batch/?portalId=62515&email=testingapis@hubspot.com&email=testingapisawesomeandstuff@hubspot.com&hapikey=demo */

        IResponseObject GetContactsByEmails(IEnumerable<string> email, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false, bool includeDeleted = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_contact_by_utk */
        /* Example URL:  http://api.hubapi.com/contacts/v1/contact/utk/f844d2217850188692f2610c717c2e9b/profile?hapikey=demo */

        IResponseObject GetContactByTokenId(int? contactId, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false);

        /* http://developers.hubspot.com/docs/methods/contacts/get_batch_by_utk */
        /* Example URL:  http://api.hubapi.com/contacts/v1/contact/utks/batch/?utk=f844d2217850188692f2610c717c2e9b&utk=j94344d22178501692f2610c717c2e9b&hapikey=demo */

        IResponseObject GetContactsByTokenIds(IEnumerable<int> contactIds, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false, bool includeDeleted = false);

        /* http://developers.hubspot.com/docs/methods/contacts/search_contacts */
        /* Example URL:  https://api.hubapi.com/contacts/v1/search/query?q=example&hapikey=demo */

        IResponseObject GetContactsByQuery(string partialmatchNameOrEmail, int? count = null, int? vidOffset = null, string property = null);

        #endregion Read Contacts
    }
}