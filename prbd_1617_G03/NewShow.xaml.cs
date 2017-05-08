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
        public string NameShow
        {
            get { return Show.showName; }
            set
            {
                Show.showName = value;
                RaisePropertyChanged(nameof(Show.showName));
                App.Messenger.NotifyColleagues(App.MSG_NAMESHOW_CHANGED, string.IsNullOrEmpty(value) ? "<new show>" : value);
            }
        }
        public newShow(Show show ,bool isNew)
        {
            InitializeComponent();
            DataContext = this;
            Show = show;
            IsNew = isNew;

        }
    }
}
