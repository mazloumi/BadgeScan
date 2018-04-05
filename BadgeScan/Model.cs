using System;
using System.Collections.Generic;

namespace BadgeScan
{
    public class Contacts
    {
        public IEnumerable<Contact> value { get; set; }
    }

    public class Contact
    {
        public Guid contactid { get; set; }
        public string entityimage { get; set; }
        public string entityimage_url { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string fullname { get; set; }
    }
}
