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
        private int idUser;
        public int IdUser { get { return idUser; } set { idUser = value; } }
        public LoginView()
        {
            InitializeComponent();

            Login = new RelayCommand(LoginAction, () => { return pseudo != null && password != null && !HasErrors; });
            Cancel = new RelayCommand(() => Close());
            DataContext = this;
        }

        private User Validate()
        {
            ClearErrors();
            if (Pseudo == "admin") {
                idUser = 1;

            }else if (Pseudo == "vendor")
            {
                idUser = 2;
            }else
            {
                idUser = 0;
            }
                
           

            var member = App.Model.User.Find(idUser);
            
            if (string.IsNullOrEmpty(Pseudo))
            {
                AddError("Pseudo", Properties.Resources.Error_Required);
                Console.WriteLine("Pseudo empty");
            }
            if (Pseudo != null)
            {
                if (Pseudo.Length < 3)
                    AddError("Pseudo", Properties.Resources.Error_LengthGreaterEqual3);
                else
                {
                    if (member == null)
                        AddError("Pseudo", Properties.Resources.Error_DoesNotExist);
                }
            }

            if (string.IsNullOrEmpty(Password))
                AddError("Password", Properties.Resources.Error_Required);
            if (Password != null)
            {
                if (Password.Length < 3)
                    AddError("Password", Properties.Resources.Error_LengthGreaterEqual3);
                else if (member != null && member.pwd != Password)
                    AddError("Password", Properties.Resources.Error_WrongPassword);
            }

            RaiseErrors();

            return member;
        }

        private void LoginAction()
        {
            var member = Validate();
            if (!HasErrors)
            {
                App.CurrentUser = member;
                //ShowMainView();
                Close();
            }
        }

        //private static void ShowMainView()
        //{
        //    var mainView = new MainView();
        //    mainView.Show();
        //    Application.Current.MainWindow = mainView;
        //}

    }
}

