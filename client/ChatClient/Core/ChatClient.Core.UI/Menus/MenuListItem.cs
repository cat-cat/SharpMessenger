using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Core.UI.Menus
{
    public class MenuListItem
    {
        private string _title;
        private string _iconSource;
        private Type _targetType;

        public string Title {
            get {
                return _title;
            }
            set {
                _title = value;
            }
        }

        public string IconSource {
            get {
                return _iconSource;
            }
            set {
                _iconSource = value;
            }
        }

        public Type TargetType {
            get {
                return _targetType;
            }
            set {
                _targetType = value;
            }
        }
    }
}
