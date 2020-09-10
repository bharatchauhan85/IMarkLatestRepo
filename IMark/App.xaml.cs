using System.Diagnostics;
using IMark.Core.Models.Enums;
using IMark.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Essentials;
using Xamarin.Forms;
using IMark.Areas.Views;
using Plugin.Connectivity;
using Acr.UserDialogs;
using IMark.Areas.Views.MasterDetailsPage;
using IMark.Database;
using System.IO;
using System;

namespace IMark
{
    public partial class App : Application
    {

        public const string APP_NAME = "IMark";
        public const string Access_User = "466ac9a2d9895221a83e3a189e72514e";
        public const string Access_Pass = "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";
        public static ViewModelLocator Locator { get; set; } // set by native platform
        public static AppState State = AppState.Undefinded;
        public static double MainPageScreenWidth { get; set; } //screen width
        public static double MainPageScreenHeight { get; set; }//screen height
        public Services.Interfaces.INavigationService _navigationService;
        public static SimpleIoc Container => SimpleIoc.Default;
        public static Application AppInstance => Application.Current;
               
        public App()
        {    

            InitializeComponent();
            Xamarin.Forms.PlatformConfiguration.AndroidSpecific.Application.SetWindowSoftInputModeAdjust(this, Xamarin.Forms.PlatformConfiguration.AndroidSpecific.WindowSoftInputModeAdjust.Resize);
            XF.Material.Forms.Material.Init(this);
            Device.SetFlags(new string[] { "Expander_Experimental", "IndicatorView_Experimental" });
            Debug.WriteLine("ScreenHeight = {0}; SW = {1}", MainPageScreenHeight, MainPageScreenWidth);
            Locator.RegisterViewModels();
            _navigationService = Container.GetInstance<Services.Interfaces.INavigationService>();
            _navigationService.InitializeAsync();
            //MainPage = new NavigationPage(new OffersPage());
            if (!CrossConnectivity.Current.IsConnected)
            {
                App.Current.MainPage.Navigation.PushAsync(new CheckInternetPage());
            }
            Xamarin.Essentials.Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            //App.Current.MainPage = new DesignLab();
        }
        
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
            }
            else if (e.NetworkAccess == NetworkAccess.ConstrainedInternet)
            {
                UserDialogs.Instance.Alert("Weak internet connection.");
            }
            else if (e.NetworkAccess != NetworkAccess.None)
            {
                //UserDialogs.Instance.Alert("Your are not connected to the internet.");
                App.Current.MainPage.Navigation.PushAsync(new CheckInternetPage());
            }
        }


        static NoteDatabase database;
        public static NoteDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new NoteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IMark.db3"));
                }
                return database;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            State = AppState.Foreground;
        }
        protected override void OnSleep()
        {
            // Handle when your app sleeps
            State = AppState.Background;
        }
        protected override void OnResume()
        {
            // Handle when your app resumes
            State = AppState.Foreground; 
        }
    }
}