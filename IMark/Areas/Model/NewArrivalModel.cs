using IMark.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
//using Image = IMark.Data.Models.Response.Image;

namespace IMark.Areas.Model
{
    public class NewArrivalModel : BindableObject
    {
        private int _counter;
        public int Counter
        {
            get { return _counter; }
            set { _counter = value; OnPropertyChanged(); }
        }
        private bool _stackLayoutIsVisible;
        public bool StackLayoutIsVisible
        {
            get { return _stackLayoutIsVisible; }
            set { _stackLayoutIsVisible = value; OnPropertyChanged(); }
        }
        private bool _stackLayoutIsVisible1;
        public bool StackLayoutIsVisible1
        {
            get { return _stackLayoutIsVisible1; }
            set { _stackLayoutIsVisible1 = value; OnPropertyChanged(); }
        }
        public ICommand MinusCommand => new Command(async (obj) =>
        {
            if (Counter > 1)
            {
                Counter--; 
            } 
        });
        public ICommand AddCartCommand => new Command(async (obj) =>
        {
             StackLayoutIsVisible1 = false;
             StackLayoutIsVisible = true;
        });
        public ICommand PlusCommand => new Command(async (obj) =>
        {
            Counter++; 
        });
        public string NewArrivalImages { get; set; }
        public string MinusImage { get; set; }
        public string PlusImage { get; set; }
        public string TitleName { get; set; }
        public string Rating { get; set; }
        public string AddProduct { get; set; }
        //public bool StackLayoutIsVisible { get; set; }
        //public bool StackLayoutIsVisible1 { get; set; }
        public string Price { get; set; }
        public string star1 { get; set; }
        public string star2 { get; set; }
        public string star3 { get; set; }
        public string star4 { get; set; }
        public string star5 { get; set; }
        public string body_html { get; set; }
        public IList<Images> images { get; set; }
        public Image image { get; set; }
        public IList<Variants> variants { get; set; }
        public string CarouselImages { get; set; }
        public string CategoriesImages { get; set; }
        public string CategoriesColor { get; set; }
        public string CategoriesName { get; set; }
        public string ProductImages { get; set; }
        public string product_type { get; set; }
        public string ProductForwardImages { get; set; }
        public string productName { get; set; }
        public string ProductPrice { get; set; }
        public string ProductColor { get; set; }
       
    }
}
