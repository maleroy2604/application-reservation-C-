using PRBD_Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1617_G03
{
    
    public partial class ViewShow : UserControlBase
    {
        public ICommand NewShowCommand { get; }
        public ICommand ShowDisplayCommand { get; }
        public ICommand ClearFilterCommand { get; }

        public string AdminVisible => App.AdminVisible;
        private ObservableCollection<Show> shows;

        public ObservableCollection<Show> Shows
        {
            get => shows;
            set
            {
                shows = value;
                RaisePropertyChanged(nameof(Shows));
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
        public ViewShow()
        {

            Shows = new ObservableCollection<Show>(App.Model.Show);

            ClearFilterCommand = new RelayCommand(() => Filter = string.Empty);
            NewShowCommand = new RelayCommand(() => App.Messenger.NotifyColleagues(App.MSG_NEW_SHOW));
            ShowDisplayCommand = new RelayCommand<Show>(selectedShow =>
            {
                if (selectedShow != null)
                    App.Messenger.NotifyColleagues(App.MSG_DISPLAY_SHOW, selectedShow);
            });

            App.Messenger.Register<Show>(App.MSG_SHOW_CHANGED, _ => ApplyFilterAction());

            InitializeComponent();
            DataContext = this;
        }

        private void ApplyFilterAction()
        {
            IEnumerable<Show> query = App.Model.Show;
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                query = from currentShow in App.Model.Show
                        where currentShow.showName.Contains(Filter) || currentShow.description.Contains(Filter)
                        select currentShow;
            }

            Shows = new ObservableCollection<Show>(query);
        }
        
        
    }
   
}
