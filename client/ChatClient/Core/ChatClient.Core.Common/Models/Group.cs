#region

using System;

using ChatClient.Core.Common.Models.Base;
using ChatClient.Core.Common.Resx;

using Newtonsoft.Json;

#endregion

namespace ChatClient.Core.Common.Models {
    public class Group:BaseModel {
        #region Fields

        private double _cost;
        private User _creator;
        private User _winner;
        private DateTime _dc;
        private DateTime _endDate;
        private string _id;
        private string _image= "winning_sum.png";
        private DateTime _lastDeposit;
		private string _name=AppResources.NewGroup;
        private double _nominal;
        private GroupStatus _status;
        private int _v;
        private int _views;
		private string _ownerStatus= AppResources.StatusMessage;
        private bool _ended;
        private double _greyOpacity=0;
        private ListItemStyle _itemStyle=new ListItemStyle(false);
        #endregion

        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string Name {
            get {
                return _name;
            }
            set {
                if (Equals(_name, value))
                    return;
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        [JsonProperty(PropertyName = "image")]
        public string Image {
            get {
                return _image;
            }
            set {
                if (Equals(_image, value))
                    return;
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime DC {
            get {
                return _dc;
            }
            set {
                _dc = value;
            }
        }

        [JsonProperty(PropertyName = "endAt")]
        public DateTime EndDate {
            get {
                return _endDate;
            }
            set {
                if(Equals(_endDate, value))
                    return;
                _endDate = value;
                if(value<=DateTime.UtcNow)
                    ItemStyle=new ListItemStyle(true);
                OnPropertyChanged("EndDate");
            }
        }

        [JsonProperty(PropertyName = "_id")]
        public string Id {
            get {
                return _id;
            }
            set {
                if (Equals(_id, value))
                    return;
                _id = value;
                OnPropertyChanged("Id");
                
            }
        }

        [JsonProperty(PropertyName = "cost")]
        public double Cost {
            get {
                return _cost;
            }
            set {
                if (Equals(_cost, value))
                    return;
                _cost = value;
                OnPropertyChanged("Cost");
            }
        }

        [JsonProperty(PropertyName = "_creator")]
        public User Creator {
            get {
                return _creator;
            }
            set {
                _creator = value;
            }
        }

        [JsonProperty(PropertyName = "__v")]
        public int V {
            get {
                return _v;
            }
            set {
                _v = value;
            }
        }

        [JsonProperty(PropertyName = "views")]
        public int Views {
            get {
                return _views;
            }
            set {
                if (Equals(_views, value))
                    return;
                OnPropertyChanged("Views");
                _views = value;
            }
        }
        [JsonProperty(PropertyName = "lastMember")]
        public DateTime LastDeposit {
            get {
                return _lastDeposit;
            }
            set
            {
                if (Equals(_lastDeposit, value))
                    return;
                OnPropertyChanged("LastDeposit");
                _lastDeposit = value;
            }
        }

        [JsonProperty(PropertyName = "nominal")]
        public double Nominal {
            get {
                return _nominal;
            }

            set {
                if (Equals(_nominal, value))
                    return;
                OnPropertyChanged("Nominal");
                _nominal = value;
            }
        }

        [JsonProperty(PropertyName = "status")]
        public GroupStatus Status {
            get {
                return _status;
            }

            set {
                if (Equals(_status, value))
                    return;
                OnPropertyChanged("Status");
                _status = value;
            }
        }
        [JsonProperty(PropertyName = "statusMessage")]
        public string OwnerStatus {
            get {
                return _ownerStatus;
            }
            set {
                if (Equals(_ownerStatus, value))
                    return;
                OnPropertyChanged("OwnerStatus");
                _ownerStatus = value;
            }
        }
        [JsonProperty(PropertyName = "ended")]
        public bool Ended
        {
            get
            {
                return _ended;
            }

            set
            {
                if (Equals(_ended, value))
                    return;
                if (value)
                    GreyOpacity = 0.5;
                ItemStyle=new ListItemStyle(value);
                OnPropertyChanged("Ended");
                _ended = value;
            }
        }

        public double GreyOpacity
        {
            get
            {
                return _greyOpacity;
            }

            set
            {
                if (Equals(_greyOpacity, value))
                    return;
                OnPropertyChanged("GreyOpacity");
                _greyOpacity = value;
            }
        }
        [JsonProperty(PropertyName = "currentWinner")]
        public User Winner
        {
            get
            {
                return _winner;
            }

            set
            {
                if (Equals(_winner, value))
                    return;
                OnPropertyChanged("Winner");
                _winner = value;
            }
        }

        public ListItemStyle ItemStyle
        {
            get
            {
                return _itemStyle;
            }

            set
            {
                if (Equals(_itemStyle, value))
                    return;
                OnPropertyChanged("ItemStyle");
                _itemStyle = value;
            }
        }

        #endregion
    }
}