#region

using System;
using System.Linq;

using ChatClient.Core.Common.Models.Base;
using ChatClient.Core.Common.Resx;

using Newtonsoft.Json;

using SQLite;

#endregion

namespace ChatClient.Core.Common.Models {
    public class User:BaseModel {
        #region Fields

		private string _email=AppResources.EnterEmail;
        private double _balance;
        private string _id;
        private GeoLocation _location;
		private string _nickname= AppResources.NewMember;
        private string[] _phones;
        private string _photo= "profile_avatar.png";
        private UserStatus _status;
        private string _token;
        private string[] _pushIds;
        private string _phoneString;
        private string _pushIdsString;
        private string _geo;
        #endregion

        #region Properties

        [JsonProperty(PropertyName = "image")]
        public string Photo {
            get {
                return _photo;
            }
            set {
                if(Equals(_photo, value))
                    return;
                _photo = value;
                OnPropertyChanged("Photo");
            }
        }

        [PrimaryKey]
        [JsonProperty(PropertyName = "_id")]
        public string Id {
            get {
                return _id;
            }
            set {
                _id = value;
            }
        }

        [JsonProperty(PropertyName = "token")]
        public string Token {
            get {
                return _token;
            }

            set {
                _token = value;
            }
        }

        [JsonProperty(PropertyName = "name")]
        public string Nickname {
            get {
                return _nickname;
            }

            set {
                _nickname = value;
                OnPropertyChanged("Nickname");
            }
        }

        [JsonProperty(PropertyName = "phones")]
        [Ignore]
        public string[] Phones {
            get {
                return _phones;
            }

            set {
                if (value != null) {
               
                _phoneString = String.Join(",", value);
                _phones = value;
            }
        }
        }

        [JsonProperty(PropertyName = "push_ids")]
        [Ignore]
        public string[] PushIds {
            get {
                return _pushIds;
            }

            set {
                if (value != null) {
                    _pushIdsString = String.Join(",", value);
                    _pushIds = value;
                }
            }
        }

        [JsonProperty(PropertyName = "geo")]
        [Ignore]
        public GeoLocation Location {
            get {
                return _location;
            }

            set {
                if (value != null) {
                    _geo = String.Format("{0},{1}", value.Latitude, value.Longitude);
                    _location = value;
                }
            }
        }

        [JsonProperty(PropertyName = "status")]
        public UserStatus Status {
            get {
                return _status;
            }

            set {
                _status = value;
            }
        }

        [JsonProperty(PropertyName = "balance")]
        public double Balance {
            get {
                return _balance;
            }

            set {
                if(Equals(_balance,value))
                    return;
                _balance = value;
                OnPropertyChanged("Balance");
            }
        }

        public string PhoneString
        {
            get {
               return _phoneString;
            }

            set {
                if(!string.IsNullOrEmpty(value))
                _phones = value.Split(',');
                _phoneString = value;
            }
        }

        public string PushIdsString
        {
            get
            {
                return _pushIdsString;
            }

            set {
                if (!string.IsNullOrEmpty(value))
                    _pushIds = value.Split(',');
                   _pushIdsString = value;
            }
        }

        public string Geo
        {
            get
            {
                return _geo;
            }

            set {
                if (!string.IsNullOrEmpty(value)) {
                    string[] word = value.Split(':');
                    if (word.Count() > 1)
                        _location = new GeoLocation() { Latitude = word[0], Longitude = word[1] };
                }
                _geo = value;
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                if(Equals(_email,value))
                    return;
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        #endregion
    }
}