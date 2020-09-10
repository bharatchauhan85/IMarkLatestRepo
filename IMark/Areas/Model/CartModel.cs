using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.Model
{
   public class CartModel : BindableObject
    {
        private int _counter;
        public int Counter
        {
            get { return _counter; }
            set { _counter = value; OnPropertyChanged(nameof(Counter)); }
        }
        
        public string Image { get; set; }
        public string Title { get; set; }
        public string ProductColor { get; set; }
        public string Qty { get; set; }
        public string Price { get; set; }
        public string PrintName { get; set; }
        public string MinusImage { get; set; }
        public string PlusImage { get; set; }

    }
}
