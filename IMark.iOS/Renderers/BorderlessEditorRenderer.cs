using System;
using System.ComponentModel;
using IMark.Controls;
using IMark.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessEditor), typeof(BorderlessEditorRenderer))]
namespace IMark.iOS.Renderers
{
    public class BorderlessEditorRenderer : EditorRenderer
    {
        public static void Init() { }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null || this.Element == null)
                return;
            Control.Layer.BorderWidth = 0;
        }
    }
}
