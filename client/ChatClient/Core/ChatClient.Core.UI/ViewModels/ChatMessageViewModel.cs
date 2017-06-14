using System;

namespace ChatClient.Core.UI.ViewModels {
    public class ChatMessageViewModel : BaseViewModel {
        #region Fields

        private string _id;
        private string _image;
        private bool _isMine;
        private string _message;
        private string _name;
        private DateTime _timestamp;

        #endregion


        #region Properties

        public string Name {
            get {
                return _name;
            }
            set {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Message {
            get {
                return _message;
            }
            set {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public string Image {
            get {
                return _image;
            }
            set {
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        public bool IsMine {
            get {
                return _isMine;
            }
            set {
                _isMine = value;
                OnPropertyChanged("IsMine");
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return _timestamp;
            }

            set
            {
                _timestamp = value;
                OnPropertyChanged("Timestamp");
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        #endregion
    }
}