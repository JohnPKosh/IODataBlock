using System.Collections.Generic;
using HubSpot.Models;

namespace HubSpot.Services
{
    /* http://developers.hubspot.com/docs/methods/contacts/contacts-overview */

    public interface IContactService
    {
        /* http://developers.hubspot.com/docs/methods/contacts/create_contact */
        /* Example URL to POST to:  https://api.hubapi.com/contacts/v1/contact/?hapikey=demo */

        string CreateContact(string value);

        string CreateContact(ContactDto value);

        /* http://developers.hubspot.com/docs/methods/contacts/update_contact */
        /* Example URL to POST to:  https://api.hubapi.com/contacts/v1/contact/vid/61571/profile?hapikey=demo */

        bool UpdateContact(string value, int id);

        bool UpdateContact(ContactDto value, int id);

        /* http://developers.hubspot.com/docs/methods/contacts/create_or_update */
        /* Example URL to POST to:  http://api.hubapi.com/contacts/v1/contact/createOrUpdate/email/test@hubspot.com/?hapikey=demo */

        string UpsertContact(string value);

        string UpsertContact(ContactDto value);

        /* http://developers.hubspot.com/docs/methods/contacts/batch_create_or_update */
        /* Example URL to POST to:  "http://api.hubapi.com/contacts/v1/contact/batch/?hapikey=demo" */

        string BatchUpsertContacts(string value);

        string BatchUpsertContacts(IEnumerable<ContactDto> value);

        /* http://developers.hubspot.com/docs/methods/contacts/delete_contact */
        /* Example URL:  https://api.hubapi.com/contacts/v1/contact/vid/61571?hapikey=demo */

        bool DeleteContact(int id);

        /* http://developers.hubspot.com/docs/methods/contacts/delete_contact */
        /* Example URL:  https://api.hubapi.com/contacts/v1/contact/vid/61571?hapikey=demo */

        bool GetAllContacts();
    }
}