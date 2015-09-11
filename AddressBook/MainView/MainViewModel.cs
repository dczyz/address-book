using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AddressBook.Address;
using AddressBook.Dto;
using AddressBook.Login;
using AddressBook.Properties;
using AddressBook.Service;
using AddressBook.Session;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AddressBook.MainView
{
    class MainViewModel : ObservableObject, IPageViewModel
    {
        public readonly static string Name = "Main";

        private readonly INavigator _navigator;

        private string _firstName;
        private string _secondName;
        private string _phoneNumber1;
        private string _phoneNumber2;
        private string _searchText;
        private DateTime _birthday;
        private BitmapImage _photo;
        private EntryDto _selectedEntry;
        private ObservableCollection<AddressModel> _addresses;
        private ObservableCollection<EntryDto> _entries;
        private ObservableCollection<EntryDto> _allEntries;

        private bool _nonNewEntry;

        private ICommand _newCommand;
        private ICommand _searchCommand;
        private ICommand _logoutCommand;
        private ICommand _saveCommand;
        private ICommand _addAddressCommand;
        private ICommand _deleteAddressCommand;
        private ICommand _openPhotoCommand;
        private ICommand _cancelCommand;
        private ICommand _deleteCommand;
        private ICommand _deletePhotoCommand;

        public MainViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        public void Init()
        {
            NonNewEntry = true;
            AllEntries = new ObservableCollection<EntryDto>(ServiceLocator.AddressService.GetEntries(AppSession.UserId));
            Search();
        }

        public ICommand NewCommand
        {
            get
            {
                _newCommand = _newCommand ?? new RelayCommand(New);
                return _newCommand;
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                _searchCommand = _searchCommand ?? new RelayCommand(Search);
                return _searchCommand;
            }
        }

        public ICommand LogoutCommand
        {
            get
            {
                _logoutCommand = _logoutCommand ?? new RelayCommand(Exit);
                return _logoutCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                _saveCommand = _saveCommand ?? new RelayCommand(Save);
                return _saveCommand;
            }
        }

        private void Save()
        {
            var entryDto = new EntryDto
            {
                Birthday = Birthday,
                FirstName = FirstName,
                PhoneNumber1 = PhoneNumber1,
                PhoneNumber2 = PhoneNumber2,
                SecondName = SecondName,
                Addresses = new List<AddressDto>(),
                Id = _selectedEntry?.Id
            };
            foreach (var addressDto in Addresses.Select(address => new AddressDto
            {
                PhoneNumber2 = address.PhoneNumber2,
                City = address.City,
                FaxNumber = address.FaxNumber,
                FlatNumber = address.FlatNumber,
                HomeNumber = address.HomeNumber,
                Id = address.Id,
                PhoneNumber1 = address.PhoneNumber1,
                Postcode = address.Postcode,
                Street = address.Street
            }))
            {
                entryDto.Addresses.Add(addressDto);
            }
            if (entryDto.Id == null)
            {
                var entry = ServiceLocator.AddressService.SaveEntry(entryDto);
                if (Photo != null && entry.Id != null)
                {
                    ServiceLocator.PhotoService.SavePhoto(Photo, entry.Id.Value);
                }
                AllEntries.Add(entry);
                Entries.Add(entry);
                SelectedEntry = entry;
            }
            else
            {
                var entry = ServiceLocator.AddressService.UpdateEntry(entryDto);
                AllEntries[AllEntries.IndexOf(SelectedEntry)] = entry;
                Entries[Entries.IndexOf(SelectedEntry)] = entry;
                SelectedEntry = entry;
                if (entry.Id != null)
                {
                    if (Photo != null)
                    {
                        ServiceLocator.PhotoService.SavePhoto(Photo, entry.Id.Value);
                    }
                    else
                    {
                        ServiceLocator.PhotoService.DeletePhoto(entry.Id.Value);
                    }
                }           
            }
            RaisePropertyChanged(() => Entries);
            RaisePropertyChanged(() => SelectedEntry);
            NonNewEntry = true;
        }

        private void Exit()
        {
            _navigator.Navigate(LoginViewModel.Name);
        }

        private void Search()
        {
            Entries = new ObservableCollection<EntryDto>(
            AllEntries.Where(ae => ($"{ae.FirstName}{ae.SecondName}".ToLower()).Contains(SearchText.ToLower())));
        }

        private void New()
        {
            NonNewEntry = false;
            SelectedEntry = null;
            FirstName = string.Empty;
            SecondName = string.Empty;
            PhoneNumber1 = string.Empty;
            PhoneNumber2 = string.Empty;
            Photo = null;
            Birthday = DateTime.Now;
            Addresses = new ObservableCollection<AddressModel>();
        }

        public bool NonNewEntry
        {
            get { return _nonNewEntry; }
            set
            {
                _nonNewEntry = value;
                RaisePropertyChanged(() => NonNewEntry);
                RaisePropertyChanged(() => EntrySelected);
            }
        }

        public ICommand AddAddressCommand
        {
            get
            {
                _addAddressCommand = _addAddressCommand ?? new RelayCommand(AddAddress);
                return _addAddressCommand;
            }
        }

        private void AddAddress()
        {
            CurrentAddress = new AddressModel(new AddressDto());
            Addresses.Add(CurrentAddress);
        }

        public ICommand DeleteAddressCommand
        {
            get
            {
                _deleteAddressCommand = _deleteAddressCommand ?? new RelayCommand(DeleteAddress);
                return _deleteAddressCommand;
            }
        }

        private void DeleteAddress()
        {
            DeleteCurrentAddress();
        }

        public bool EntrySelected => SelectedEntry != null || !NonNewEntry;

        public ICommand OpenPhotoCommand
        {
            get
            {
                _openPhotoCommand = _openPhotoCommand ?? new RelayCommand(OpenPhoto);
                return _openPhotoCommand;
            }
        }

        private void OpenPhoto()
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = Resources.PhotoImageFormat,
                FilterIndex = 1,
                CheckFileExists = true
            };


            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(openFileDialog.FileName);
            image.EndInit();
            Photo = image;
        }

        public ICommand CancelCommand
        {
            get
            {
                _cancelCommand = _cancelCommand ?? new RelayCommand(Cancel);
                return _cancelCommand;
            }
        }

        private void Cancel()
        {
            NonNewEntry = true;
            SelectedEntry = SelectedEntry;
        }

        public ICommand DeleteCommand
        {
            get
            {
                _deleteCommand = _deleteCommand ?? new RelayCommand(Delete);
                return _deleteCommand;
            }
        }

        private void Delete()
        {
            if (_selectedEntry.Id != null)
            {
                ServiceLocator.AddressService.DeleteEntry(_selectedEntry.Id.Value);
            }
            AllEntries.Remove(SelectedEntry);
            Entries.Remove(SelectedEntry);
            SelectedEntry = null;
        }

        public ICommand DeletePhotoCommand
        {
            get
            {
                _deletePhotoCommand = _deletePhotoCommand ?? new RelayCommand(DeletePhoto);
                return _deletePhotoCommand;
            }
        }

        private void DeletePhoto()
        {
            Photo = null;
        }

        public bool CanDeletePhoto => Photo != null;

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
