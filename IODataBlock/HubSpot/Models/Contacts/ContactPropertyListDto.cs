using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubSpot.Models.Contacts
{
    public class ContactPropertyListDto
    {
        public DateTime? LastUpdated { get; set; }

        public List<ContactPropertyDto> Properties { get; set; }
    }
}
