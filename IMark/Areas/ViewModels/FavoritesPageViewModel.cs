using IMark.Areas.Model;
using IMark.Areas.Views;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
   public class FavoritesPageViewModel : BasePageViewModel
    {
      
        private ObservableCollection<ProductViewModel> _favoritesList;
        public ObservableCollection<ProductViewModel> FavoritesList
        {
            get { return _favoritesList; }
            set { _favoritesList = value; RaisePropertyChanged(); }
        }
      
        public FavoritesPageViewModel()
        {
            FavoritesList = GetFavoritesList();
            foreach (var item in FavoritesList)
            {
                if (int.Parse(item.Rating) == 3)
                {
                    item.star1 = "star";
                    item.star2 = "star";
                    item.star3 = "star";
                    item.star4 = "unfilledStar";
                    item.star5 = "unfilledStar";
                }
                else if (int.Parse(item.Rating) == 4)
                {
                    item.star1 = "star";
                    item.star2 = "star";
                    item.star3 = "star";
                    item.star4 = "star";
                    item.star5 = "unfilledStar";
                }
                else if (int.Parse(item.Rating) == 5)
                {
                    item.star1 = "star";
                    item.star2 = "star";
                    item.star3 = "star";
                    item.star4 = "star";
                    item.star5 = "star";
                }
            }
        }
       
        public ObservableCollection<ProductViewModel> GetFavoritesList()
        {
            return new ObservableCollection<ProductViewModel>()
            {
                new ProductViewModel{Image= "character", Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="4",Price="21.98" },
                new ProductViewModel{Image= "img2", Favorite = "wishlist", Title ="Port Authority@ EZCotton Pique polo",Rating="5",Price="$21.98" },
                new ProductViewModel{Image= "img1",Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="4",Price="$21.98" },
                new ProductViewModel{Image= "thumbnail_foam",Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="5",Price="$21.98" },
                new ProductViewModel{Image= "character", Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="4",Price="$21.98" },
                new ProductViewModel{Image= "img2", Favorite = "wishlistgrey", Title ="Port Authority@ EZCotton Pique polo",Rating="5",Price="$21.98" },
                new ProductViewModel{Image= "img1",Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="4",Price="$29.95" },
                new ProductViewModel{Image= "thumbnail_foam",Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="5",Price="21.98" },
            };
        }
        //public ICommand FavoritesCommand => new Command(async (obj) =>
        //{
        //    var productData = obj as ProductViewModel;
        //    await App.Current.MainPage.Navigation.PushModalAsync(new ProductDetailsPage(productData));
        //});
    }
}