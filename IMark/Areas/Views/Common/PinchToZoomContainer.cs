using System;
using IMark.Extenstions;
using Xamarin.Forms;

namespace IMark.Areas.Views.Common
{
    public class PinchToZoomContainer : ContentView
    {
        private const double MIN_SCALE = 1;
        private const double MAX_SCALE = 4;
        private double startScale, currentScale;
        private double startX, startY;
        private double xOffset, yOffset;

        public PinchToZoomContainer()
        {
            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinchGesture);

            var pan = new PanGestureRecognizer();
            pan.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(pan);

            TapGestureRecognizer tap = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            tap.Tapped += OnTapped;
            GestureRecognizers.Add(tap);

            Scale = MIN_SCALE;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
        }

        private void OnTapped(object sender, EventArgs e)
        {
            //if (Content.Scale > MIN_SCALE)
            //{
            //    RestoreScaleValues();
            //}
            //else
            //{
            //    Content.AnchorX = Content.AnchorY = 0.5;
            //    Content.ScaleTo(MAX_SCALE, 250, Easing.CubicInOut);
            //}
        }
        void RestoreScaleValues()
        {
            Content.ScaleTo(MIN_SCALE, 250, Easing.CubicInOut);
            Content.TranslateTo(0.5, 0.5, 250, Easing.CubicInOut);

            currentScale = 1;

            Content.TranslationX = 0.5;
            Content.TranslationY = 0.5;

            xOffset = Content.TranslationX;
            yOffset = Content.TranslationY;
        }

        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin.
                Content.TranslationX = targetX.Clamp(-Content.Width * (currentScale - 1), 0);
                Content.TranslationY = targetY.Clamp(-Content.Height * (currentScale - 1), 0);

                // Apply scale factor.
                this.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                xOffset = this.TranslationX;
                yOffset = this.TranslationY;
            }
        }

       async void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            //switch (e.StatusType)
            //{
            //    case GestureStatus.Started:
            //        startX = e.TotalX;
            //        startY = e.TotalY;
            //        Content.AnchorX = 0;
            //        Content.AnchorY = 0;
            //        break;

            //    case GestureStatus.Running:
            //        var maxTranslationX = Content.Scale * Content.Width - Content.Width;
            //        Content.TranslationX = Math.Min(0, Math.Max(-maxTranslationX, xOffset + e.TotalX - startX));

            //        var maxTranslationY = Content.Scale * Content.Height - Content.Height;
            //        Content.TranslationY = Math.Min(0, Math.Max(-maxTranslationY, yOffset + e.TotalY - startY));

            //        break;

            //    case GestureStatus.Completed:
            //        xOffset = Content.TranslationX;
            //        yOffset = Content.TranslationY;
            //        break;
            //}
            


            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    //startX = e.TotalX;
                    //startY = e.TotalY;
                    //Content.AnchorX = 0;
                    //Content.AnchorY = 0;
                    break;

                case GestureStatus.Running:
                   // var maxTranslationX = Content.Scale * Content.Width - Content.Width;
                  //  this.TranslationX = e.TotalX;//Math.Min(0, Math.Max(-maxTranslationX, xOffset + e.TotalX - startX));
               
                  //  var maxTranslationY = Content.Scale * Content.Height - Content.Height;
                  //  this.TranslationY = e.TotalY;// Math.Min(0, Math.Max(-maxTranslationY, yOffset + e.TotalY - startY));
                   await this.TranslateTo(e.TotalX, e.TotalY, 60);

                    break;

                case GestureStatus.Completed:
                    //xOffset = Content.TranslationX;
                    //yOffset = Content.TranslationY;
                    break;
            }
        }
    }
}

