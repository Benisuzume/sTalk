using sTalk.Client.Windows.Controls;
using sTalk.Libraries.Communication.Packet.Data;
using System;
using System.Collections.ObjectModel;

namespace sTalk.Client.Windows.Factories
{
    public static class ContactFactory
    {
        private static ObservableCollection<ContactInstance> _contacts = new ObservableCollection<ContactInstance>();

        public static ObservableCollection<ContactInstance> Contacts
        {
            get
            {
                return _contacts;
            }
        }

        public static void Create(User user, Action<string> sendMessage, Action<string> delete, Action<string> block)
        {
            var contact = new ContactInstance(user);
            contact.SendMessage += sendMessage;
            contact.Delete += delete;
            contact.Block += block;

            for (int i = 0; i < _contacts.Count; i++)
            {
                if (string.Compare(_contacts[i].Username, user.Name) > 0)
                {
                    _contacts.Insert(i, contact);
                    return;
                }
            }

            _contacts.Add(contact);
        }

        public static bool Delete(string username)
        {
            var contact = Find(username);
            if (contact == null)
                return false;

            return _contacts.Remove(contact);
        }

        public static ContactInstance Find(string username)
        {
            foreach (var contact in _contacts)
                if (contact.Username == username)
                    return contact;

            return null;
        }
    }
}