﻿using System;
using GalaSoft.MvvmLight;

namespace AddressBook.Model
{
     public class AddressModel : ObservableObject, ICloneable
    {
        private string _city;
        private string _postcode;
        private string _street;
        private string _homeNumber;
        private string _flatNumber;
        private string _phoneNumber1;
        private string _phonenumber2;
        private string _phonenumber3;
        private string _faxNumber;

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                RaisePropertyChanged(() => City);
            }
        }

        public string Postcode
        {
            get
            {
                return _postcode;
            }
            set
            {
                _postcode = value;
                RaisePropertyChanged(() => Postcode);
            }
        }

        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                RaisePropertyChanged(() => Street);
            }
        }

        public string HomeNumber
        {
            get { return _homeNumber; }
            set
            {
                _homeNumber = value;
                RaisePropertyChanged(() => HomeNumber);
            }
        }

        public string FlatNumber
        {
            get
            {
                return _flatNumber;
            }
            set
            {
                _flatNumber = value;
                RaisePropertyChanged(() => FlatNumber);
            }
        }

        public string PhoneNumber1
        {
            get
            {
                return _phoneNumber1;
            }
            set
            {
                _phoneNumber1 = value;
                RaisePropertyChanged(() => PhoneNumber1);
            }
        }

        public string PhoneNumber2
        {
            get { return _phonenumber2; }
            set
            {
                _phonenumber2 = value;
                RaisePropertyChanged(() => PhoneNumber2);
            }
        }

        public string PhoneNumber3
        {
            get { return _phonenumber3; }
            set
            {
                _phonenumber3 = value;
                RaisePropertyChanged(() => PhoneNumber3);
            }
        }

        public string FaxNumber
        {
            get
            {
                return _faxNumber;
            }
            set
            {
                _faxNumber = value;
                RaisePropertyChanged(() => FaxNumber);
            }
        }

        public int? Id { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
