using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsRest.Models
{
    public class Account
    {
        //public AccountType? acctType { get; set; }

        public string unitsType { get; set; }

        public string unit { get; set; }

        public string acctNumber { get; set; }

        public string acctName { get; set; }

        public Boolean? includeChildren { get; set; }

        public string currency { get; set; }

        public string exchangeRate { get; set; }

        //public ConsolidatedRate? generalRate { get; set; }

        public string parent { get; set; }

        //public ConsolidatedRate? cashFlowRate { get; set; }

        public string billableExpensesAcct { get; set; }

        public string deferralAcct { get; set; }

        public string description { get; set; }

        public Int64? curDocNum { get; set; }

        public Boolean? isInactive { get; set; }

        public string department { get; set; }

        //public string class { get; set;}

        public string location { get; set; }

        public Boolean? inventory { get; set; }

        public Boolean? eliminate { get; set; }

        //public RecordRef[] subsidiaryList { get; set; }

        public string category1099misc { get; set; }

        public Double? openingBalance { get; set; }

        public DateTime? tranDate { get; set; }

        public Boolean? revalue { get; set; }

        //public AccountTranslationList translationsList { get; set; }

        //public CustomFieldRef[] customFieldList { get; set; }

        public string internalId { get; set; }

        public string externalId { get; set; }

        public String[] nullFieldList { get; set; }


}
}
