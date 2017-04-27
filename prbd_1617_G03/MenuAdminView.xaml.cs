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
   
    public partial class MenuAdminView : WindowBase
    {
        public ICommand Show { get; set; }
        public ICommand Price { get; set; }
       
        public MenuAdminView()
        {
            InitializeComponent();
            Show = new RelayCommand(ShowView);
            Price = new RelayCommand(PriceView);
            DataContext = this;

        }
        private static void ShowView()
        {
            var ViewShow = new ViewShow();
            ViewShow.Show();
            Application.Current.MainWindow = ViewShow;
            
        }
        private static void PriceView()
        {
            var priceViewShow = new PriceViewShow();
            priceViewShow.Show();
            Application.Current.MainWindow = priceViewShow;

        }



    }
}
