using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using AddressBook.Address;
using AddressBook.Dto;
using AddressBook.Service;
using AddressBook.Session;
using GalaSoft.MvvmLight;

namespace AddressBook.MainView
{
    internal class MainModel : ObservableObject
    {
        private string _firstName;
        private string _secondName;
        private DateTime _birthday;
        private string _phoneNumber1;
        private string _phoneNumber2;
        private string _searchText;
        private BitmapImage _photo;
        private int? _id;
        private ObservableCollection<AddressModel> _addresses;
        private ObservableCollection<EntryDto> _entries;
        private EntryDto _selectedEntry;
        private ObservableCollection<EntryDto> _allEntries;
        
        public MainModel()
        {
            SearchText = string.Empty;
            AllEntries = new ObservableCollection<EntryDto>(ServiceLocator.AddressService.GetEntries(AppSession.UserId));
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        public string SecondName
        {
            get { return _secondName; }
            set
            {
                _secondName = value;
                RaisePropertyChanged(() => SecondName);
            }
        }

        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                RaisePropertyChanged(() => Birthday);
            }
        }

        public string PhoneNumber1
        {
            get { return _phoneNumber1; }
            set
            {
                _phoneNumber1 = value;
                RaisePropertyChanged(() => PhoneNumber1);
            }
        }

        public string PhoneNumber2
        {
            get { return _phoneNumber2; }
            set
            {
                _phoneNumber2 = value;
                RaisePropertyChanged(() => PhoneNumber2);
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                RaisePropertyChanged(() => SearchText);
            }
        }

        public BitmapImage Photo
        {
            get { return _photo; }
            set
            {
                _photo = value;
                RaisePropertyChanged(() => Photo);
            }
        }

        public ObservableCollection<AddressModel> Addresses
        {
            get
            {
                return _addresses;
            }
            set
            {
                _addresses = value;
                RaisePropertyChanged(() => Addresses);
            }
        }

        public void AddAddress()
        {
            CurrentAddress = new AddressModel(new AddressDto());
            Addresses.Add(CurrentAddress);
        }

        public AddressModel CurrentAddress { get; set; }

        public void DeleteCurrentAddress()
        {
            if (Addresses.Count > 1)
            {
                var index = Addresses.IndexOf(CurrentAddress);
                CurrentAddress = Addresses[index - 1];
                Addresses.RemoveAt(index);
            }
            else
            {
                Addresses.Remove(CurrentAddress);
                CurrentAddress = null;
            }
        }

        public EntryDto SelectedEntry
        {
            get
            {
                return _selectedEntry;
            }
            set
            {
                _selectedEntry = value;
                if (_selectedEntry != null)
                {
                    LoadEntryData();           
                }
                RaisePropertyChanged(() => SelectedEntry);
            }
        }

        private void LoadEntryData()
        {
            FirstName = _selectedEntry.FirstName;
            SecondName = _selectedEntry.SecondName;
            PhoneNumber1 = _selectedEntry.PhoneNumber1;
            PhoneNumber2 = _selectedEntry.PhoneNumber2;
            Birthday = _selectedEntry.Birthday;
             _id = _selectedEntry.Id;
            LoadAddresses();
        }

        public ObservableCollection<EntryDto> AllEntries
        {
            get { return _allEntries; }
            set
            {
                _allEntries = value;
                RaisePropertyChanged(() => AllEntries);
            }
        }

        public ObservableCollection<EntryDto> Entries
        {
            get { return _entries; }
            set
            {
                _entries = value;
                RaisePropertyChanged(() => Entries);
            }
        }

        private void LoadAddresses()
        {
            Addresses = new ObservableCollection<AddressModel>();
            foreach (var address in _selectedEntry.Addresses)
            {
                Addresses.Add(new AddressModel(address));
            }
            CurrentAddress = Addresses.FirstOrDefault();
        }
    }
}
