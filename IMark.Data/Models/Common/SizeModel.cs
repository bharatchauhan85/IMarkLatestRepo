using IMark.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace IMark.Data.Models.Common
{
    public class SizeModel : SelectedOption, INotifyPropertyChanged
    {
        private string _borderCol;

        public string BorderCol
        {
            get { return _borderCol; }
            set { _borderCol = value;NotifyPropertyChanged(nameof(BorderCol)); }
        }

        public string varientId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
