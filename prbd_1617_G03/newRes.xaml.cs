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
    
    public partial class newRes : UserControlBase
    {
        public Client Client { get; set; }
        private bool isNew;
        public bool IsExisting { get { return !IsNew; } }

        public string clientName
        {
            get { return Client.clientFName; }
            set
            {
               Client.clientFName= value;
                RaisePropertyChanged(nameof(clientName));
                App.Messenger.NotifyColleagues(App.MSG_NAMECLIENT_CHANGED, string.IsNullOrEmpty(value) ? "<new client>" : value);
            }
        }
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));

            }

        }
        public newRes(Client cl, bool isNew)
        {
            InitializeComponent();
            DataContext = this;
            Client = cl;
            IsNew = isNew;
            
        }
    }
}
