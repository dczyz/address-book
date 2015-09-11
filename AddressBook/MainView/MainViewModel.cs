using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AddressBook.Address;
using AddressBook.Dto;
using AddressBook.Properties;
using AddressBook.Service;
using AddressBook.Session;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Application = System.Windows.Application;

namespace AddressBook.MainView
{
    class MainViewModel : ObservableObject, IPageViewModel
    {
        private string _firstName;
        private string _secondName;
        private DateTime _birthday;
        private string _phoneNumber1;
        private string _phoneNumber2;
        private string _searchText;
        private BitmapImage _photo;
        private ICommand _newCommand;
        private ICommand _searchCommand;
        private ICommand _exitCommand;
        private ObservableCollection<EntryDto> _entries;
        private ObservableCollection<EntryDto> _allEntries;
        private ICommand _selectEntryCommand;
        private EntryDto _selectedEntry;
        private ICommand _saveCommand;
        private bool _nonNewEntry;
        private ObservableCollection<AddressViewModel> _addresses;
        private ICommand _addAddressCommand;
        private AddressViewModel _currentAddress;
        private ICommand _deleteAddressCommand;
        private INavigator _navigator;
        private int? _id;
        private ICommand _loadPhotoCommand;
        private ICommand _cancelCommand;
        private ICommand _deleteCommand;
        private ICommand _deletePhotoCommand;

        public void Init()
        {
            _allEntries = new ObservableCollection<EntryDto>(ServiceLocator.AddressService.GetEntries(AppSession.UserId));
            NonNewEntry = true;
            Birthday = DateTime.Now;
            SearchText = string.Empty;
            RaisePropertyChanged(() => EntrySelected);
            Search();
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

        public ICommand ExitCommand
        {
            get
            {
                _exitCommand = _exitCommand ?? new RelayCommand(Exit);
                return _exitCommand;
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

        public EntryDto SelectedEntry
        {
            get
            {
                return _selectedEntry;
            }
            set
            {
                _selectedEntry = value;
                if (_selectedEntry == null)
                {
                    RaisePropertyChanged(() => EntrySelected);
                    return;
                }
                FirstName = _selectedEntry.FirstName;
                SecondName = _selectedEntry.SecondName;
                PhoneNumber1 = _selectedEntry.PhoneNumber1;
                PhoneNumber2 = _selectedEntry.PhoneNumber2;
                Photo = _selectedEntry.Id != null ? ServiceLocator.PhotoService.GetPhoto(_selectedEntry.Id.Value) : null;
                Birthday = _selectedEntry.Birthday;
                Addresses = new ObservableCollection<AddressViewModel>();
                _id = _selectedEntry.Id;
                foreach (var address in _selectedEntry.Addresses)
                {
                    Addresses.Add(new AddressViewModel(address));
                }
                CurrentAddress = Addresses.FirstOrDefault();
                RaisePropertyChanged(() => SelectedEntry);
                RaisePropertyChanged(() => EntrySelected);
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
                Id = _id
            };
            foreach (var addressDto in _addresses.Select(address => new AddressDto
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
                if (Photo != null)
                {
                    ServiceLocator.PhotoService.SavePhoto(Photo, entry.Id.Value);
                }
                _allEntries.Add(entry);
                Entries.Add(entry);
                SelectedEntry = entry;
            }
            else
            {
                var entry = ServiceLocator.AddressService.UpdateEntry(entryDto);
                _allEntries[_allEntries.IndexOf(SelectedEntry)] = entry;
                Entries[Entries.IndexOf(SelectedEntry)] = entry;
                SelectedEntry = entry;
                if (Photo != null)
                {
                    ServiceLocator.PhotoService.SavePhoto(Photo, SelectedEntry.Id.Value);
                }
                else
                {
                    ServiceLocator.PhotoService.DeletePhoto(_id.Value);
                }
                RaisePropertyChanged(() => Entries);
                RaisePropertyChanged(() => SelectedEntry);
            }
            NonNewEntry = true;
        }

        private static void Exit()
        {
            Application.Current.Shutdown();
        }

        private void Search()
        {
            Entries = new ObservableCollection<EntryDto>(
                _allEntries.Where(ae => ($"{ae.FirstName}{ae.SecondName}".ToLower()).Contains(SearchText.ToLower())));
        }

        private void New()
        {
            _id = null;
            NonNewEntry = false;
            SelectedEntry = null;
            FirstName = string.Empty;
            SecondName = string.Empty;
            PhoneNumber1 = string.Empty;
            PhoneNumber2 = string.Empty;
            Photo = null;
            Birthday = DateTime.Now;
            Addresses = new ObservableCollection<AddressViewModel>();
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


        public ObservableCollection<AddressViewModel> Addresses
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
            var address = new AddressViewModel(new AddressDto());
            Addresses.Add(address);
            CurrentAddress = address;
        }

        public ICommand DeleteAddressCommand
        {
            get
            {
                _deleteAddressCommand = _deleteAddressCommand ?? new RelayCommand(DeleteAddress);
                return _deleteAddressCommand;
            }
        }

        public AddressViewModel CurrentAddress
        {
            get { return _currentAddress; }
            set
            {
                _currentAddress = value;
                RaisePropertyChanged(() => CurrentAddress);
            }
        }

        private void DeleteAddress()
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

        public string Name => "Main";

        public bool EntrySelected => SelectedEntry != null || !NonNewEntry;

        public ICommand LoadPhotoCommand
        {
            get
            {
                _loadPhotoCommand = _loadPhotoCommand ?? new RelayCommand(LoadPhoto);
                return _loadPhotoCommand;
            }
        }

        private void LoadPhoto()
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
            ServiceLocator.AddressService.DeleteEntry(_id.Value);
            _allEntries.Remove(SelectedEntry);
            Entries.Remove(SelectedEntry);
            SelectedEntry = null;
        }

        public ICommand DeletePhotoCommand
        {
            get
            {
                _deletePhotoCommand = _deletePhotoCommand ?? new RelayCommand(DeletePhoto);
                return _deletePhotoCommand;;
            }
        }

        private void DeletePhoto()
        {
            Photo = null;
        }
    }
}
