using SQLite;

namespace ChatClient.Core.Common.Models
{
  public  class Parameters
    {
        private string _reFillUrl;
        private string _baseUrl;
        private int _id=1;
        public string BaseUrl {
            get {
                return _baseUrl;
            }
            set {
                _baseUrl = value;
            }
        }

        public string ReFillUrl {
            get {
                return _reFillUrl;
            }
            set {
                _reFillUrl = value;
            }
        }
        [PrimaryKey]
        public int Id {
            get {
                return _id;
            }
            set {
                _id = value;
            }
        }
    }
}
