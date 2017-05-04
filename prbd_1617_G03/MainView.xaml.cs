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
    
    public partial class MainView : WindowBase
    {
        public ICommand ViewShow { get; set; }
        

        public MainView()
        {
            InitializeComponent();
            ViewShow = new RelayCommand(() => { App.Messenger.NotifyColleagues(App.MSG_VIEW_SHOW); });
            App.Messenger.Register(App.MSG_VIEW_SHOW,
                                () =>
                                {
                                        // crée dynamiquement un nouvel onglet avec le titre "<new member>"
                                        var tab = new TabItem()
                                {
                                    Header = "<SHOW>"
                                };
                                        // ajoute ce onglet à la liste des onglets existant du TabControl
                                        tabControl.Items.Add(tab);
                                        // exécute la méthode Focus() de l'onglet pour lui donner le focus (càd l'activer)
                                        Dispatcher.InvokeAsync(() => tab.Focus());
                                });


       
        }
    }
}
