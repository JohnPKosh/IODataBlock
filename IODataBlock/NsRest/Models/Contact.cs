using System;

namespace NsRest
{
    public class Contact
    {
        public string customForm { get; set; }

        public string entityId { get; set; }

        public string contactSource { get; set; }

        public string company { get; set; }

        public string salutation { get; set; }

        public string firstName { get; set; }

        public string middleName { get; set; }

        public string lastName { get; set; }

        public string title { get; set; }

        public string phone { get; set; }

        public string fax { get; set; }

        public string email { get; set; }

        public string defaultAddress { get; set; }

        public Boolean? isPrivate { get; set; }

        public Boolean? isInactive { get; set; }

        public string subsidiary { get; set; }

        public string phoneticName { get; set; }

        //public RecordRef[] categoryList { get; set; }

        public string altEmail { get; set; }

        public string officePhone { get; set; }

        public string homePhone { get; set; }

        public string mobilePhone { get; set; }

        public string supervisor { get; set; }

        public string supervisorPhone { get; set; }

        public string assistant { get; set; }

        public string assistantPhone { get; set; }

        public string comments { get; set; }

        //public GlobalSubscriptionStatus? globalSubscriptionStatus { get; set; }

        public string image { get; set; }

        public Boolean? billPay { get; set; }

        public DateTime? dateCreated { get; set; }

        public DateTime? lastModifiedDate { get; set; }

        //public ContactAddressbookList addressbookList { get; set; }

        //public SubscriptionsList subscriptionsList { get; set; }

        //public CustomFieldRef[] customFieldList { get; set; }

        public string internalId { get; set; }

        public string externalId { get; set; }

        public String[] nullFieldList { get; set; }
    }
}