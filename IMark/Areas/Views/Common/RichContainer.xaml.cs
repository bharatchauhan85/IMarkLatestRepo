using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace IMark.Areas.Views.Common
{
    public partial class RichContainer : ContentView
    {
        public RichContainer()
        {
            InitializeComponent();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
        }

       async void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    break;

                case GestureStatus.Running:
                    // var maxTranslationX = Content.Scale * Content.Width - Content.Width;
                    //  this.TranslationX = e.TotalX;//Math.Min(0, Math.Max(-maxTranslationX, xOffset + e.TotalX - startX));

                    //  var maxTranslationY = Content.Scale * Content.Height - Content.Height;
                    //  this.TranslationY = e.TotalY;// Math.Min(0, Math.Max(-maxTranslationY, yOffset + e.TotalY - startY));
                     
                    await this.ScaleTo(0.1, 60);

                    break;

                case GestureStatus.Completed:
                    break;
            }
        }
    }
}
