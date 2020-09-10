using System;
using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace IMark.Areas.PopUp
{
    public partial class EmojiPicker : PopupPage
    {
        public List<string> Emojies = "😀 😃 😄 😁 😆 😅 😂 🤣 ☺️ 😊 😇 🙂 🙃 😉 😌 😍 🥰 😘 😗 😙 😚 😋 😛 😝 😜 🤪 🤨 🧐 🤓 😎 🤩 🥳 😏 😒 😞 😔 😟 😕 🙁 ☹️ 😣 😖 😫 😩 🥺 😢 😭 😤 😠 😡 🤬 🤯 😳 🥵 🥶 😱 😨 😰 😥 😓 🤗 🤔 🤭 🤫 🤥 😶 😐 😑 😬 🙄 😯 😦 😧 😮 😲 🥱 😴 🤤 😪 😵 🤐 🥴 🤢 🤮 🤧 😷 🤒 🤕 🤑 🤠 😈 👿 👹 👺 🤡 💩 👻 💀 ☠️ 👽 👾 🤖 🎃 😺 😸 😹 😻 😼 😽 🙀 😿 😾".Split(new char []{ ' ' }).ToList();
        private string _selectedEmoji;

        public EmojiPicker()
        {
            InitializeComponent();
            EmojiCollection.ItemsSource = Emojies;
        }

        private void EmojiCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmojiEntry.Text += e.CurrentSelection[0] as string;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            _selectedEmoji = EmojiEntry.Text;
           await PopupNavigation.Instance.PopAsync();
            if (!string.IsNullOrEmpty(_selectedEmoji))
            {
                MessagingCenter.Send(this, "Emoji", _selectedEmoji);
            }
        }
    }
}
