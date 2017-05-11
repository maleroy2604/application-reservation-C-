﻿using Microsoft.Win32;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.IO;
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

    public partial class newShow : UserControlBase
    {
        public Show Show { get; set; }
        public ICommand Save { get; set; }
        public ICommand Cancel { get; set;}
        public ICommand Delete { get; set; }
        public ICommand LoadImage { get; set; }
        public ICommand ClearImage { get; set; }
        private bool isNew;
        public bool IsExisting { get { return !IsNew; } }
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));

            }

        }
        public string showName
        {
            get { return Show.showName; }
            set
            {
                Show.showName = value;
                RaisePropertyChanged(nameof(showName));
                App.Messenger.NotifyColleagues(App.MSG_NAMESHOW_CHANGED, string.IsNullOrEmpty(value) ? "<new show>" : value);
            }
        }
        public string showDate
        {
            get { return Convert.ToString(Show.showDate); }
            set
            {
                Show.showDate = Convert.ToDateTime(value);
                RaisePropertyChanged(nameof(showDate));
            }
        }
        public string description
        {
            get { return Show.description; }
            set
            {
                Show.description = value;
                RaisePropertyChanged(nameof(description));

            }
        }
        public byte[] poster
        {
            get { return Show.poster; }
            set
            {
                Show.poster = value;
                RaisePropertyChanged(nameof(poster));
            }
        }

        public newShow(Show show ,bool isNew)
        {
            InitializeComponent();
            Show = show;
            IsNew = isNew;
            Save = new RelayCommand(SaveAction,CanSaveOrCancelAction);
            Cancel = new RelayCommand(CancelAction, CanSaveOrCancelAction);
            LoadImage = new RelayCommand(LoadImageAction);
            ClearImage= new RelayCommand(ClearImageAction);

            DataContext = this;

        }
        private void SaveAction()
        {
            if(IsNew)
            {

                App.Model.Show.Add(Show);
                IsNew = false;
            }
            App.Model.SaveChanges();
            App.Messenger.NotifyColleagues(App.MSG_SHOW_CHANGED,Show);

        }
        private bool CanSaveOrCancelAction()
        {
            if (IsNew)
                return !string.IsNullOrEmpty(showName) && !HasErrors;
            var change = (from c in App.Model.ChangeTracker.Entries<Show>()
                          where c.Entity == Show
                          select c).FirstOrDefault();
            return change != null && change.State != EntityState.Unchanged;
        }
        private void CancelAction()
        {
            if (IsNew)
            {
               showName = null;
               showDate = null;
               description = null;
               poster= null;
            
                RaisePropertyChanged(nameof(Show));
            }
            else
            {
                var change = (from c in App.Model.ChangeTracker.Entries<Show>()
                              where c.Entity == Show
                              select c).FirstOrDefault();
                if (change != null)
                {
                    change.Reload();
                    RaisePropertyChanged(nameof(showDate));
                    RaisePropertyChanged(nameof(description));
                    RaisePropertyChanged(nameof(poster));
                }
            }
        }
        private void LoadImageAction()
        {
            var fd = new OpenFileDialog();
            if (fd.ShowDialog() == true)
            {
                var filename = fd.FileName;
                if (filename != null && File.Exists(filename))
                {
                    var img = System.Drawing.Image.FromFile(filename);
                    var ms = new MemoryStream();
                    var ext = System.IO.Path.GetExtension(filename).ToUpper();
                    switch (ext)
                    {
                        case ".PNG":
                            img.Save(ms, ImageFormat.Png);
                            break;
                        case ".GIF":
                            img.Save(ms, ImageFormat.Gif);
                            break;
                        case ".JPG":
                            img.Save(ms, ImageFormat.Jpeg);
                            break;
                    }
                    poster = ms.ToArray();
                    RaisePropertyChanged(nameof(poster));
                }
            }
        }
        private void ClearImageAction()
        {
            
            Show.poster = null;
            RaisePropertyChanged(nameof(poster));
        
    }

    }
    }
    

