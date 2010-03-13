using System.Collections.Generic;
using System.Linq;
using BlackBox.Recorder;

namespace BlackBox.Tests.Fakes
{
    public class SimpleAddressBook
    {
        private readonly List<Contact> _contacts;

        public SimpleAddressBook()
        {
            _contacts = new List<Contact>();
        }

        public void AddContact(Contact contact)
        {
            _contacts.Add(contact);
        }

        [Recording]
        public List<Contact> AllExcept(Contact contact)
        {
            return (from c in _contacts
                    where c != contact
                    select c).ToList();
        }
    }

    public class Contact
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public Contact()
        {
            
        }

        public Contact(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
