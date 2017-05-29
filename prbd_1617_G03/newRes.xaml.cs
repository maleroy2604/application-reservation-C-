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
        public infoClient info { get; set; }
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
        public string nickName
        {
            get { return Client.clientLName; }
            set
            {
                Client.clientLName = value;
                RaisePropertyChanged(nameof(nickName));
                
            }
        }
        public DateTime? clientDate
        {
            get { return Client.bdd; }
            set
            {
                Client.bdd = value;
                RaisePropertyChanged(nameof(clientDate));
            }
        }
        public decimal PriceA
        {
            get { return getPrice(info.show.idS, 1); }
        }

        public decimal PriceB
        {
            get { return getPrice(info.show.idS, 2);  }
        }
        public decimal PriceC
        {
            get { return getPrice(info.show.idS, 3);  } 
        }
        public int nbPlaceA { get; set; }
        public int nbPlaceB { get; set; }
        public int nbPlaceC { get; set; }

        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));

            }

        }
        public newRes(infoClient cl, bool isNew)
        {
            InitializeComponent();
            DataContext = this;
            Client = cl.client;
            info = cl;
            IsNew = isNew;
            
        }
        private decimal getPrice(int idS, int cat)
        {
            Show sh=App.Model.Show.Find(idS);
            ICollection<PriceList> priceList = sh.PriceList;
            foreach (PriceList p in priceList)
            {
                if (p.Category.idCat == cat)
                    return p.price;
            }
            return 0;



        }

    }
}
