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
            App.Messenger.Register<Show>(App.MSG_DISPLAY_CLIENT,
                                    show =>
                                    {

                                        var tab = new TabItem()
                                        {
                                            Header = "listReservation",
                                            Content = new listReservation(show)
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
                                        
                                    });
            App.Messenger.Register<Show>(App.MSG_NEW_RES,
                                    show =>
                                   {
                                       var cl = App.Model.Client.Create();

                                       newTabForClient(new infoClient(cl,show), true);
                                       

                                       
                                   });
            App.Messenger.Register(App.MSG_NEW_SHOW,
                                 () =>
                                 {
                                     var show = App.Model.Show.Create();
                                     newTabForShow(show, true);



                                 });

            App.Messenger.Register<infoClient>(App.MSG_DISPLAY_RES,
                                   info =>
                                   {
                                       if (info != null)
                                       {
                                           var tab = (from TabItem t in tabControl.Items where (string)t.Header == info.client.clientLName+info.client.clientFName select t).FirstOrDefault();
                                           if (tab == null)
                                               newTabForClient(info, false);
                                           else
                                               Dispatcher.InvokeAsync(() => tab.Focus());
                                       }


                                   });

            App.Messenger.Register<string>(App.MSG_NAMESHOW_CHANGED, (s) =>
            {
                (tabControl.SelectedItem as TabItem).Header = s;
            });
            App.Messenger.Register<TabItem>(App.MSG_CLOSE_TAB, tab =>
            {
                tabControl.Items.Remove(tab);
            });
            App.Messenger.Register<Show>(App.MSG_DISPLAY_SHOW, show =>
            {
                if (show != null)
                {
                    var tab = (from TabItem t in tabControl.Items where (string)t.Header == show.showName select t).FirstOrDefault();
                    if (tab == null)
                        newTabForShow(show, false);
                    else
                        Dispatcher.InvokeAsync(() => tab.Focus());
                }
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
        private void newTabForClient(infoClient cl, bool isNew)
        {
            var tab = new TabItem()
            {
                Header = isNew ? "<new Reservation>" : cl.client.clientFName,
                Content = new newRes(cl, isNew)
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

