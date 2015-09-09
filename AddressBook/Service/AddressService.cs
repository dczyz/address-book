using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using AddressBook.Database;
using AddressBook.Dto;
using AddressBook.Entity;
using AddressBook.Session;
using AutoMapper;

namespace AddressBook.Service
{
    class AddressService
    {
        public ICollection<EntryDto> GetEntries(int userId)
        {
            using (var db = new DatabaseContext())
            {
                return db.Entries.Where(e => e.UserId == userId).ToList().Select(MapToEntryDto).ToList();
            }
        }

        public EntryDto SaveEntry(EntryDto entryDto)
        {
            using (var db = new DatabaseContext())
            {
                var entry = MapToEntry(entryDto);
                entry.UserId = AppSession.UserId;
                db.Entries.Add(entry);
                db.SaveChanges();
                return MapToEntryDto(entry);
            }
        }

        public EntryDto UpdateEntry(EntryDto entryDto)
        {
            using (var db = new DatabaseContext())
            {
                var entry = MapToEntry(entryDto);
                entry.UserId = AppSession.UserId;
                var existingEntry = db.Entries.FirstOrDefault(e => e.Id == entry.Id);
                var addressesToDelete = existingEntry.Addresses.Where(address => entry.Addresses.All(a => a.Id != address.Id)).ToList();
                foreach (var address in entry.Addresses)
                {
                    address.Entry = existingEntry;
                    db.Addresses.AddOrUpdate(address);
                }
                db.Addresses.RemoveRange(addressesToDelete);
                db.Entries.AddOrUpdate(entry);
                db.SaveChanges();
                return MapToEntryDto(entry);
            }
        }

        public void DeleteEntry(int entryId)
        {
            using (var db = new DatabaseContext())
            {
                var entry = db.Entries.FirstOrDefault(e => e.Id == entryId);
                db.Addresses.RemoveRange(entry.Addresses);
                db.Entries.Remove(entry);
                db.SaveChanges();
            }
        }

        private static EntryDto MapToEntryDto(Entry entry)
        {
            return Mapper.Map<EntryDto>(entry);
        }

        private static Entry MapToEntry(EntryDto entryDto)
        {
            return Mapper.Map<Entry>(entryDto);
        }
    }
}
