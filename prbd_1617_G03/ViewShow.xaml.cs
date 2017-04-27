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
    
    public partial class ViewShow : WindowBase
    {
        public ICommand NewShow { get; set; }
        
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
            DataContext = this;

            var model = new Entities();
            Shows = new ObservableCollection<Show>(model.Show);

            ClearFilter = new RelayCommand(() => { Filter = ""; });

            InitializeComponent();
        }

        public ICommand ClearFilter { get; set; }
        
        private void ApplyFilterAction()
        {
            Console.WriteLine("Search clicked! " + Filter);
            var model = new Entities();
            var query = from m in model.Show
                        where
                            m.showName.Contains(Filter) 
                        select m;
            Shows = new ObservableCollection<Show>(query);
        }
    }
   
}
