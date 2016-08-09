using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Exchange.WebServices.Data;

namespace Business.EWS.Mail
{
    public class ExchangeEmailSearch
    {

        #region Fields

        private readonly ExchangeService _service;

        #endregion

        #region Credential

        readonly string _serviceUrl;
        readonly string _userName;
        readonly string _password;

        #endregion

        #region Constructors

        public ExchangeEmailSearch(string serviceUrl, string userName, string password)
        {
            _serviceUrl = serviceUrl;
            _userName = userName;
            _password = password;

            _service = new ExchangeService(ExchangeVersion.Exchange2010_SP2)
            {
                Credentials = new NetworkCredential(_userName, _password),
                Url = new Uri(_serviceUrl)
            };
        }

        #endregion

        #region Methods

        public IEnumerable<Item> SearchInboxContainsSubstring(string filter, int maxResults = 50)
        {

            ExtendedPropertyDefinition def = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "TempId", MapiPropertyType.String);


            // Create a search collection that contains your search conditions.
            var searchFilterCollection = new List<SearchFilter>
            {
                new SearchFilter.ContainsSubstring(ItemSchema.Subject, filter),
                new SearchFilter.ContainsSubstring(ItemSchema.Body, filter)
            };

            // Create the search filter with a logical operator and your search parameters.
            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.Or, searchFilterCollection.ToArray());

            // Limit the view to 50 items.
            var view = new ItemView(maxResults)
            {
                // Limit the property set to the property ID for the base property set, and the subject and item class for the additional properties to retrieve.
                //PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.ItemClass),
                PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, def),
                // Setting the traversal to shallow will return all non-soft-deleted items in the specified folder.
                Traversal = ItemTraversal.Shallow
            };

            // Send the request to search the Inbox and get the results.
            var findResults = _service.FindItems(WellKnownFolderName.Inbox, searchFilter, view);
            return new List<Item>(findResults.Items);
        }


        public IEnumerable<Item> SearchInboxUnreadFrom(string filter, int maxResults = 50)
        {

            ExtendedPropertyDefinition def = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "TempId", MapiPropertyType.String);


            // Create a search collection that contains your search conditions.
            var searchFilterCollection = new List<SearchFilter>
            {
                new SearchFilter.ContainsSubstring(EmailMessageSchema.From, filter),
                new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false)
            };

            // Create the search filter with a logical operator and your search parameters.
            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray());

            // Limit the view to 50 items.
            var view = new ItemView(maxResults)
            {
                // Limit the property set to the property ID for the base property set, and the subject and item class for the additional properties to retrieve.
                //PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.ItemClass),
                PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, def),
                // Setting the traversal to shallow will return all non-soft-deleted items in the specified folder.
                Traversal = ItemTraversal.Shallow
            };

            // Send the request to search the Inbox and get the results.
            var findResults = _service.FindItems(WellKnownFolderName.Inbox, searchFilter, view);
            return new List<Item>(findResults.Items);
        }



        public IEnumerable<EmailMessage> SearchInboxFromAddresses(string filter, int maxResults = 50)
        {

            // Create a search collection that contains your search conditions.
            var searchFilterCollection = new List<SearchFilter>
            {
                new SearchFilter.ContainsSubstring(EmailMessageSchema.From, filter),
                new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false)
            };

            // Create the search filter with a logical operator and your search parameters.
            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray());

            // Limit the view to 50 items.
            var view = new ItemView(maxResults)
            {
                // Limit the property set to the property ID for the base property set, and the subject and item class for the additional properties to retrieve.
                //PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.ItemClass),
                PropertySet = new PropertySet(BasePropertySet.IdOnly, EmailMessageSchema.From, EmailMessageSchema.Sender, ItemSchema.DateTimeReceived),
                // Setting the traversal to shallow will return all non-soft-deleted items in the specified folder.
                Traversal = ItemTraversal.Shallow
            };

            // Send the request to search the Inbox and get the results.
            var findResults = _service.FindItems(WellKnownFolderName.Inbox, searchFilter, view);
            return new List<EmailMessage>(findResults.Items.Select(x => x as EmailMessage));
        }

        public IEnumerable<EmailMessage> SearchInboxFromAddressesToday(string filter, int pageSize = 2)
        {

            // Create a search collection that contains your search conditions.
            var searchFilterCollection = new List<SearchFilter>
            {
                new SearchFilter.ContainsSubstring(EmailMessageSchema.From, filter),
                new SearchFilter.IsGreaterThanOrEqualTo(EmailMessageSchema.DateTimeReceived, DateTime.Today.AddDays(-30)),
                new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false)
            };

            // Create the search filter with a logical operator and your search parameters.
            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray());

            // Limit the view to 50 items.
            var view = new ItemView(pageSize)
            {
                // Limit the property set to the property ID for the base property set, and the subject and item class for the additional properties to retrieve.
                //PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.ItemClass),
                PropertySet = new PropertySet(BasePropertySet.IdOnly, EmailMessageSchema.From, EmailMessageSchema.Sender, ItemSchema.DateTimeReceived),
                // Setting the traversal to shallow will return all non-soft-deleted items in the specified folder.
                Traversal = ItemTraversal.Shallow
            };

            // Send the request to search the Inbox and get the results.

            var results = new List<EmailMessage>();
            var resultCount = 0;
            do
            {
                var tempResults = _service.FindItems(WellKnownFolderName.Inbox, searchFilter, view).Items.Select(x => x as EmailMessage).ToList();
                if (tempResults.Any())
                {
                    view.Offset = view.Offset + pageSize;
                    results.AddRange(tempResults);
                }
                resultCount = tempResults.Count();
            } while (resultCount > 0);
            return results;
        }



        public IEnumerable<EmailMessage> SearchInboxAllToday(int pageSize = 50)
        {

            // Create a search collection that contains your search conditions.
            var searchFilterCollection = new List<SearchFilter>
            {
                new SearchFilter.IsGreaterThanOrEqualTo(EmailMessageSchema.DateTimeReceived, DateTime.Now.AddDays(-30))
            };

            // Create the search filter with a logical operator and your search parameters.
            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray());

            // Limit the view to 50 items.
            var view = new ItemView(pageSize)
            {
                // Limit the property set to the property ID for the base property set, and the subject and item class for the additional properties to retrieve.
                //PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.ItemClass),
                PropertySet = new PropertySet(BasePropertySet.IdOnly, EmailMessageSchema.From, EmailMessageSchema.Sender, ItemSchema.DateTimeReceived),
                // Setting the traversal to shallow will return all non-soft-deleted items in the specified folder.
                Traversal = ItemTraversal.Shallow
            };

            // Send the request to search the Inbox and get the results.

            var results = new List<EmailMessage>();
            var resultCount = 0;
            do
            {
                var tempResults = _service.FindItems(WellKnownFolderName.Inbox, searchFilter, view).Items.Select(x => x as EmailMessage).ToList();
                if (tempResults.Any())
                {
                    view.Offset = view.Offset + pageSize;
                    results.AddRange(tempResults);
                }
                resultCount = tempResults.Count();
            } while (resultCount > 0);
            return results;
        }

        public IEnumerable<EmailMessage> SearchInboxUnreadToday(int pageSize = 50)
        {

            // Create a search collection that contains your search conditions.
            var searchFilterCollection = new List<SearchFilter>
            {
                new SearchFilter.IsGreaterThanOrEqualTo(EmailMessageSchema.DateTimeReceived, DateTime.Now.AddDays(-30)),
                new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false)
            };

            // Create the search filter with a logical operator and your search parameters.
            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray());

            // Limit the view to 50 items.
            var view = new ItemView(pageSize)
            {
                // Limit the property set to the property ID for the base property set, and the subject and item class for the additional properties to retrieve.
                //PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.ItemClass),
                PropertySet = new PropertySet(BasePropertySet.IdOnly, EmailMessageSchema.From, EmailMessageSchema.Sender, ItemSchema.DateTimeReceived),
                // Setting the traversal to shallow will return all non-soft-deleted items in the specified folder.
                Traversal = ItemTraversal.Shallow
            };

            // Send the request to search the Inbox and get the results.

            var results = new List<EmailMessage>();
            var resultCount = 0;
            do
            {
                var tempResults = _service.FindItems(WellKnownFolderName.Inbox, searchFilter, view).Items.Select(x => x as EmailMessage).ToList();
                if (tempResults.Any())
                {
                    view.Offset = view.Offset + pageSize;
                    results.AddRange(tempResults);
                }
                resultCount = tempResults.Count();
            } while (resultCount > 0);
            return results;
        }


        public IEnumerable<EmailMessage> SearchInboxReadToday(int pageSize = 50)
        {

            // Create a search collection that contains your search conditions.
            var searchFilterCollection = new List<SearchFilter>
            {
                new SearchFilter.IsGreaterThanOrEqualTo(EmailMessageSchema.DateTimeReceived, DateTime.Now.AddDays(-30)),
                new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, true)
            };

            // Create the search filter with a logical operator and your search parameters.
            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray());

            // Limit the view to 50 items.
            var view = new ItemView(pageSize)
            {
                // Limit the property set to the property ID for the base property set, and the subject and item class for the additional properties to retrieve.
                //PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.ItemClass),
                PropertySet = new PropertySet(BasePropertySet.IdOnly, EmailMessageSchema.From, EmailMessageSchema.Sender, ItemSchema.DateTimeReceived),
                // Setting the traversal to shallow will return all non-soft-deleted items in the specified folder.
                Traversal = ItemTraversal.Shallow
            };

            // Send the request to search the Inbox and get the results.

            var results = new List<EmailMessage>();
            var resultCount = 0;
            do
            {
                var tempResults = _service.FindItems(WellKnownFolderName.Inbox, searchFilter, view).Items.Select(x => x as EmailMessage).ToList();
                if (tempResults.Any())
                {
                    view.Offset = view.Offset + pageSize;
                    results.AddRange(tempResults);
                }
                resultCount = tempResults.Count();
            } while (resultCount > 0);
            return results;
        }

        #endregion

    }
}
