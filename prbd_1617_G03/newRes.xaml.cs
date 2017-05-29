using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Delete { get; set; }
        private bool nbModified;
        private bool isNew;
        public bool IsExisting { get { return !IsNew; } }

        public string clientName
        {
            get { return Client.clientFName; }
            set
            {
                Client.clientFName = value;
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
            get { return getPrice(info.show.idS, 2); }
        }
        public decimal PriceC
        {
            get { return getPrice(info.show.idS, 3); }
        }
        public Int32 nbPlaceA
        {
            get { return getPlace(Client.); }
            set
            {

                nbModified = true;
            }
        }
        public Int32 nbPlaceB
        {
            get { return nbPlaceB; }
            set
            {

                nbModified = true;
            }
        }
        public Int32 nbPlaceC
        {
            get { return nbPlaceC; }
            set
            {

                nbModified = true;
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

        public newRes(infoClient cl, bool isNew)
        {
            InitializeComponent();
            DataContext = this;
            Client = cl.client;
            info = cl;
            IsNew = isNew;
            nbModified = false;
            Save = new RelayCommand(SaveAction, CanSaveOrCancelAction);
            Cancel = new RelayCommand(CancelAction, CanSaveOrCancelAction);
            Delete = Delete = new RelayCommand(DeleteAction, () => { return IsExisting; });

        }


        private bool CanSaveOrCancelAction()
        {
            if (IsNew)
                return !string.IsNullOrEmpty(clientName) && !HasErrors;
            var change = (from c in App.Model.ChangeTracker.Entries<Client>()
                          where c.Entity == Client
                          select c).FirstOrDefault();


            return nbModified || change != null && change.State != EntityState.Unchanged;
        }

        private void SaveAction()
        {
            if (IsNew)
            {

                App.Model.Client.Add(Client);

                IsNew = false;
            }
            App.Model.SaveChanges();
            //App.Messenger.NotifyColleagues(App.MSG_SHOW_CHANGED, Show);
            App.Messenger.NotifyColleagues(App.MSG_CLOSE_TAB, Parent);

        }
        private void CancelAction()
        {
            if (IsNew)
            {
                clientName = null;
                nickName = null;
                clientDate = null;

                RaisePropertyChanged(nameof(Show));
            }
            else
            {
                var change = (from c in App.Model.ChangeTracker.Entries<Client>()
                              where c.Entity == Client
                              select c).FirstOrDefault();
                if (change != null)
                {
                    change.Reload();
                    RaisePropertyChanged(nameof(clientName));
                    RaisePropertyChanged(nameof(nickName));
                    RaisePropertyChanged(nameof(clientDate));
                }
            }
        }

        private Int32 getPlace(int idS,int cat, int idc  )
        {
           Reservation reservation = App.Model.Reservation.Find(idS,cat,idc);
            if (cat == reservation.numCat)
            {
                return reservation.nbr;
            }
            return 0;


        }
        private decimal getPrice(int idS, int cat)
        {
            Show sh = App.Model.Show.Find(idS);
            ICollection<PriceList> priceList = sh.PriceList;
            foreach (PriceList p in priceList)
            {
                if (p.Category.idCat == cat)
                    return p.price;
            }
            return 0;



        }

        private void DeleteAction()
        {



            App.Model.Client.Remove(Client);
            App.Model.SaveChanges();

            App.Messenger.NotifyColleagues(App.MSG_CLOSE_TAB, Parent);
        }
        private void setPlace(int idcat, Int32 val)
        {
            var q = from m in this.Client.Reservation
                    where m.Category.idCat == idcat
                    select m;
            var res = q.FirstOrDefault();
            if (res != null)
            {
                res.nbr = val;
            }
            else
            {
                Reservation reservation = new Reservation();
                res.numC = Client.idC;
                res.numCat = idcat;
                res.nbr = val;
                this.Client.Reservation.Add(res);
            }

        }
    }
}
