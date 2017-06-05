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
        public string VendorVisible { get { return App.VendorVisible; } }
        public bool AdminReadOnly { get { return !(App.AdminReadOnly); } }
        public bool IsExisting { get { return !IsNew; } }
        public bool DateReadOnly { get { return (App.AdminReadOnly); } }

        public string clientName
        {
            get { return Client.clientFName; }
            set
            {
                modified = true;
                Client.clientFName = value;
                RaisePropertyChanged(nameof(clientName));
                Validate();
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
                Validate();

            }
        }
        public DateTime? clientDate
        {
            get { return Client.bdd; }
            set
            {
                if (isNew == false)
                    modified = true;
                Client.bdd = value;
                RaisePropertyChanged(nameof(clientDate));
               

            }
        }
        public int? postalCode
        {
            get { return Client.postalCode; }
            set
            {
                if (isNew == false)
                    modified = true;
                Client.postalCode = value;
                RaisePropertyChanged(nameof(postalCode));
               

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
        public int nbPlaceA
        {
            get { return getPlace(info.show.idS,App.CategoryA); }
            set
            {
                if (isNew == false)
                    modified = true;
                setPlace(info.show.idS, App.CategoryA, Client.idC, value);
                RaisePropertyChanged(nameof(PlaceRestanteA));
                Validate();



            }
        }
        public int nbPlaceB
        {
            get { return getPlace(info.show.idS,App.CategoryB); }
            set
            {
                if (isNew ==false)
                    modified = true;
                setPlace(info.show.idS, App.CategoryB, Client.idC, value);
                RaisePropertyChanged(nameof(PlaceRestanteB));
                Validate();



            }
        }
        public int nbPlaceC
        {
            get { return getPlace(info.show.idS, App.CategoryC); }
            set
            {
                if (isNew == false)
                    modified = true;
                setPlace(info.show.idS, App.CategoryC, Client.idC, value);
                RaisePropertyChanged(nameof(PlaceRestanteC));
                Validate();



            }
        }

        public int PlaceRestanteA { get { return placeRestante(App.CategoryA); }  }
        public int PlaceRestanteB { get { return placeRestante(App.CategoryB); }  }
        public int PlaceRestanteC { get { return placeRestante(App.CategoryC); }  }


        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                if (isNew == true)
                    Validate();
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
            
            if ( App.CategoryA.placesNumber - (nbrTotal(App.CategoryA)) <0 ||
                App.CategoryB.placesNumber - (nbrTotal(App.CategoryB)) < 0 || App.CategoryC.placesNumber - (nbrTotal(App.CategoryC)) < 0)
            {
               

                modified = false;
            }
            return modified;


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
                RaisePropertyPlace();
               

                RaisePropertyChanged(nameof(Client));
                

            }
            else
            {
                var change = (from c in App.Model.ChangeTracker.Entries<Client>()
                              where c.Entity == Client
                              select c).FirstOrDefault();
               
                if (change != null)
                {
                    change.Reload();
                    RaisePropertyPlace();
                    RaisePropertyChanged(nameof(clientName));
                    RaisePropertyChanged(nameof(nickName));
                    RaisePropertyChanged(nameof(clientDate));
                   

                }
            }
        }

        private Int32 getPlace(int idS, Category cat)
        {

            ICollection<Reservation> reserv = Client.Reservation;
            foreach (Reservation p in reserv)
            {
                if (reserv == null)
                    return 0;
                if (p.Category.idCat == cat.idCat && p.Show.idS==idS)
                    return p.nbr;
            }
            return 0;



        }
        private void setPlace(int idS, Category cat, int idc, int val )
        {
            
            var q = from m in Client.Reservation
                    where m.Category.idCat == cat.idCat && m.Show.idS == idS 
                    select m;
            var res = q.FirstOrDefault();

            if (res != null)
            {
                res.nbr = val;
            }
            else
            {
                Reservation reserv = new Reservation();
                reserv.Category = cat;
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
        private void setPlace(Category cat, Int32 val)
        {
           
            var q = from m in this.Client.Reservation
                    where m.Category.idCat == cat.idCat && info.show.idS== m.numS
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
                res.numCat = cat.idCat;
                res.nbr = val;
                this.Client.Reservation.Add(res);
            }

        }
        private int placeRestante(Category cat )
        {
           
            return cat.placesNumber - nbrTotal(cat);
        }
        private int nbrTotal(Category cat)
        {
            var res = 0;
            var query = from m in info.show.Reservations
                        where cat.idCat == m.numCat
                        select m.nbr;
            foreach (int i in query)
            {
                res += i;
            }
            return res;
        }
        private void RaisePropertyPlace()
        {
            nbPlaceA = 0;
            nbPlaceB = 0;
            nbPlaceC = 0;

            RaisePropertyChanged(nameof(nbPlaceA));
            RaisePropertyChanged(nameof(nbPlaceB));
            RaisePropertyChanged(nameof(nbPlaceC));

        }
        private  void Validate()
        {
            ClearErrors();
            if (string.IsNullOrEmpty(clientName))
            {
                AddError("clientName", "Required");
                

            }
            if (string.IsNullOrEmpty(nickName))
            {
                AddError("nickName", "Required");
               
            }
            if (postalCode < 0)
            {
                AddError("postalCode", "Can't be negative !");
               
            }
            if (nbPlaceA < 0)
            {
                AddError("nbPlaceA", "Can't be negative !");

            }
            if (nbPlaceB < 0)
            {
                AddError("nbPlaceA", "Can't be negative !");

            }
            if (nbPlaceA < 0)
            {
                AddError("nbPlaceA", "Can't be negative !");

            }
            if (nbPlaceA < 0)
            {
                AddError("nbPlaceA", "Can't be negative !");

            }
            if (nbPlaceA < 0)
            {
                AddError("nbPlaceB", "Can't be negative !");

            }
            if (nbPlaceA < 0)
            {
                AddError("nbPlaceC", "Can't be negative !");

            }






        }
    }
}
