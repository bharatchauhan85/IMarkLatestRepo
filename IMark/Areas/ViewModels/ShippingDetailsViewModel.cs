using Acr.UserDialogs;
using IMark.Areas.Views;
using IMark.Behaviors;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
   public class ShippingDetailsViewModel : BasePageViewModel
    { 
     private string _firstName;
    public string FirstName
    {
        get { return _firstName; }
        set { _firstName = value; RaisePropertyChanged(); }
    }
    private string _lastName;
    public string LastName
    {
        get { return _lastName; }
        set { _lastName = value; RaisePropertyChanged(); }
    }
    private string _mobileNumber;
    public string MobileNumber
    {
        get { return _mobileNumber; }
        set { _mobileNumber = value; RaisePropertyChanged(); }
    }
    private string _email;
    public string Email
    {
        get { return _email; }
        set { _email = value; RaisePropertyChanged(); }
    }
    private string _option;
    public string Option
    {
        get { return _option; }
        set { _option = value; RaisePropertyChanged(); }
    }
    private string _postalCode;
    public string PostalCode
    {
        get { return _postalCode; }
        set { _postalCode = value; RaisePropertyChanged(); }
    }
    private string _city;
    public string City
    {
        get { return _city; }
        set { _city = value; RaisePropertyChanged(); }
    }

    public ShippingDetailsViewModel()
    {

    }
    public ICommand ContinueCommand => new Command(async (obj) =>
    {
        if (string.IsNullOrEmpty(Email))
            UserDialogs.Instance.Alert("Please enter the email id.", "Error", "Ok");
        else if (!Regex.IsMatch(Email, RegexBehavior.emailRegex().ToString()))
            UserDialogs.Instance.Alert("Please enter the valid email id.", "Error", "Ok");
        else if (string.IsNullOrEmpty(MobileNumber))
            UserDialogs.Instance.Alert("Please enter the phone number.", "Error", "Ok"); 
       else if (string.IsNullOrEmpty(FirstName))
            UserDialogs.Instance.Alert("Please enter the first name.", "Error", "Ok");
        else if (string.IsNullOrEmpty(LastName))
            UserDialogs.Instance.Alert("Please enter the last name.", "Error", "Ok");        
        else if (string.IsNullOrEmpty(_option))
            UserDialogs.Instance.Alert("please enter apt, Unit, Suit,etc(Optional)", "Error", "Ok");
        else if (string.IsNullOrEmpty(PostalCode))
            UserDialogs.Instance.Alert("Please enter the postal code.", "Error", "Ok");
        else if (string.IsNullOrEmpty(City))
            UserDialogs.Instance.Alert("Please enter the city.", "Error", "Ok");
        else
          await App.Current.MainPage.Navigation.PushModalAsync(new ShippingConfirmation());
    });
}
}