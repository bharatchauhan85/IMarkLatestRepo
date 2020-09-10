using System;
using IMark.Areas.Authentication.ViewModels;
using IMark.Areas.PopUp;
using IMark.Areas.ViewModels;
using IMark.Areas.Views.MasterDetailsPage.ViewModels;
using IMark.Bootstrap;

namespace IMark.ViewModels
{
    public class ViewModelLocator
    {
        public readonly IoCConfig _iocConfig;
        internal void RegisterViewModels()
        {
            _iocConfig.RegisterServices();
            _iocConfig.RegisterViewModel<MainPageViewModel>();
            _iocConfig.RegisterViewModel<AppShellPageViewModel>();
            _iocConfig.RegisterViewModel<CheckInternetPageViewModel>();
            _iocConfig.RegisterViewModel<MainMenuViewModel>();
            _iocConfig.RegisterViewModel<HomeViewModel>();
            _iocConfig.RegisterViewModel<CategoryPageViewModel>();
            _iocConfig.RegisterViewModel<LoginPageViewModel>();
            _iocConfig.RegisterViewModel<RegisterPageViewModel>();
            _iocConfig.RegisterViewModel<SaveAddressPageViewModel>();
            _iocConfig.RegisterViewModel<SettingPageViewModel>();
            _iocConfig.RegisterViewModel<AddNewAddressViewModel>();
            _iocConfig.RegisterViewModel<EditSaveAddressViewModel>(); 
            _iocConfig.RegisterViewModel<ProductsPageViewModel>();
            _iocConfig.RegisterViewModel<CategoryPageViewModel>();
            _iocConfig.RegisterViewModel<NewsDetailsPageViewModel>();
            _iocConfig.RegisterViewModel<MyOrderPageViewModel>();
            _iocConfig.RegisterViewModel<MyOrderDetailsPageViewModel>();
            _iocConfig.RegisterViewModel<ProfilePageViewModel>();
            _iocConfig.RegisterViewModel<MyReviewsPageViewModel>();
            _iocConfig.RegisterViewModel<EditMyReviewsPageViewModel>();
            _iocConfig.RegisterViewModel<AccountSettinPageViewModel>();
            _iocConfig.RegisterViewModel<CartPageViewModel>();
            _iocConfig.RegisterViewModel<CartPageViewModel>();
            _iocConfig.RegisterViewModel<EditCartPageViewModel>();
            _iocConfig.RegisterViewModel<PaymentPageViewModel>();
            _iocConfig.RegisterViewModel<SuccessPopupViewModel>();
            _iocConfig.RegisterViewModel<ProductViewPageViewModel>();
            _iocConfig.RegisterViewModel<ProductDetailsPageViewModel>();
            _iocConfig.RegisterViewModel<MainMenuMasterViewModel>();
            _iocConfig.RegisterViewModel<FavoritesPageViewModel>();
            _iocConfig.RegisterViewModel<ShippingConfirmationViewModel>();
            _iocConfig.RegisterViewModel<ShippingDetailsViewModel>();
            _iocConfig.RegisterViewModel<PartnerBrandsViewModel>();
            _iocConfig.RegisterViewModel<FeaturesProductsViewModel>();

            _iocConfig.RegisterViewModel<AddPhotoViewModel>();
            _iocConfig.RegisterViewModel<OurContributionViewModel>();
            _iocConfig.RegisterViewModel<PhotoGalleryViewModel>();

            _iocConfig.RegisterViewModel<AddPhoto2ViewModel>();
            _iocConfig.RegisterViewModel<CustomizationViewModel>();
            _iocConfig.RegisterViewModel<BecomeDistributorViewModel>();
            _iocConfig.RegisterViewModel<BecomeDistributor2ViewModel>();
            _iocConfig.RegisterViewModel<NotificationPageViewModel>();
            _iocConfig.RegisterViewModel<SearchPageViewModel>();
            _iocConfig.RegisterViewModel<CatagoriesByListViewModel>();
            _iocConfig.RegisterViewModel<CatagoriesTapListViewModel>();
            _iocConfig.RegisterViewModel<UpdateAddressPageViewModel>();
            _iocConfig.RegisterViewModel<ForgetPasswordPageViewModel>();
            _iocConfig.RegisterViewModel<CollectionByListViewModel>();
            _iocConfig.RegisterViewModel<SortPageViewModel>();
            _iocConfig.RegisterViewModel<DesignLabViewModel>();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Yukon.Application.ViewModels.ViewModelLocator"/> class.
        /// </summary>
        /// <param name="iocConfig">The native IoC config implementation.</param>
        public ViewModelLocator(IoCConfig iocConfig)
        {
            _iocConfig = iocConfig;
        }
        public MainPageViewModel MainPage => GetViewModel<MainPageViewModel>();
        public AppShellPageViewModel AppShellPage => GetViewModel<AppShellPageViewModel>();
        public CheckInternetPageViewModel CheckInternetPage => GetViewModel<CheckInternetPageViewModel>();
        public MainMenuViewModel MainMenu => GetViewModel<MainMenuViewModel>();
        public HomeViewModel Home => GetViewModel<HomeViewModel>();
        public LoginPageViewModel LoginPage => GetViewModel<LoginPageViewModel>();
        public RegisterPageViewModel RegisterPage => GetViewModel<RegisterPageViewModel>();
        public SaveAddressPageViewModel SaveAddressPage => GetViewModel<SaveAddressPageViewModel>();
        public SettingPageViewModel SettingPage => GetViewModel<SettingPageViewModel>();
        public AddNewAddressViewModel AddNewAddress => GetViewModel<AddNewAddressViewModel>();
        public EditSaveAddressViewModel EditSaveAddress => GetViewModel<EditSaveAddressViewModel>(); 
        public ProductsPageViewModel ProductsPage => GetViewModel<ProductsPageViewModel>();
        public CategoryPageViewModel OffersPage => GetViewModel<CategoryPageViewModel>();
        public NewsDetailsPageViewModel NewsDetailsPage => GetViewModel<NewsDetailsPageViewModel>();
        public CategoryPageViewModel CategoryPage => GetViewModel<CategoryPageViewModel>();
        public MyOrderPageViewModel MyOrderPage => GetViewModel<MyOrderPageViewModel>();
        public MyOrderDetailsPageViewModel MyOrderDetailsPage => GetViewModel<MyOrderDetailsPageViewModel>();
        public ProfilePageViewModel ProfilePage => GetViewModel<ProfilePageViewModel>();
        public MyReviewsPageViewModel MyReviewsPage => GetViewModel<MyReviewsPageViewModel>();
        public EditMyReviewsPageViewModel EditMyReviewsPage => GetViewModel<EditMyReviewsPageViewModel>();
        public AccountSettinPageViewModel AccountSettinPage => GetViewModel<AccountSettinPageViewModel>();
        public CartPageViewModel CartPage => GetViewModel<CartPageViewModel>();
        public EditCartPageViewModel EditCartPage => GetViewModel<EditCartPageViewModel>();
        public PaymentPageViewModel PaymentPage => GetViewModel<PaymentPageViewModel>();
        public SuccessPopupViewModel SuccessPopup => GetViewModel<SuccessPopupViewModel>();
        public ProductViewPageViewModel ProductViewPage => GetViewModel<ProductViewPageViewModel>();
        public ProductDetailsPageViewModel ProductDetailsPage => GetViewModel<ProductDetailsPageViewModel>();
        public MainMenuMasterViewModel MainMenuMaster => GetViewModel<MainMenuMasterViewModel>();
        public ShippingConfirmationViewModel ShippingConfirmation => GetViewModel<ShippingConfirmationViewModel>();
        public ShippingDetailsViewModel ShippingDetails => GetViewModel<ShippingDetailsViewModel>();
        public PartnerBrandsViewModel PartnerBrands => GetViewModel<PartnerBrandsViewModel>();
        public PhotoGalleryViewModel PhotoGallery => GetViewModel<PhotoGalleryViewModel>();
        public OurContributionViewModel OurContribution => GetViewModel<OurContributionViewModel>();

        public AddPhotoViewModel AddPhoto => GetViewModel<AddPhotoViewModel>();
        public AddPhoto2ViewModel AddPhoto2 => GetViewModel<AddPhoto2ViewModel>();
        public CustomizationViewModel Customization => GetViewModel<CustomizationViewModel>();
        public BecomeDistributorViewModel BecomeDistributor => GetViewModel<BecomeDistributorViewModel>();
        public BecomeDistributor2ViewModel BecomeDistributor2 => GetViewModel<BecomeDistributor2ViewModel>();
        public FavoritesPageViewModel FavoritesPage => GetViewModel<FavoritesPageViewModel>();
        public NotificationPageViewModel NotificationPage => GetViewModel<NotificationPageViewModel>();
        public SearchPageViewModel SearchPage => GetViewModel<SearchPageViewModel>();
        public FeaturesProductsViewModel FeaturesProducts => GetViewModel<FeaturesProductsViewModel>();
        public CatagoriesByListViewModel CatagoriesByList => GetViewModel<CatagoriesByListViewModel>();
        public CatagoriesTapListViewModel CatagoriesTapList => GetViewModel<CatagoriesTapListViewModel>();
        public UpdateAddressPageViewModel UpdateAddressPage => GetViewModel<UpdateAddressPageViewModel>();
        public ForgetPasswordPageViewModel ForgetPasswordPage => GetViewModel<ForgetPasswordPageViewModel>();
        public CollectionByListViewModel CollectionByList => GetViewModel<CollectionByListViewModel>();
        public SortPageViewModel SortPage => GetViewModel<SortPageViewModel>();
        public DesignLabViewModel DesignLab => GetViewModel<DesignLabViewModel>();

        // TODO: create properties for each newly created view model
        public TViewModel GetViewModel<TViewModel>() where TViewModel : BasePageViewModel
        {
            return _iocConfig.FindViewModel<TViewModel>();
        }
    }
}