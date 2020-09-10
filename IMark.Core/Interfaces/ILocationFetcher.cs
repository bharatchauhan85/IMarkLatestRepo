using System;
namespace IMark.Core.Interfaces
{
    public interface ILocationFetcher
    {
        System.Drawing.PointF GetCoordinates(global::Xamarin.Forms.VisualElement view);
    }
}
