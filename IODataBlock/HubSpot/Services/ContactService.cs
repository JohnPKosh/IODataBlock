using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Responses;
using HubSpot.Models;

namespace HubSpot.Services
{
    public class ContactService : IContactService
    {
        public IResponseObject CreateContact(string value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject CreateContact(ContactDto value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject UpdateContact(string value, int id)
        {
            throw new NotImplementedException();
        }

        public IResponseObject UpdateContact(ContactDto value, int id)
        {
            throw new NotImplementedException();
        }

        public IResponseObject UpsertContact(string value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject UpsertContact(ContactDto value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject BatchUpsertContacts(string value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject BatchUpsertContacts(IEnumerable<ContactDto> value)
        {
            throw new NotImplementedException();
        }

        public IResponseObject DeleteContact(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IResponseObject> GetAllContacts(int? count = null, int? vidOffset = null, string property = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IResponseObject> GetRecentContacts(int? count = null, long? timeOffset = null, int? vidOffset = null, string property = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false)
        {
            throw new NotImplementedException();
        }

        public IResponseObject GetContactById(int? contactId, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only,
            FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false)
        {
            throw new NotImplementedException();
        }

        public IResponseObject GetContactsByIds(IEnumerable<int> contactIds, string property = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false, bool includeDeleted = false)
        {
            throw new NotImplementedException();
        }

        public IResponseObject GetContactByEmail(string email, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only,
            FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false)
        {
            throw new NotImplementedException();
        }

        public IResponseObject GetContactsByEmails(IEnumerable<string> email, string property = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false, bool includeDeleted = false)
        {
            throw new NotImplementedException();
        }

        public IResponseObject GetContactByTokenId(int? contactId, string property = null, PropertyModeType propertyMode = PropertyModeType.value_only,
            FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest, bool showListMemberships = false)
        {
            throw new NotImplementedException();
        }

        public IResponseObject GetContactsByTokenIds(IEnumerable<int> contactIds, string property = null,
            PropertyModeType propertyMode = PropertyModeType.value_only, FormSubmissionModeType formSubmissionMode = FormSubmissionModeType.Newest,
            bool showListMemberships = false, bool includeDeleted = false)
        {
            throw new NotImplementedException();
        }

        public IResponseObject GetContactsByQuery(string partialmatchNameOrEmail, int? count = null, int? vidOffset = null,
            string property = null)
        {
            throw new NotImplementedException();
        }
    }
}
