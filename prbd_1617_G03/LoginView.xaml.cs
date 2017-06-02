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

            Login = new RelayCommand(LoginAction, () => { return pseudo != null && password != null && !HasErrors; });
            Cancel = new RelayCommand(() => Close());
            DataContext = this;
        }

        private void LoginAction()
        {
            var user = Validate();
            if (!HasErrors) 
            {
                App.CurrentUser = user;
                App.AdminVisible = adminVisible();
                App.AdminReadOnly = adminReadOnly();
                App.VendorVisible = vendorVisible();


                ShowMainView(); 
                Close(); 
            }
        }

        private User Validate()
        {
            ClearErrors();
            

            var query = from m in App.Model.User where m.login == Pseudo  select m;
            var user = query.FirstOrDefault();

            if (string.IsNullOrEmpty(Pseudo))
            {
                AddError("Pseudo", "Required");
                Console.WriteLine("Pseudo empty");
            }
            if (Pseudo != null)
            {
                if (Pseudo.Length < 3)
                    AddError("Pseudo","Pseudo must contains 3 letters or more");
                else
                {
                    if (user == null)
                        AddError("Pseudo", "user does not exist");
                }
            }

            if (string.IsNullOrEmpty(Password))
                AddError("Password", "Required");
            if (Password != null)
            {
                if (Password.Length < 3)
                    AddError("Password", "Password must contains 3 letters or more");
                else if (user != null && user.pwd != Password)
                    AddError("Password", "wrong password");
            }

            RaiseErrors();

            return user;
        }
        private static void ShowMainView()
        {
            var mainView = new MainView ();
            mainView.Show();
            Application.Current.MainWindow = mainView;
        }
        private string adminVisible()
        {

            if (App.CurrentUser.admin == 1)
            {
                return "Visible";
            }
            else
            {
                return "Hidden";
            }
        }
        private string vendorVisible()
        {

            if (App.CurrentUser.admin == 1)
            {
                return "Hidden";
            }
            else
            {
                return "Visible";
            }
        }

        private bool adminReadOnly()
        {

            if (App.CurrentUser.admin == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
