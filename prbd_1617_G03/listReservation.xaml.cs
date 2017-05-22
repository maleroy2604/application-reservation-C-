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
        //IObservable<Client> clients;
        IObservable<Client> Clients { get; set; }

        public listReservation(ObservableCollection<Client> clients)
        {

            InitializeComponent();
        }

        
    }
}
