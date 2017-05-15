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
    
    public partial class ViewShow : UserControlBase
    {
        public ICommand NewShow { get; set; }
        public ICommand ShowDisplay { get; set; }
        private ObservableCollection<Show> shows;
       

        public ObservableCollection<Show> Shows
        {
            get
            {
                return shows;
            }
            set
            {
                shows = value;
                RaisePropertyChanged(nameof(Shows));
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
                RaisePropertyChanged(nameof(Filter));
            }
        }
        public ViewShow()
        {
            

           
            Shows = new ObservableCollection<Show>(App.Model.Show);

            ClearFilter = new RelayCommand(() => { Filter = ""; });
            NewShow = new RelayCommand(() => { App.Messenger.NotifyColleagues(App.MSG_NEW_SHOW); });
            App.Messenger.Register<Show>(App.MSG_SHOW_CHANGED, show => { ApplyFilterActionbis(); });
            ShowDisplay = new RelayCommand<Show>(m => { App.Messenger.NotifyColleagues(App.MSG_DISPLAY_SHOW, m); });
            InitializeComponent();

           
            DataContext = this;
        }

        public ICommand ClearFilter { get; set; }

        private void ApplyFilterAction()
        {

            var model = new Entities();
            var query = from m in App.Model.Show
                        where
                            m.showName.Contains(Filter)
                        select m;
            Shows = new ObservableCollection<Show>(query);
        }
        private void ApplyFilterActionbis()
        {
            IEnumerable<Show> query = App.Model.Show;
            if (!string.IsNullOrEmpty(Filter))
                query = from m in App.Model.Show
                        where
                            m.showName.Contains(Filter) || m.description.Contains(Filter) 
                        select m;
            Shows = new ObservableCollection<Show>(query);
        }
    }
   
}
