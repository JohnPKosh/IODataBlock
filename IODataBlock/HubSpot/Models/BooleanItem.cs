using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubSpot.Models
{
    public class BooleanItem
    {
        public BooleanItem() { }

        public BooleanItem(bool? item)
        {
            value = item;
        }

        public bool? value { get; set; }

        public List<Version> versions { get; set; }

        static public implicit operator bool? (BooleanItem item)
        {
            return item.value;
        }

        static public implicit operator bool (BooleanItem item)
        {
            return item.value ?? false;
        }

        static public implicit operator BooleanItem(bool? value)
        {
            return new BooleanItem(value);
        }

        static public implicit operator BooleanItem(bool value)
        {
            return new BooleanItem(value);
        }


        //static public implicit operator BooleanItem(bool value)
        //{
        //    return new BooleanItem(value);
        //}
    }
}
