﻿using prbd_1617_G03;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

    public partial class listReservation : UserControlBase
    {
        public ICommand ResDisplay { get; set; }
        public ICommand ClearFilter { get; set; }
        public ICommand newRes { get; set; }
        public string VendorVisible { get { return App.VendorVisible; } }
        private ObservableCollection<Client> clients;
        public ObservableCollection<Client> Clients
        {
            get
            {
                
                return clients;
            }
            set
            {
               clients = value;
               RaisePropertyChanged(nameof(Clients));
            }
        }

        private string filter;
        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                ApplyFilterAction();
                RaisePropertyChanged(nameof(Clients));
            }
        }

        private Show show;
        public Show Show
        {
            get
            {
                return show;
            }
            set
            {
                show = value;
                RaisePropertyChanged(nameof(Show));
            }
        }


        public listReservation(Show show)
        {
            

           Show = show;
           ClearFilter = new RelayCommand(() => { Filter = ""; });
           Clients =new ObservableCollection<Client>(getBookedClients());
           ResDisplay = new RelayCommand<Client>(m => { App.Messenger.NotifyColleagues(App.MSG_DISPLAY_RES, new infoClient(m,show)); });
            newRes= new RelayCommand(() => { App.Messenger.NotifyColleagues(App.MSG_NEW_RES,show); });
            App.Messenger.Register<Client>(App.MSG_RES_CHANGED, client => { /*if (!Clients.Contains(client)) Clients.Add(client);*/ApplyFilterAction(); });
            InitializeComponent();
            DataContext = this;
        }

        private IQueryable<Client> getBookedClients()
        {
            var query = (from c in App.Model.Client
                         join r in App.Model.Reservation on c.idC equals r.numC
                         where r.numS == Show.idS
                         
                         select c ).Distinct();
            return query;
        }
        private void ApplyFilterAction()
        {
            IEnumerable<Client> query = (from c in App.Model.Client
                                         join r in App.Model.Reservation on c.idC equals r.numC
                                         where r.numS == Show.idS
                                         select c).Distinct();

            
            if (!string.IsNullOrEmpty(Filter))
                query = (from c in App.Model.Client
                         join r in App.Model.Reservation on c.idC equals r.numC
                         where r.numS == Show.idS && (c.clientFName.Contains(Filter) || c.clientLName.Contains(Filter))
                         select c).Distinct();
            
            Clients = new ObservableCollection<Client>(query);
        }


    }
}
