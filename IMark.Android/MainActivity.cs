using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using IMark.ViewModels;
using IMark.Droid.Renderers;
using Android.Content;
using CarouselView.FormsPlugin.Android;
using Acr.UserDialogs;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;

namespace IMark.Droid
{
    [Activity(Label = "IMark", Icon = "@drawable/logo", Theme = "@style/MainTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private AndroidIoCConfig _iocConfig;
        internal static MainActivity Instance { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _iocConfig = new AndroidIoCConfig(this);
            Instance = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            var pixelWidth = (double)Resources.DisplayMetrics.WidthPixels;
            var pixelHeight = (double)Resources.DisplayMetrics.HeightPixels;
            var density = (double)Resources.DisplayMetrics.Density;
            App.MainPageScreenWidth = (double)((pixelWidth - 0.5f) / density);
            App.MainPageScreenHeight = (double)((pixelHeight - 0.5f) / density);
            App.Locator = new ViewModelLocator(_iocConfig);
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            Forms.SetFlags("IndicatorView_Experimental");
            Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CarouselViewRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: false);
            ImageCircleRenderer.Init();
            UserDialogs.Init(this);            
            XF.Material.Droid.Material.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            XamEffects.Droid.Effects.Init();
            LoadApplication(new App());
        }
        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

      
        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                System.Diagnostics.Debug.WriteLine("Android back button: There are some pages in the PopupStack");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Android back button: There are not any pages in the PopupStack");
            }
        }
        // Field, property, and method for Picture Picker
        public static readonly int PickImageId = 1000;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {
                    Android.Net.Uri uri = data.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }
    }
}