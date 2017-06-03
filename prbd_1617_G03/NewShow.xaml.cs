using Microsoft.Win32;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.IO;
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
        public ICommand ListRes{get; set;}
        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Delete { get; set; }
        public ICommand LoadImage { get; set; }
        public ICommand ClearImage { get; set; }
        public ICommand ClearFilter { get; set; }
        public bool AdminReadOnly { get { return App.AdminReadOnly; } }
        public string AdminVisible { get { return App.AdminVisible; } }
        public bool DateReadOnly { get { return !(App.AdminReadOnly); } }
        private bool modified ;

        private bool isNew;
        public bool IsExisting { get { return !IsNew; } }

        public bool IsNew
        {
            get { return isNew; }
            set
            {
                modified = true;
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));
                Validate();
            }

        }
        public string showName
        {
            get { return Show.showName; }
            set
            {
                modified = true;
                Show.showName = value;
                RaisePropertyChanged(nameof(showName));
                App.Messenger.NotifyColleagues(App.MSG_NAMESHOW_CHANGED, string.IsNullOrEmpty(value) ? "<new show>" : value);
                Validate();
            }
        }
        public DateTime showDate
        {
            get { return Show.showDate; }
            set
            {
                modified = true;
                Show.showDate = value;
                RaisePropertyChanged(nameof(showDate));
                Validate();
            }
        }
        public string description
        {
            get { return Show.description; }
            set
            {
                modified = true;
                Show.description = value;
                RaisePropertyChanged(nameof(description));
                Validate();

            }
        }
        public byte[] poster
        {
            get { return Show.poster; }
            set
            {
                modified = true;
                Show.poster = value;
                RaisePropertyChanged(nameof(poster));
                Validate();
            }
        }
        public decimal PriceA
        {
            get { return getPrice(Show.idS, App.CategoryA.idCat); }
            set {
               
                setPrice(App.CategoryA.idCat, value);
                modified = true;
                RaisePropertyChanged(nameof(PriceA));
                Validate();
            }
        }
        public decimal PriceB
        {
            get { return getPrice(Show.idS, App.CategoryB.idCat); }
            set {
                setPrice(App.CategoryB.idCat, value);
                modified = true;
                Validate();
            }
        }
        public decimal PriceC
        {
            get { return getPrice(Show.idS, App.CategoryC.idCat); }
            set {
                setPrice(App.CategoryC.idCat, value);
                modified = true;
                Validate();
            }
        }



        public newShow(Show show, bool isNew)
        {
            InitializeComponent();
            DataContext = this;
            Show = show;
            IsNew = isNew;
            modified = false;

            Save = new RelayCommand(SaveAction, CanSaveOrCancelAction);
            Cancel = new RelayCommand(CancelAction, CanSaveOrCancelAction);
            Delete = new RelayCommand(DeleteAction, () => { return IsExisting; });
            LoadImage = new RelayCommand(LoadImageAction);
            ClearImage = new RelayCommand(ClearImageAction);
            
            ListRes =  new RelayCommand<Show>(s => { App.Messenger.NotifyColleagues(App.MSG_DISPLAY_CLIENT, Show); });


        }
        private void SaveAction()
        {
            Validate();
            if (IsNew)
            {

                App.Model.Show.Add(Show);

                IsNew = false;
            }
            App.Model.SaveChanges();
            App.Messenger.NotifyColleagues(App.MSG_SHOW_CHANGED, Show);
            App.Messenger.NotifyColleagues(App.MSG_CLOSE_TAB, Parent);

        }
        private bool CanSaveOrCancelAction()
        {


            return modified ;
        }
        private void CancelAction()
        {
            if (IsNew)
            {
                showName = null;
               
                description = null;
                poster = null;
                RaisePropertyPrice();
                RaisePropertyChanged(nameof(Show));
            }
            else
            {
                var change = (from c in App.Model.ChangeTracker.Entries<Show>()
                              where c.Entity == Show
                              select c).FirstOrDefault();
                if (change != null)
                {
                    change.Reload();
                    RaisePropertyPrice();
                    RaisePropertyChanged(nameof(showDate));
                    RaisePropertyChanged(nameof(description));
                    RaisePropertyChanged(nameof(poster));
                }
            }
        }
        private void LoadImageAction()
        {
            var fd = new OpenFileDialog();
            if (fd.ShowDialog() == true)
            {
                var filename = fd.FileName;
                if (filename != null && File.Exists(filename))
                {
                    var img = System.Drawing.Image.FromFile(filename);
                    var ms = new MemoryStream();
                    var ext = System.IO.Path.GetExtension(filename).ToUpper();
                    switch (ext)
                    {
                        case ".PNG":
                            img.Save(ms, ImageFormat.Png);
                            break;
                        case ".GIF":
                            img.Save(ms, ImageFormat.Gif);
                            break;
                        case ".JPG":
                            img.Save(ms, ImageFormat.Jpeg);
                            break;
                    }
                    poster = ms.ToArray();
                    RaisePropertyChanged(nameof(poster));
                }
            }
        }
        private void ClearImageAction()
        {

            Show.poster = null;
            RaisePropertyChanged(nameof(poster));

        }
        private void DeleteAction()
        {

            Show.Reservations.Clear();
            Show.PriceList.Clear();
            App.Model.Show.Remove(Show);
            App.Model.SaveChanges();
            App.Messenger.NotifyColleagues(App.MSG_SHOW_CHANGED, Show);
            App.Messenger.NotifyColleagues(App.MSG_CLOSE_TAB, Parent);
        }
        private decimal getPrice(int idS, int cat)
        {
            
            ICollection<PriceList> priceList = Show.PriceList;
            foreach(PriceList p in priceList)
            {
                if (priceList == null)
                    return (decimal)0;
                if (p.Category.idCat == cat)
                    return p.price;
            }
            return 0;
           


        }
        private void setPrice(int idcat, decimal val)
        {
            var q = from m in this.Show.PriceList
                    where m.Category.idCat == idcat
                    select m;
            var res = q.FirstOrDefault();
            if (res != null)
            {
                res.price = val;   
            }
            else {
                PriceList price = new PriceList();
                price.Show = this.Show;
                price.Category=App.Model.Category.Find(idcat);
                price.price = val;
                Show.PriceList.Add(price); 
            }
            

        }
        private void RaisePropertyPrice()
        {
            PriceA=0;
            PriceB=0;
            PriceC=0;
            RaisePropertyChanged(nameof(PriceA));
            RaisePropertyChanged(nameof(PriceB));
            RaisePropertyChanged(nameof(PriceC));


        }
        private void Validate()
        {
            ClearErrors();
            if (string.IsNullOrEmpty(showName) )
            {
                AddError("showName", "Required");

            }
            if (string.IsNullOrEmpty(description))
            {
                AddError("description", "Required");
            }
            if (PriceA < 0)
            {
                AddError("PriceA", "Can't be negative !");
                modified = false;
            }
            if (PriceB < 0)
            {
                AddError("PriceB", "Can't be negative !");
                modified = false;

            }
            if (PriceC < 0)
            {
                AddError("PriceC", "Can't be negative !");
                modified = false;

            }
            
        }



    }
}


