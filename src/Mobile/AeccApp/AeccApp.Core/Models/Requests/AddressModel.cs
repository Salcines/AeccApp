﻿using AeccApp.Core.Models.Requests;
using System.Linq;

namespace AeccApp.Core.Models
{
    public class AddressModel
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Number { get; set; }
        public string Floor { get; set; }
        public string Portal { get; set; }
        public string HospitalRoom { get; set; }
        public bool WillBeSaved { get; set; }
        public string PlaceId { get; set; }

        private Position _coordinates;

        public Position Coordinates
        {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        public string DisplayAddress
        {
            get
            {
                return string.IsNullOrEmpty(Number) ?
                  $"{Street}, {City}" :
                  $"{Street} {Number}, {City}";
            }
        }

        public override string ToString()
        {
            var address = DisplayAddress;
            return string.IsNullOrEmpty(Name) ?
                  address :
                   $"{Name} - {address}";
        }

        public AddressModel(Prediction item)
        {
            PlaceId = item.PlaceId;
            int numTerms = item.Terms.Count();

            if (numTerms<2)
            {
                return;
            }

            switch (numTerms)
            {
                case 3:
                    Street = item.Terms[0].Value;
                    break;
                case 4:
                    Street = item.Terms[0].Value;
                    if (int.TryParse(item.Terms[1].Value, out int number))
                    {
                        Number = item.Terms[1].Value;
                    }
                    break;
                default:
                    break;
            }

            City = item.Terms[numTerms - 2].Value;
        }
        public AddressModel(GooglePlacesDetailModel googlePlacesDetailModel,AddressModel modifiedAddress)
        {
            modifiedAddress.Coordinates = new Position(googlePlacesDetailModel.Result.Geometry.Location.Lat, googlePlacesDetailModel.Result.Geometry.Location.Lng);
     
            int n;
            bool thereIsNaturalNumberInput = int.TryParse(googlePlacesDetailModel.Result.AddressComponents[0].LongName, out n);

            if (thereIsNaturalNumberInput)
            {
                modifiedAddress.Number = googlePlacesDetailModel.Result.AddressComponents[0].LongName;
                modifiedAddress.Province = googlePlacesDetailModel.Result.AddressComponents[2].LongName;
            }
            else
            {
                modifiedAddress.Province = googlePlacesDetailModel.Result.AddressComponents[2].LongName;
            }
        }
        public AddressModel()
        {

        }

        public AddressModel(string name, string street, string province, string number, string floor, string placeId, Position Coordinates)
        {
            Name = name;
            Street = street;
            Province = province;
            Number = number;
            Floor = floor;
            PlaceId = placeId;
            this.Coordinates = Coordinates;
        }
    }
}
