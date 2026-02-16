using PRBD_Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1617_G03
{
    public partial class listReservation : UserControlBase
    {
        public ICommand ResDisplayCommand { get; }
        public ICommand ClearFilterCommand { get; }
        public ICommand NewReservationCommand { get; }

        public string VendorVisible => App.VendorVisible;

        private ObservableCollection<Client> clients;
        public ObservableCollection<Client> Clients
        {
            get => clients;
            set
            {
                clients = value;
                RaisePropertyChanged(nameof(Clients));
            }
        }

        private string filter;
        public string Filter
        {
            get => filter;
            set
            {
                if (filter == value)
                    return;

                filter = value;
                RaisePropertyChanged(nameof(Filter));
                ApplyFilterAction();
            }
        }

        private Show show;
        public Show Show
        {
            get => show;
            set
            {
                show = value;
                RaisePropertyChanged(nameof(Show));
            }
        }

        public listReservation(Show show)
        {
            Show = show;
            ClearFilterCommand = new RelayCommand(() => Filter = string.Empty);
            ResDisplayCommand = new RelayCommand<Client>(selectedClient =>
            {
                if (selectedClient != null)
                    App.Messenger.NotifyColleagues(App.MSG_DISPLAY_RES, new infoClient(selectedClient, show));
            });
            NewReservationCommand = new RelayCommand(() => App.Messenger.NotifyColleagues(App.MSG_NEW_RES, show));

            Clients = new ObservableCollection<Client>(GetBookedClients());
            App.Messenger.Register<Client>(App.MSG_RES_CHANGED, _ => ApplyFilterAction());

            InitializeComponent();
            DataContext = this;
        }

        private IQueryable<Client> GetBookedClients()
        {
            return (from client in App.Model.Client
                    join reservation in App.Model.Reservation on client.idC equals reservation.numC
                    where reservation.numS == Show.idS
                    select client).Distinct();
        }

        private void ApplyFilterAction()
        {
            IEnumerable<Client> query = GetBookedClients();

            if (!string.IsNullOrWhiteSpace(Filter))
            {
                query = query.Where(client => client.clientFName.Contains(Filter) || client.clientLName.Contains(Filter));
            }

            Clients = new ObservableCollection<Client>(query);
        }
    }
}
