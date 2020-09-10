using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace IMark.Helpers
{
   public class NavigationHelper
    {
        public static bool CheckType(Type type)
        {
            if(App.Current.MainPage.Navigation.NavigationStack.Last().GetType() == typeof(MasterDetailPage))
            {
               return (App.Current.MainPage.Navigation.NavigationStack.Last() as MasterDetailPage).Detail.GetType() != type;
            }
            if (App.Current.MainPage.Navigation.NavigationStack.Count > 0)
                return App.Current.MainPage.Navigation.NavigationStack.Last().GetType() != type;
            else
                return true;
        }
    }
}
