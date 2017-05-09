using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace prbd_1617_G03
{
    
    public partial class newShow : UserControlBase
    {
        public Show Show { get; set; }
        public ICommand Save { get; set; }
        private bool isNew;
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));

            }

        }
        public string showName
        {
            get { return Show.showName; }
            set
            {
                Show.showName = value;
                RaisePropertyChanged(nameof(showName));
                App.Messenger.NotifyColleagues(App.MSG_NAMESHOW_CHANGED, string.IsNullOrEmpty(value) ? "<new show>" : value);
            }
        }
        public DateTime showDate
        {
            get { return Show.showDate; }
            set
            {
                Show.showDate = value;
                RaisePropertyChanged(nameof(showDate));
            }
        }
        public string description
        {
            get { return Show.description; }
            set
            {
                Show.description = value;
                RaisePropertyChanged(nameof(description));

            }
        }
        public byte[] poster
        {
            get { return Show.poster; }
            set
            {
                Show.poster = value;
                RaisePropertyChanged(nameof(poster));
            }
        }

        public newShow(Show show ,bool isNew)
        {
            InitializeComponent();
            DataContext = this;
            Show = show;
            IsNew = isNew;
            Save = new RelayCommand(SaveAction, CanSaveOrCancelAction);

        }
        
    }
}
