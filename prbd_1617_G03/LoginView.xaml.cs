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
    
    public partial class LoginView : WindowBase
    {
        public ICommand Login { get; set; }
        public ICommand Cancel { get; set; }
        private string pseudo;
        public string Pseudo { get { return pseudo; } set { pseudo = value; Validate(); } }
        private string password;
        public string Password { get { return password; } set { password = value; Validate(); } }
        public LoginView()
        {
            InitializeComponent();

            Login = new RelayCommand(LoginAction/*, () => { return pseudo != null && password != null && !HasErrors; }*/);
            Cancel = new RelayCommand(() => Close());
            DataContext = this;
        }

        private void LoginAction()
        {
            var user = Validate();
            if (!HasErrors) 
            {
                //App.CurrentUser = user;
                ShowMainView(); 
                Close(); 
            }
        }

        private static void ShowMainView()
        {
            var mainView = new MainView ();
            mainView.Show();
            Application.Current.MainWindow = mainView;
        }
    }
}
