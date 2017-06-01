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
        private bool modified;
        private bool isNew;
        public bool IsExisting { get { return !IsNew; } }

        public string clientName
        {
            get { return Client.clientFName; }
            set
            {
                modified = true;
                Client.clientFName = value;
                RaisePropertyChanged(nameof(clientName));
              
            }
        }
        public string nickName
        {
            get { return Client.clientLName; }
            set
            {
                modified = true;
                Client.clientLName = value;
                RaisePropertyChanged(nameof(nickName));

            }
        }
        public DateTime? clientDate
        {
            get { return Client.bdd; }
            set
            {
                modified = true;
                Client.bdd = value;
                RaisePropertyChanged(nameof(clientDate));
            }
        }
        public decimal PriceA
        {
            get {
                
                return getPrice(info.show.idS, App.CategoryA.idCat, nbPlaceA); }
            
        }

        public decimal PriceB
        {
            get { return getPrice(info.show.idS, App.CategoryB.idCat,nbPlaceB); }
        }
        public decimal PriceC
        {
            get { return getPrice(info.show.idS, App.CategoryC.idCat, nbPlaceC); }
        }
        public Int32 nbPlaceA
        {
            get { return getPlace(info.show.idS,1); }
            set
            {
                modified = true;
                setPlace(info.show.idS, 1, Client.idC, value);
               
            }
        }
        public Int32 nbPlaceB
        {
            get { return getPlace(info.show.idS,2); }
            set
            {
                modified = true;
                setPlace(info.show.idS, 2, Client.idC, value);
               
            }
        }
        public Int32 nbPlaceC
        {
            get { return getPlace(info.show.idS, 3); }
            set
            {
                modified = true;
                setPlace(info.show.idS, 3, Client.idC, value);
              
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
           modified = false;
            Save = new RelayCommand(SaveAction, CanSaveOrCancelAction);
            Cancel = new RelayCommand(CancelAction, CanSaveOrCancelAction);
            Delete  = new RelayCommand(DeleteAction, () => { return IsExisting; });

        }


        private bool CanSaveOrCancelAction()
        {
            


            return modified ;
        }

        private void SaveAction()
        {
            if (IsNew)
            {

                App.Model.Client.Add(Client);

                IsNew = false;
            }
            App.Model.SaveChanges();
            App.Messenger.NotifyColleagues(App.MSG_RES_CHANGED, Client);
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

        private Int32 getPlace(int idS, int cat)
        {

            ICollection<Reservation> reserv = Client.Reservation;
            foreach (Reservation p in reserv)
            {
                if (reserv == null)
                    return 0;
                if (p.Category.idCat == cat && p.Show.idS==idS)
                    return p.nbr;
            }
            return 0;



        }
        private void setPlace(int idS, int cat, int idc, int val )
        {
            
            var q = from m in Client.Reservation
                    where m.Category.idCat == cat && m.Show.idS == idS 
                    select m;
            var res = q.FirstOrDefault();

            if (res != null)
            {
                res.nbr = val;
            }
            else
            {
                Reservation reserv = new Reservation();
                reserv.Category = App.Model.Category.Find(cat);
                reserv.Client = App.Model.Client.Find(idc);
                reserv.Show = App.Model.Show.Find(idS);
                reserv.nbr = val;
                Client.Reservation.Add(reserv);
            }
            RaisePropertyChanged(nameof(PriceA));
            RaisePropertyChanged(nameof(PriceB));
            RaisePropertyChanged(nameof(PriceC));
        }
        private decimal getPrice(int idS, int cat, Int32 nbPlace)
        {
            Show sh = App.Model.Show.Find(idS);
            ICollection<PriceList> priceList = sh.PriceList;
            foreach (PriceList p in priceList)
            {
                if (p.Category.idCat == cat)
                    return ((decimal)p.price*nbPlace);
            }
            return 0;



        }

        private void DeleteAction()
        {
            Client.Reservation.Clear();
            App.Model.Client.Remove(Client);
            App.Model.SaveChanges();
            App.Messenger.NotifyColleagues(App.MSG_RES_CHANGED, Client);
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
