using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = HubSpot.Models.Contacts.Version;

namespace HubSpot.Models
{
    public class StringItem
    {
        public StringItem() { }

        public StringItem(string item)
        {
            value = item;
        }

        public string value { get; set; }

        public List<Version> versions { get; set; }

        static public implicit operator string (StringItem item)
        {
            return item.value;
        }

        static public implicit operator StringItem(string value)
        {
            return new StringItem(value);
        }
    }
}
