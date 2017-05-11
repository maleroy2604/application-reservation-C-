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
    
    public partial class MainView : WindowBase
    {



        public MainView()
        {
            App.Messenger.Register(App.MSG_VIEW_SHOW,
                                    () =>
                                    {

                                        var tab = new TabItem()
                                        {
                                            Header = "SHOW",
                                            Content = new ViewShow()
                                        };

                                        tabControl.Items.Add(tab);

                                        Dispatcher.InvokeAsync(() => tab.Focus());
                                    });
            App.Messenger.Register(App.MSG_VIEW_PRICE,
                                   () =>
                                   {

                                       var tab = new TabItem()
                                       {
                                           Header = "PRICE"
                                       };

                                       tabControl.Items.Add(tab);

                                       Dispatcher.InvokeAsync(() => tab.Focus());
                                   });
            App.Messenger.Register(App.MSG_NEW_SHOW,
                                   () =>
                                   {
                                       var show = App.Model.Show.Create();
                                       newTabForShow(show, true);
                                       //var tab = new TabItem()
                                       //{
                                       //    Header = "NEW SHOW",
                                       //    Content = new newShow(show, true)
                                       //};

                                       
                                   });
            App.Messenger.Register<string>(App.MSG_NAMESHOW_CHANGED, (s) =>
            {
                (tabControl.SelectedItem as TabItem).Header = s;
            });
        }
            private void newTabForShow(Show show, bool isNew)
        {
            var tab = new TabItem()
            {
                Header = isNew ? "<new show>" : show.showName,
                Content = new newShow(show, isNew)
            };
            tab.MouseDown += (o, e) =>
            {
                if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
                    tabControl.Items.Remove(o);
            };
            tab.KeyDown += (o, e) =>
            {
                if (e.Key == Key.W && Keyboard.IsKeyDown(Key.LeftCtrl))
                    tabControl.Items.Remove(o);
            };
            tabControl.Items.Add(tab);
            Dispatcher.InvokeAsync(() => tab.Focus());
        }

    }
        
    }

