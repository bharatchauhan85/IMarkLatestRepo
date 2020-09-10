using IMark.Helpers;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
    public class SettingPageViewModel : BasePageViewModel
    {
        private bool _notificationIsActive;
        public bool NotificationIsActive
        {
            get { return _notificationIsActive; }
            set { _notificationIsActive = value;RaisePropertyChanged(); }
        }

        public SettingPageViewModel()
        {
            NotificationIsActive = true;
        }
    }
}
