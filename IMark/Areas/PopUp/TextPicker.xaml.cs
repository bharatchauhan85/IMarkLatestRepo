using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMark.Areas.PopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextPicker : PopupPage
    {
        private List<Color> _colors=new List<Color>();
        private string _selectedFont;
        private Color _selectedColor=Color.Black;
        public Color SelectedColor { get=>_selectedColor;  set {
                _selectedColor = value;
                OnPropertyChanged();
            } 
        }

        public TextPicker()
        {
            InitializeComponent();
            foreach (var field in typeof(Xamarin.Forms.Color).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (field != null && !String.IsNullOrEmpty(field.Name))
                {
                    _colors.Add((Color)field.GetValue(Color.Default));
                }
            }
            //ColorCollection.ItemsSource = _colors;
            FontCollection.ItemsSource = new List<string> { 
                "Quicksand-Bold", 
                "Quicksand-Regular", 
                "Quicksand-Light", 
                "Quicksand-Medium",
                "CoventryGardenNF",
                "FancyPantsNF",
                "FontleroyBrownNF",
                "LittleLordFontleroyNF",
                "ParkLaneNF",
                "RhumbaScriptNF",
                "Precious" };


        }

        private void EmojiCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedColor =(Color) e.CurrentSelection[0];
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            if (!string.IsNullOrEmpty(TextEntry.Text))
            {
                var tpf = Utils.GetTypeface(_selectedFont + ".ttf");
                    var item = new TextInfo { Text = TextEntry.Text, TextColor = SelectedColor, Typeface= tpf };
                MessagingCenter.Send(this, "TextAdd", item);
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private void FontCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextEntry.FontFamily = (string)e.CurrentSelection[0];
            _selectedFont = (string)e.CurrentSelection[0];
        }
    }

    public class TextInfo
    {
        public Color TextColor { get; set; }
        public string Text { get; set; }
        public SKTypeface Typeface { get; set; }

    }
}