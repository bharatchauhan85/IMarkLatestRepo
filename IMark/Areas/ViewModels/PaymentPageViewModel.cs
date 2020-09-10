using Acr.UserDialogs;
using IMark.Areas.Model;
using IMark.Areas.PopUp;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
    public class PaymentPageViewModel : BasePageViewModel
    {
        private ObservableCollection<CartModel> _paymentModeList;
        public ObservableCollection<CartModel> PaymentModeList
        {
            get { return _paymentModeList; }
            set { _paymentModeList = value; RaisePropertyChanged(); }
        }
         
        private string _cardNumber; 
        public string CardNumber
        {
            get => _cardNumber;  
            set { _cardNumber = value; RaisePropertyChanged(); }
        }
        private string _cardName;
        public string CardName
        {
            get => _cardName; 
            set { _cardName = value;RaisePropertyChanged(); }
        }
        private string _expiry;
        public string Expiry
        {
            get { return _expiry; }
            set { _expiry = value;RaisePropertyChanged(); }
        }  
        private DateTime _fromMiminumDate;
        public DateTime FromMiminumDate
        {
            get { return _fromMiminumDate; }
            set { _fromMiminumDate = value;RaisePropertyChanged(); }
        }
        private string _cvv;
        public string cvv
        {
            get => _cvv; 
            set { _cvv = value;RaisePropertyChanged(); }
        }

        public PaymentPageViewModel()
        {
            FromMiminumDate = DateTime.Now;
            Expiry = null;
            PaymentModeList = GetPaymentModeList();
        }
        public ObservableCollection<CartModel> GetPaymentModeList()
        {
            return new ObservableCollection<CartModel>()
            {
                new CartModel {Image="paytick",Title="Credit/Debit" },
                new CartModel {Image="paytick",Title="PayPal" },
                new CartModel {Image="paytick",Title="Pay by phone" },
                new CartModel {Image="paytick",Title="PhonePay" }
          };
        } 

        public ICommand PaymentCommand => new Command(async (obj) =>
        { 
            if (string.IsNullOrEmpty(CardNumber))
            {
                UserDialogs.Instance.Alert("Please enter the card number.", "Error","Ok"); 
            } 
            else if (string.IsNullOrEmpty(CardName))
            {
                UserDialogs.Instance.Alert("Please enter the card name.", "Error","Ok"); 
            } 
            else if (string.IsNullOrEmpty(Expiry))
            {
                UserDialogs.Instance.Alert("Please enter the expiry date.", "Error","Ok"); 
            } 
            else if (string.IsNullOrEmpty(cvv))
            {
                UserDialogs.Instance.Alert("Please enter the cvv.", "Error","Ok"); 
            }
            else
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new SuccessPopup());
                CardNumber = string.Empty;
                CardName = string.Empty;
                Expiry = string.Empty;
                cvv = string.Empty;
            }
        });
    }
}
