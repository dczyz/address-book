using System.Collections.Generic;
using AddressBook.Model;

namespace AddressBook.Service
{
    public interface IAddressService
    {
        void DeleteEntry(int entryId);

        ICollection<EntryModel> GetEntries(int userId);

        EntryModel SaveEntry(EntryModel entryModel);

        EntryModel UpdateEntry(EntryModel entryModel);
    }
}