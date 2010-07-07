using System;

using System.Collections.Generic;
using System.Text;

namespace NotifyPropertyChanged
{
    [NotifyPropertyChanged]
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
