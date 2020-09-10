using System;
using Android.Content;
using IMark.Bootstrap;
using GalaSoft.MvvmLight.Ioc;

namespace IMark.Droid
{
    public class AndroidIoCConfig : IoCConfig
    {
        private readonly Context _context;
        public AndroidIoCConfig(Context context)
        {
            _context = context;
        }
        public override void RegisterServices()
        {
            base.RegisterServices();
            //SimpleIoc.Default.Register<ISqlite, Sqlite>();
            // TODO: register other native based services here!
        }
    }
}
