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

        [Recording]
        public List<Contact> GetAllContacts()
        {
            var db = new SimpleAddressBookDb();
            return db.GetContacts("anything");
        }

        [Recording]
        public List<Contact> GetAllContacts(string filter)
        {
            var db = new SimpleAddressBookDb();
            return db.GetContacts(filter);
        }

        [Recording]
        public List<Contact> GetAllContacts(List<string> filters)
        {
            var db = new SimpleAddressBookDb();
            return db.GetContacts(filters.First());
        }

        [Recording]
        public List<Contact> GetAllContactsViaStatic()
        {
            return SimpleAddressBookDb.GetContactsStatic("Anything");
        }
    }

    [Dependency]
    public class SimpleAddressBookDb
    {
        public List<Contact> GetContacts(string filter)
        {
            return new List<Contact>
                       {
                           new Contact("Jonas Follesø", "jonas@follesoe.no"),
                           new Contact("John Doe", "john@doe.com")
                       };
        }

        public static List<Contact> GetContactsStatic(string filter)
        {
            return new List<Contact>
                       {
                           new Contact("Jonas Follesø", "jonas@follesoe.no"),
                           new Contact("John Doe", "john@doe.com")
                       };
        }

        public int Test()
        {
            return 10;
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
