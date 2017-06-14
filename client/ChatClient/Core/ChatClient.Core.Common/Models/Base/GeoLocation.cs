#region

using Newtonsoft.Json;

#endregion

namespace ChatClient.Core.Common.Models.Base {
    public class GeoLocation {
        #region Fields

        private string _latitude;
        private string _longitude;

        #endregion

        #region Properties

        [JsonProperty(PropertyName = "lat")]
        public string Latitude {
            get {
                return _latitude;
            }
            set {
                _latitude = value;
            }
        }

        [JsonProperty(PropertyName = "lng")]
        public string Longitude {
            get {
                return _longitude;
            }
            set {
                _longitude = value;
            }
        }

        #endregion
    }
}